using System.Runtime.CompilerServices;

namespace UniversityClassLibrary.FileWork;

class FileProcess<T> : FileSimple
{
    // Описание делегатов, используемых для выполнения обработки прочитанных данных
    public delegate void ProcessBuf<Type>(byte[] Buf, int BufLen, ref Type CurVal);

    public delegate void ProcessVal<Type>(Type CurVal, ref Type Val);

    public bool IsOverl { get; private set; }


    // Класс, массив экземпляров которых используется
    // при реализации асинхронного и синхронного режимов работы
    public class StateObject
    {
        // буфер для чтения данных
        public byte[] Buf; 
         // экземпляр класса FileStream, используется только для асинхронного режима
        public FileStream fs;
        // Логическое значение, показывающее завершение отложенной операции ввода-вывода.
        // Используется только для асинхронного режима.
        // Ключевое слово volatile используется для отключения попыток компилятора
        // при оптимизации сохраниться значение поля в регистре. Это необходимо для
        // корректной работы release версии проекта 
        public volatile bool EndInOut;
        // Количество прочитанных из потока байт
        public int ReadBytes;

        // Конструктор с параметрами
        public StateObject(int BufSz, FileStream fsVal)
        {
            Buf = new byte[BufSz];
            fs = fsVal;
        }
    }

    // Массив объектов состояния
    private StateObject[] States;

    // Метод инициализации массива объектов состояния
    private void InitBufs(bool IsOverl = false)
    {
        // Количество объектов состояния массива зависит от режима работы.
        int NStates = IsOverl ? 2 : 1;
        States = new StateObject[NStates];

        // Инициализация элементов массива
        for (; NStates > 0; NStates--)
            States[NStates - 1] = new StateObject(BufLen, fIn);
    }

    // Конструктор с параметрами.
    // 1-ый аргумент  имя файла, 2-ой режим работы: false  синхронный, а true  асинхронный
    public FileProcess(string FNIn, bool IsOverl = false, int BufSize = FileBase.DefBufSize)
    {
        this.IsOverl = IsOverl;
        if (Init(FNIn, IsOverl))
        {
            BufLen = BufSize;
            InitBufs(IsOverl);
        }
    }

    // Метод чтения и обработки данных в синхронном режиме.
    // Аргументами являются 2 делегата:
    // ProcBuf  обработка прочитанного блока данных и
    // получение в результате одного значения, ProcVal  обработка полученного
    // для каждого блока значения с целью формирования единого значения для всего файла.
    // 3-ий аргумент представляет собой значение, которое будет сформировано в результате обработки файла.
    public bool ProcessFilesSync(ProcessBuf<T> ProcBuf, ProcessVal<T> ProcVal, ref T CalcVal)
    {
        long FileLen;
        int NProcess;
        //Присваивание значения по умолчанию для переменной,
        //получающей результат обработки каждого блока файла
        T CurCalcVal = default(T);
        bool Res = false;

        // Проверка возможности обработки файла
        if (CanWork() && (FileLen = GetInputFileSize()) != 0 && IsOverl == false)
        {

            // Считывание и обработка первого блока.
            // 1-ый аргумент метода задает буфер для чтения,
            // 2-ой  начальное смещение в этом буфере,
            // 3-ий  количество элементов, количество байт, которое планируется считать.
            // Метод возвращает количество прочитанных байт.
            // Для записи используется аналогичный метод Write, который не возвращает никаких значений.
            NProcess = fIn.Read(States[0].Buf, 0, BufLen);
            ProcBuf(States[0].Buf, NProcess, ref CalcVal);
            Res = true;

            // Считывание и обработка всех остальных блоков
            for (FileLen -= BufLen; FileLen >= 0; FileLen -= BufLen)
            {
                NProcess = fIn.Read(States[0].Buf, 0, BufLen);
                ProcBuf(States[0].Buf, NProcess, ref CurCalcVal);
                ProcVal(CurCalcVal, ref CalcVal);
            }
        }

        FreeRes();

        return Res;
    }

    // Метод, используемый как подстановка для делегата,
    // и вызывающийся при окончании каждой операции ввода в асинхронном режиме
    static void EndOfRead(IAsyncResult Result)
    {
        StateObject Obj = (StateObject)Result.AsyncState;

        // В конце каждой асинхронной операции ввода-вывода должен быть обязательно
        // вызван метод EndRead (при чтении) или EndWrite (при записи)
        Obj.ReadBytes = Obj.fs.EndRead(Result);
        Obj.EndInOut = true;
    }


    // Метод чтения и обработки данных в асинхронном режиме.
    // Аргументами являются 2 делегата:
    // ProcBuf  обработка прочитанного блока данных и
    // получение в результате одного значения, ProcVal  обработка полученного
    // для каждого блока значения с целью формирования единого значения для всего файла.
    // 3-ий аргумент представляет собой значение, которое будет сформировано в результате обработки файла.
    public bool ProcessFilesAsync(ProcessBuf<T> ProcBuf, ProcessVal<T> ProcVal, ref T CalcVal)
    {
        long FileLen;
        int NProcess;

        // Присваивание значения по умолчанию для переменной,
        // получающей результат обработки каждого блока файла
        T CurCalcVal = default(T);
        bool Res = false;
        byte nBuf = 0, nBufNew = 0, nLastBuf = 1;

        // Microsoft рекомендует описывать переменную типа AsyncCallback
        // и инициализировать ее при помощи имени определенного метода,
        // описанного как подстановка для делегата в методе BeginRead
        // AsyncCallback ReadAsyncCB = new AsyncCallback(EndOfRead);

        // Проверка возможности обработки файла
        if (CanWork() && (FileLen = GetInputFileSize()) != 0 && IsOverl)
        {
            // Начальная инициализация флагов массива состояний
            for (byte i = 0; i <= nLastBuf; i++)
                States[i].EndInOut = false;


            // Считывание и обработка первого блока.
            // 1-ый аргумент метода задает буфер для чтения,
            // 2-ой  начальное смещение в этом буфере,
            // 3-ий  количество элементов, количество байт, которое планируется считать,
            // 4-ый делегат, который вызывается в конце выполнения асинхронной операции чтения,
            // 5-ый аргумент  экземпляр класса, описывающего состояние данной операции ввода-вывода.
            // Для записи используется аналогичный метод BeginWrite.
            fIn.BeginRead(States[nBuf].Buf, 0, BufLen, EndOfRead, States[nBuf]);
            FileLen -= BufLen;


            // Запускается цикл ожидания завершения отложенной операции ввода.
            // Пока не будет вызван делегат, в котором значение флажка EndInOut
            // изменится на противоположное, то цикл будет продолжать работать.
            while (States[nBuf].EndInOut == false) ;
            ProcBuf(States[nBuf].Buf, States[nBuf].ReadBytes, ref CalcVal);

            // Если не конец файла
            Res = true;
            if (FileLen > 0)
            {
                // Считывание и обработка второго блока.
                States[nBuf].EndInOut = false;
                fIn.BeginRead(States[nBuf].Buf, 0, BufLen, EndOfRead, States[nBuf]);
                FileLen -= BufLen;
                while (States[nBuf].EndInOut == false) ;

                do
                {
                    // Переключение на использование другого элемента состояний массива
                    nBufNew = (byte)(nBuf ^ 1);
                    States[nBufNew].EndInOut = false;
                    // Получение количества прочитанных байт предыдущей операции чтения
                    NProcess = States[nBuf].ReadBytes;
                    // Если не конец файла
                    if (FileLen <= 0)
                    {
                        // Обработка последнего блока
                        ProcBuf(States[nBuf].Buf, NProcess, ref CurCalcVal);
                        ProcVal(CurCalcVal, ref CalcVal);
                        break;
                    }

                    // Запуск следующей операции чтения
                    fIn.BeginRead(States[nBufNew].Buf, 0, BufLen, EndOfRead, States[nBufNew]);
                    FileLen -= BufLen;

                    // Обработка последнего прочитанного блока
                    ProcBuf(States[nBuf].Buf, NProcess, ref CurCalcVal);
                    ProcVal(CurCalcVal, ref CalcVal);
                    nBuf = nBufNew;

                    // Ожидание завершения последней отложенной операции
                    while (States[nBufNew].EndInOut == false) ;
                } while (true);
            }
        }

        FreeRes();
        return Res;
    }

    // данный метод необходим, т.к. для использования await необходим метод, описанный с 
    // ключевым словом async, а такой метод не может иметь ref/out аргументов, которые есть в
    // ProcessFilesAsyncNet45
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private async Task<int> WaitForReadEnd(Task<int> GetDataPart)
    {
        return await GetDataPart;
    }

    // Структура метод похожа на структуру ProcessFilesAsync. Однако используется ReadAsync
    // для чтения данных из файла и WaitForReadEnd совместно с GetDataPart − для ожидания 
    // завершения отложенной операции чтения.
    public bool ProcessFilesAsyncNet45(ProcessBuf<T> ProcBuf, ProcessVal<T> ProcVal, ref T CalcVal)
    {
        long FileLen;
        int NProcess;
        T CurCalcVal = default(T);
        byte nBuf = 0, nBufNew = 0;
        bool Res = false;
        Task<int> GetDataPart;

        if (CanWork() && (FileLen = GetInputFileSize()) != 0 && IsOverl)
        {
            Res = true;

            GetDataPart = fIn.ReadAsync(States[nBuf].Buf, 0, BufLen);
            FileLen -= BufLen;
            NProcess = WaitForReadEnd(GetDataPart).Result;
            ProcBuf(States[nBuf].Buf, NProcess, ref CalcVal);

            if (FileLen > 0)
            {
                GetDataPart = fIn.ReadAsync(States[nBuf].Buf, 0, BufLen);
                FileLen -= BufLen;
                NProcess = WaitForReadEnd(GetDataPart).Result;
                do
                {
                    nBufNew = (byte)(nBuf ^ 1);
                    if (FileLen <= 0)
                    {
                        ProcBuf(States[nBuf].Buf, NProcess, ref CurCalcVal);
                        ProcVal(CurCalcVal, ref CalcVal);
                        break;
                    }

                    GetDataPart = fIn.ReadAsync(States[nBufNew].Buf, 0, BufLen);
                    FileLen -= BufLen;

                    ProcBuf(States[nBuf].Buf, NProcess, ref CurCalcVal);
                    ProcVal(CurCalcVal, ref CalcVal);
                    nBuf = nBufNew;

                    NProcess = WaitForReadEnd(GetDataPart).Result;
                } while (true);
            }
        }

        FreeRes();
        return Res;
    }
}