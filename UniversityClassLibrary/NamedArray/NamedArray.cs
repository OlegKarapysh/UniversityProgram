using UniversityClassLibrary.DynamicArray;
using UniversityClassLibrary.HashCodes;

namespace UniversityClassLibrary.NamedArray;

public class NamedArray<T> : DynamicArray<T>, IComparable<NamedArray<T>>
    where T : IComparable<T>, new()
{
    public string Name
    {
        get => _name;
        set => _name = value ?? string.Empty;
    }

    private string _name = string.Empty;
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

    public override bool Equals(object? obj) =>
        obj is NamedArray<T> other && CompareTo(other) == 0;
    
    public bool Equals(NamedArray<T> other) => CompareTo(other) == 0;

    public int CompareTo(NamedArray<T>? other) =>
        other is null ? 1 : string.Compare(Name, other.Name, StringComparison.Ordinal);
        

    public override int GetHashCode() => HashFNV.GetHashForString(Name);

    public override string ToString() =>
        $"Named array name: {Name}, count: {Count}, " +
        $"capacity: {Capacity}, reserve step: {ReserveStep}";

    public override object Clone() => new NamedArray<T>(this);
}
