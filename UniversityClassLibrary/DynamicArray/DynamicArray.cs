using System.Collections;
using System.Runtime.CompilerServices;

namespace UniversityClassLibrary.DynamicArray;

public class DynamicArray<T> : IDynamicArray<T>
    where T : IComparable<T>
{
    public const int ItemNotFound = -1;

    #region PropsAndIndexers
    public int Count { get; private set; }
    public int Capacity { get; private set; }
    public bool IsReadOnly => throw new NotImplementedException();
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
        int n when n <= 8 => 256,
        int n when n <= 64 => 64,
        _ => 4
    };
    public IComparer<IDynamicArray<T>> Comparer
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

    #region PrivateFields
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

    public DynamicArray(DynamicArray<T> other) : this()
    {
        if (other is null)
        {
            return;
        }

        _array = other._array
            .Select(item =>
            {
                try
                {
                    return CopyItem(item);
                }
                catch (InvalidCastException)
                {
                    throw new InvalidOperationException("Cannot make a deep copy!");
                }
            })
            .ToArray();
        Count = other.Count;
        Capacity = other.Capacity;
        ReserveStep = other.ReserveStep;
    }
    #endregion

    #region OverloadedOperators
    public static bool operator !(DynamicArray<T> array) => array.Count == 0;

    public static bool operator ==(DynamicArray<T> left, DynamicArray<T> right)
        => left.Equals(right);

    public static bool operator !=(DynamicArray<T> left, DynamicArray<T> right)
        => !left.Equals(right);
    #endregion

    #region InterfacesImplementation
    public void Add(T item)
    {
        Resize(Count + 1);
        _array[Count] = item;
        Count++;
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
        throw new NotImplementedException();
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
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    #endregion

    #region OverriddenMethods
    public override bool Equals(object? obj)
    {
        if (!(obj is DynamicArray<T>))
        {
            return false;
        }

        var other = (DynamicArray<T>)obj;
        if (other.Count != Count || other.Capacity != Capacity
                || other.ReserveStep != ReserveStep)
        {
            return false;
        }

        if (!other && !this)
        {
            return true;
        }

        for (int i = 0; i < Count; i++)
        {
            if (other[i].CompareTo(this[i]) != 0)
            {
                return false;
            }
        }

        return true;
    }

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
            for (int i = newSize; i < Count; i++)
            {
                _array[i] = default;
            }
            Count = newSize;
        }

        Reserve(newSize);
    }

    public void Sort()
    {
        throw new NotImplementedException();
    }
    #endregion

    #region PrivateMethods
    private void Reserve(int newSize)
    {
        if (newSize == 0)
        {
            _array = Array.Empty<T>();
            Capacity = 0;
            return;
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
        }
    }

    private void SortHoare(DynamicArray<T> array, int size) { }

    private int FindIndexToInsertSorted(T item, int left = 0, int right = -1) => default;

    private bool IsIndexValid(int index) => index >= 0 && index < Count;

    private T CopyItem(T item)
    {
        return item is ValueType ? item : (T)((ICloneable)item).Clone();
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