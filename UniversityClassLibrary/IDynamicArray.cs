using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityClassLibrary;

public interface IDynamicArray<T> : IEnumerable<T>, IComparable<IDynamicArray<T>>, ICloneable
    where T : IComparable<T>, new()
{
    int Count { get; }
    int Capacity { get; }
    int ReserveStep { get; set; }
    int DefaultReserveStep { get; }
    IComparer<IDynamicArray<T>> Comparer { get; set; }
    T this[int index] { get; set; }

    void Resize(int newSize);
    void PushBack(T item);
    void PushFront(T item);
    void Remove(T item);
    void RemoveAt(int index);
    void RemoveAll();
    void Sort();
    void PushSorted(T item);
    void PushSortedForward(T item);
    void PushSortedBackward(T item);
    void Placing(int itemIndex);
    int Search(T item);
    int SearchBinary(T item, int start, int end);
}
