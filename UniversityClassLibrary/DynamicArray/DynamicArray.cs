using System.Collections;
using System.Runtime.CompilerServices;
#pragma warning disable CS8601

namespace UniversityClassLibrary.DynamicArray;

public class DynamicArray<T> : IDynamicArray<T>
    where T : IComparable<T>, new()
{
    public const int ItemNotFound = -1;

    #region PropertiesAndIndexers
    public int Count { get; private set; }
    public int Capacity { get; private set; }
    public bool IsReadOnly => false;
    public int ReserveStep
    {
        get => _reserveStep;
        set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            _reserveStep = value;
        }
    }
    public int DefaultReserveStep => Unsafe.SizeOf<T>() switch
    {
        <= 8 => 256,
        <= 64 => 64,
        _ => 4
    };
    public virtual IComparer<IDynamicArray<T>> Comparer
    {
        get => _comparer;
        set => _comparer = value ?? new DynamicArraySizeComparer<T>();
    }

    public T this[int index]
    {
        get
        {
            ThrowIfIndexOutOfRange(index);
            return _array[index];
        }
        set
        {
            ThrowIfIndexOutOfRange(index);
            _array[index] = value;
        }
    }
    #endregion

    #region Fields
    private T[] _array;
    private int _reserveStep;
    private IComparer<IDynamicArray<T>> _comparer;
    #endregion

    #region Constructors
    public DynamicArray()
    {
        _comparer = new DynamicArraySizeComparer<T>();
        _array = Array.Empty<T>();
        ReserveStep = DefaultReserveStep;
        Resize(0);
    }
    public DynamicArray(int capacity) : this()
    {
        if (capacity > Array.MaxLength)
        {
            throw new OutOfMemoryException("Not enough memory for this number of elements!");
        }
        ThrowIfArgumentNegative(capacity, nameof(capacity));

        Reserve(capacity);
    }

    public DynamicArray(DynamicArray<T>? other) : this()
    {
        if (other is null)
        {
            return;
        }

        ReserveStep = other.ReserveStep;
        if (other.Capacity != 0)
        {
            Reserve(other.Capacity);
            if (other.Count != 0)
            {
                other.CopyTo(_array, 0);
                Count = other.Count;
            }
        }
        Comparer = (IComparer<IDynamicArray<T>>)((ICloneable)other.Comparer).Clone();
    }
    #endregion

    #region OverloadedOperators
    public static bool operator !(DynamicArray<T> array) => array.Count == 0;

    public static bool operator ==(DynamicArray<T> left, DynamicArray<T> right)
        => left.CompareTo(right) == 0;

    public static bool operator !=(DynamicArray<T> left, DynamicArray<T> right)
        => !(left == right);

    public static bool operator <(DynamicArray<T> left, DynamicArray<T> right)
        => left.CompareTo(right) < 0;

    public static bool operator >(DynamicArray<T> left, DynamicArray<T> right)
        => left.CompareTo(right) > 0;

    public static bool operator <=(DynamicArray<T> left, DynamicArray<T> right)
        => left.CompareTo(right) <= 0;

    public static bool operator >=(DynamicArray<T> left, DynamicArray<T> right)
        => left.CompareTo(right) >= 0;
    #endregion

    #region InterfacesImplementation
    public void Add(T item)
    {
        Resize(Count + 1);
        _array[Count - 1] = item;
    }

    public bool Contains(T item) => IndexOf(item) != ItemNotFound;

    public int IndexOf(T item)
    {
        for (int i = 0; i < Count; i++)
        {
            if (_array[i].CompareTo(item) == 0)
            {
                return i;
            }
        }
        return ItemNotFound;
    }

    public void Insert(int index, T item)
    {
        Resize(Count + 1);

        ThrowIfIndexOutOfRange(index);

        for (int i = Count - 1; i > index; i--)
        {
            _array[i] = _array[i - 1];
        }
        _array[index] = item;
    }

    public bool Remove(T item)
    {
        int targetIndex = IndexOf(item);

        if (targetIndex == ItemNotFound)
        {
            return false;
        }

        try
        {
            for (int i = targetIndex; i < Count - 1; i++)
            {
                _array[i] = _array[i + 1];
            }
            Resize(Count - 1);
        }
        catch (Exception)
        {
            return false;
        }

        return true;
    }

    public void RemoveAt(int index)
    {
        ThrowIfIndexOutOfRange(index);

        for (; index < Count - 1; index++)
        {
            _array[index] = _array[index + 1];
        }

        Resize(Count - 1);
    }

    public void Clear() => Resize(0);

    public object Clone() => new DynamicArray<T>(this);

    public void CopyTo(T[] array, int arrayIndex)
    {
        ThrowIfIndexOutOfRange(arrayIndex);

        for (int i = arrayIndex; i < Count; i++)
        {
            array[i] = CopyItem(_array[i]);
        }
    }

    public int CompareTo(IDynamicArray<T>? other) => Comparer.Compare(this, other);

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
        {
            yield return _array[i];
        }
    }
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    #endregion

    #region OverriddenMethods
    public override bool Equals(object? obj) =>
        obj is DynamicArray<T> other && other.CompareTo(this) == 0;

    public bool Equals(DynamicArray<T> other) => other.CompareTo(this) == 0;

    public override int GetHashCode()
    {
        int hash = 0;
        if (_array != null)
        {
            for (int i = 0; i < Count; i++)
            {
                hash ^= _array[i].GetHashCode();
            }
        }

        return HashCode.Combine(Count, Capacity, ReserveStep, hash);
    }
    #endregion

    #region PublicMethods
    public void Resize(int newSize)
    {
        ThrowIfArgumentNegative(newSize, nameof(newSize));

        if (newSize > Array.MaxLength)
        {
            throw new OutOfMemoryException("Not enough memory for this number of elements!");
        }

        if (newSize < Count)
        {
            DecreaseSize(newSize);
        }
        else
        {
            IncreaseSize(newSize);
        }
    }

    public void AddToBegin(T item)
    {
        Resize(Count + 1);
        for (int i = Count - 1; i > 0; i--)
        {
            _array[i] = _array[i - 1];
        }
        _array[0] = item;
    }

    public void AddSorted(T item) => Insert(FindIndexToInsertSorted(item), item);

    public void Sort()
    {
        QuickSort(_array, 0, Count - 1);
    }

    public int IndexOfBinary(T item)
    {
        if (Count == 0)
        {
            return ItemNotFound;
        }

        var left = 0;
        var right = Count - 1;

        while (left <= right)
        {
            var mid = (left + right) / 2;

            if (item.CompareTo(_array[mid]) < 0)
            {
                right = mid - 1;
            }
            else if (item.CompareTo(_array[mid]) > 0)
            {
                left = mid + 1;
            }
            else
            {
                return mid;
            }
        }

        return ItemNotFound;
    }

    public void Ordering(int index)
    {
        var item = _array[index];
        RemoveAt(index);
        AddSorted(item);
    }
    #endregion

    #region PrivateMethods
    private ReserveResult Reserve(int newSize)
    {
        if (newSize == 0)
        {
            _array = Array.Empty<T>();
            Capacity = 0;
            return ReserveResult.ArrayReallocated;
        }

        int newCapacity = (newSize / ReserveStep + (newSize % ReserveStep == 0 ? 0 : 1)) * ReserveStep;
        if (newCapacity != Capacity)
        {
            var newArray = new T[newCapacity];

            for (int i = 0; i < Count; i++)
            {
                newArray[i] = _array[i];
            }

            _array = newArray;
            Capacity = newCapacity;
            return ReserveResult.ArrayReallocated;
        }
        return ReserveResult.ArrayNotReallocated;
    }

    private void QuickSort(T[] array, int left, int right)
    {
        var l = left;
        var r = right;
        var pivot = array[(left + right) / 2];

        while (l <= r)
        {
            while (array[l].CompareTo(pivot) < 0)
            {
                l++;
            }

            while (array[r].CompareTo(pivot) > 0)
            {
                r--;
            }
            if (l <= r)
            {
                (array[l], array[r]) = (array[r], array[l]);
                l++;
                r--;
            }
        }

        if (left < r)
        {
            QuickSort(array, left, r);
        }
        if (l < right)
        {
            QuickSort(array, l, right);
        }
    }

    private int FindIndexToInsertSorted(T item)
    {
        if (Count == 0)
        {
            return 0;
        }

        var left = 0;
        var right = Count - 1;

        while (left <= right)
        {
            var mid = (left + right) / 2;

            if (item.CompareTo(_array[mid]) < 0)
            {
                right = mid - 1;
            }
            else if (item.CompareTo(_array[mid]) > 0)
            {
                left = mid + 1;
            }
            else
            {
                return mid;
            }
        }

        return left;
    }

    private bool IsIndexValid(int index) => index >= 0 && index < Count;

    private T CopyItem(T item) => item is ValueType ? item : (T)((ICloneable)item).Clone();

    private void DecreaseSize(int newSize)
    {
        var oldSize = Count;
        Count = newSize;
        if ((Reserve(newSize) == ReserveResult.ArrayNotReallocated) && newSize < oldSize)
        {
            for (int i = newSize; i < oldSize; i++)
            {
                _array[i] = default;
            }
        }
    }

    private void IncreaseSize(int newSize)
    {
        Reserve(newSize);
        for (int i = Count; i < newSize; i++)
        {
            _array[i] = new T();
        }
        Count = newSize;
    }

    private void ThrowIfIndexOutOfRange(int index)
    {
        if (!IsIndexValid(index))
        {
            throw new IndexOutOfRangeException();
        }
    }

    private void ThrowIfArgumentNegative(int argument, string argName)
    {
        if (argument < 0)
        {
            throw new ArgumentOutOfRangeException($"{argName} cannot be negative!");
        }
    }
    #endregion
}