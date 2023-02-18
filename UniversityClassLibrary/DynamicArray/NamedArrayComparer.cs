namespace UniversityClassLibrary.DynamicArray;

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

        if (!(left.Comparer.Equals(right.Comparer)))
        {
            throw new Exception("Using different comparers!");
        }

        var l = left as NamedArray<T>;
        var r = right as NamedArray<T>;

        if (l is null || r is null)
        {
            throw new InvalidOperationException("Comparing unappropriate types!");
        }

        return l.Name.CompareTo(r.Name);
    }

    public object Clone()
    {
        return new NamedArrayComparer<T>();
    }

    public override bool Equals(object? obj)
    {
        return obj is NamedArrayComparer<T>;
    }
}
