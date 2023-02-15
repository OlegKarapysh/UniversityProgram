namespace UniversityClassLibrary.DynamicArray;

public static class DynamicArrayExtensions
{
    public static void Placing<T>(this DynamicArray<T> array, int itemIndex)
        where T : IComparable<T>, new()
    {
        throw new NotImplementedException();
    }

    public static void AddToBegin<T>(this DynamicArray<T> array, T item)
        where T : IComparable<T>, new()
    {
        array.Resize(array.Count + 1);
        for (int i = array.Count - 1; i > 0; i--)
        {
            array[i] = array[i - 1];
        }
        array[0] = item;
    }

    public static void AddSorted<T>(this DynamicArray<T> array, T item)
        where T : IComparable<T>, new()
    {
        throw new NotImplementedException();
    }

    public static void AddSortedBackward<T>(this DynamicArray<T> array, T item)
        where T : IComparable<T>, new()
    {
        throw new NotImplementedException();
    }

    public static void AddSortedForward<T>(this DynamicArray<T> array, T item)
        where T : IComparable<T>, new()
    {
        throw new NotImplementedException();
    }

    public static int SearchBinary<T>(this DynamicArray<T> array, T item, int start, int end)
        where T : IComparable<T>, new()
    {
        throw new NotImplementedException();
    }
}
