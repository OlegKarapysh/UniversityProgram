namespace UniversityClassLibrary.DynamicArray;

public class DynamicArraySizeComparer<T> : IComparer<IDynamicArray<T>>, ICloneable
    where T : IComparable<T>, new()
{
    public int Compare(IDynamicArray<T>? left, IDynamicArray<T>? right)
    {
        if (ReferenceEquals(left, right))
        {
            return 0;
        }
        if (left is null)
        {
            return -1;
        }
        if (right is null)
        {
            return 1;
        }

        if (!(left.Comparer.Equals(right.Comparer)))
        {
            throw new Exception("Using different comparers!");
        }

        return left.Count.CompareTo(right.Count);
    }

    public object Clone() => new DynamicArraySizeComparer<T>();

    public override bool Equals(object? obj)
        => obj is DynamicArraySizeComparer<T>;

    public override int GetHashCode() => HashCode.Combine(this);
}
