using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace UniversityClassLibrary.FileWork;

class FileBase
{
    public const int DefBufSize = 4096;
    
    // Описание внешнего метода на основе использования WinAPI функции
    // для определения размера кластера на указанном разделе.
    [DllImport("kernel32.dll", 
        CharSet = CharSet.Unicode, 
        SetLastError = true, 
        EntryPoint = "GetDiskFreeSpaceW")]
    static extern bool GetDiskFreeSpace(string lpRootPathName, 
        out int lpSectorsPerCluster, out int lpBytesPerSector,
        out int lpNumberOfFreeClusters, out int lpTotalNumberOfClusters);

    // метод, вычисляющий размер кластера
    public static bool GetClusterSize(string path, ref int BufSize)
    {
        int sectorsPerCluster;
        int bytesPerSector;
        int freeClusters;
        int totalClusters;
        BufSize = FileBase.DefBufSize;
        if (GetDiskFreeSpace(Path.GetPathRoot(path), 
                out sectorsPerCluster, out bytesPerSector, out freeClusters, out totalClusters))
        {
            BufSize = bytesPerSector * sectorsPerCluster;
            return true;
        }
        return false;
    }

    // Статический метод, который открывает файл в синхронном или асинхронном режимах
    public static bool OpenFile(ref FileStream fs, 
        string fileName, int buffLen, bool isRead = true, bool isOverlapped = false)
    {
        try
        {
        // Это один из конструкторов с максимальным количеством аргументов.
        // Его 1-ый аргумент  имя файла, 2-ой  режим работы с файлом,
        // 3-ий  режим совместного доступа; 4-ый длина буфера чтения;
        // 5-ый дополнительные опции, управляющие работой с файлом
        // (указано FileOptions.SequentialScan, что задает оптимизацию
        // при выполнении последовательных операций, а FileOptions.Asynchronous
        //  асинхронный режим работы с файлом). При попытке открытия файлов
        // возможно возникновения исключение, поэтому конструктор располагается в защищенной секции
            //fs = new FileStream(FN, Mode, FileAccess.Read, FileShare.None, BufLen, FileOptions.SequentialScan);
            var fileMode = isRead ? FileMode.Open : FileMode.Create;
            var fileOptions = FileOptions.SequentialScan | (isOverlapped ? FileOptions.Asynchronous : FileOptions.None);
            var fileSystemRights = (isRead ? FileSystemRights.ReadData : FileSystemRights.WriteData);
            var fileAccess = (isRead ? FileAccess.Read : FileAccess.Write);
            fs = new FileStream(fileName, fileMode, fileAccess, FileShare.None, buffLen, fileOptions);
        }
        
        catch
        {
            return false;
        }
        return true;
    }

    // Метод закрытия файла
    public static void CloseFile(ref FileStream fs)
    {
        if (fs != null)
        {
            fs.Close();
            fs = null;
        }
    }
}