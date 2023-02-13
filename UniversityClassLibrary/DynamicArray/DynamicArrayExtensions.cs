using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityClassLibrary.DynamicArray;

public static class DynamicArrayExtensions
{
    public static void Placing<T>(this DynamicArray<T> array, int itemIndex)
        where T : IComparable<T>
    {
        throw new NotImplementedException();
    }

    public static void AddToBegin<T>(this DynamicArray<T> array, T item)
        where T : IComparable<T>
    {
        array.Resize(array.Count + 1);
        for (int i = array.Count; i > 0; i--)
        {
            array[i] = array[i - 1];
        }
        array[0] = item;
    }

    public static void PushSorted<T>(this DynamicArray<T> array, T item)
        where T : IComparable<T>
    {
        throw new NotImplementedException();
    }

    public static void PushSortedBackward<T>(this DynamicArray<T> array, T item)
        where T : IComparable<T>
    {
        throw new NotImplementedException();
    }

    public static void PushSortedForward<T>(this DynamicArray<T> array, T item)
        where T : IComparable<T>
    {
        throw new NotImplementedException();
    }

    public static int SearchBinary<T>(this DynamicArray<T> array, T item, int start, int end)
        where T : IComparable<T>
    {
        throw new NotImplementedException();
    }
}
