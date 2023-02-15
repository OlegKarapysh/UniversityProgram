using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityClassLibrary.DynamicArray;

public interface IDynamicArray<T> : IList<T>, IComparable<IDynamicArray<T>>, ICloneable
    where T : IComparable<T>, new()
{
    int ReserveStep { get; set; }
    int DefaultReserveStep { get; }
    IComparer<IDynamicArray<T>> Comparer { get; set; }

    void Resize(int newSize);
    void Sort();
    //void PushSorted(T item);
    //void PushSortedForward(T item);
    //void PushSortedBackward(T item);
    //void Placing(int itemIndex);
    //int Search(T item);
    //int SearchBinary(T item, int start, int end);
}
