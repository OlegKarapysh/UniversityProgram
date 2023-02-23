using UniversityClassLibrary.DynamicArray;

namespace UniversityClassLibrary.NamedArray;

public class NamedArrayComparer<T> : IComparer<IDynamicArray<T>>, ICloneable
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

        if (!left.Comparer.Equals(right.Comparer))
        {
            throw new Exception("Using different comparers!");
        }

        var l = left as NamedArray<T>;
        var r = right as NamedArray<T>;
        if (l is null || r is null)
        {
            throw new InvalidOperationException("Comparing inappropriate types!");
        }

        return String.Compare(l.Name, r.Name, StringComparison.Ordinal);
    }

    public object Clone()
    {
        return new NamedArrayComparer<T>();
    }

    public override bool Equals(object? obj) => obj is NamedArrayComparer<T>;

    public override int GetHashCode() => HashCode.Combine(this);
}
