﻿namespace UniversityClassLibrary.FileWork;

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
