﻿using UniversityClassLibrary.DynamicArray;

namespace UniversityClassLibrary.NamedArray;

public class NamedArray<T> : DynamicArray<T>
    where T : IComparable<T>, new()
{
    public string Name
    {
        get => _name;
        set => _name = value ?? string.Empty;
    }

    public override IComparer<IDynamicArray<T>> Comparer
    {
        get => _comparer;
        set => _comparer = value ?? new NamedArrayComparer<T>();
    }

    private string _name = string.Empty;
    private IComparer<IDynamicArray<T>> _comparer = new NamedArrayComparer<T>();

    #region Constructors
    public NamedArray() { }
    public NamedArray(string name, int capacity = 0) : base(capacity)
    {
        Name = name;
    }
    public NamedArray(NamedArray<T> namedArray) : base(namedArray)
    {
        if (namedArray is not null)
        {
            _name = namedArray.Name.Substring(0);
            _comparer = (IComparer<IDynamicArray<T>>)((ICloneable)
                namedArray.Comparer).Clone();
        }
    }
    #endregion

    #region OverloadedOperators
    public static bool operator ==(NamedArray<T> left, NamedArray<T> right)
        => left.CompareTo(right) == 0;

    public static bool operator !=(NamedArray<T> left, NamedArray<T> right)
        => !(left == right);

    public static bool operator <(NamedArray<T> left, NamedArray<T> right)
        => left.CompareTo(right) < 0;

    public static bool operator >(NamedArray<T> left, NamedArray<T> right)
        => left.CompareTo(right) > 0;

    public static bool operator <=(NamedArray<T> left, NamedArray<T> right)
        => left.CompareTo(right) <= 0;

    public static bool operator >=(NamedArray<T> left, NamedArray<T> right)
        => left.CompareTo(right) >= 0;
    #endregion

    public override bool Equals(object? obj) 
        => obj is NamedArray<T> other && other.CompareTo(this) == 0;

    public override int GetHashCode() => HashCode.Combine(base.GetHashCode(), Name);

    public object Clone() => new NamedArray<T>(this);
}
