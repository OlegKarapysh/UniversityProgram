namespace UniversityClassLibrary.DynamicArray;

public interface IDynamicArray<T> : IList<T>, IComparable<IDynamicArray<T>>, ICloneable
    where T : IComparable<T>, new()
{
    int ReserveStep { get; set; }
    int DefaultReserveStep { get; }

    void Resize(int newSize);
    void Sort();
    void AddToBegin(T item);
    void AddSorted(T item);
    void Ordering(int index);
    int IndexOfBinary(T item);
}
