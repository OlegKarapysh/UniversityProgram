using UniversityClassLibrary.DynamicArray;

namespace University.Tests;

public class DynamicArrayTest
{
    private readonly DynamicArray<int> _dynamicArrayOfInts;
    private int[] _testNumbers;

    public DynamicArrayTest()
    {
        _dynamicArrayOfInts = new DynamicArray<int>();
        _testNumbers = new[] { -1, 0, 1 };
    }

    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(3, int.MaxValue)]
    [InlineData(3, int.MinValue)]
    public void TryGettingItemByInvalidIndex_ThrowsIndexOutOfRangeException(
        int count, int invalidIndex)
    {
        // Arrange.
        FillDynamicArray(_dynamicArrayOfInts, count);
        Action act = () => { _ = _dynamicArrayOfInts[invalidIndex]; };

        // Act, Assert.
        act.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void UsingIndexerWithCorrectIndexes_ReturnsAndSetsCorrectValues()
    {
        // Arrange.
        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);
        var expected = int.MaxValue;

        // Act, Assert.
        for (int i = 0; i < _testNumbers.Length; i++)
        {
            _dynamicArrayOfInts[i] = expected;
            _dynamicArrayOfInts[i].Should().Be(expected);
        }
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 256)]
    [InlineData(256, 256)]
    [InlineData(257, 512)]
    public void CreatingDynamicArrayWithSpecifiedCapacity_CreatesCorrectArray(
        int capacity, int expectedCapacity)
    {
        // Arrange, Act. 
        var newArray = new DynamicArray<int>(capacity);

        // Assert.
        newArray.Capacity.Should().Be(expectedCapacity);
    }

    [Theory]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    public void CreatingDynamicArrayWithInvalidCapacity_ThrowsCorrectException(
        int invalidCapacity)
    {
        // Arrange.
        Action act = () => { new DynamicArray<int>(invalidCapacity); };

        // Act, Assert.
        act.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void CreatingCloneOfDynamicArray_ReturnsEquivalentArray()
    {
        // Arrange.
        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);

        // Act.
        var clone = new DynamicArray<int>(_dynamicArrayOfInts);
        var result = clone.Equals(_dynamicArrayOfInts);

        // Assert.
        result.Should().BeTrue();
    }

    [Fact]
    public void CreatingCloneOfNull_ReturnsDefaultDynamicArray()
    {
        // Arrange.
        var expected = new DynamicArray<int>();

        // Act.
        var clone = new DynamicArray<int>(null);
        var result = expected.Equals(clone);

        // Assert.
        result.Should().BeTrue();
    }

    [Fact]
    public void GettingHashCodesOfTwoEqualArrays_ReturnsEqualHashCodes()
    {
        // Arrange.
        var first = new DynamicArray<int>();
        var second = new DynamicArray<int>();
        FillDynamicArrayWithTestNumbers(first);
        FillDynamicArrayWithTestNumbers(second);

        // Act.
        var firstHashCode = first.GetHashCode();
        var secondHashCode = second.GetHashCode();

        // Assert.
        firstHashCode.Should().Be(secondHashCode);
    }

    [Fact]
    public void ComparingDynamicArraysOfIntsViaEquals_ReturnsTrueIfTheyEqual()
    {
        // Arrange.
        var equalArrayInts = new DynamicArray<int>();
        var notEqualArrayInts = new DynamicArray<int>();
        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);
        FillDynamicArrayWithTestNumbers(equalArrayInts);
        FillDynamicArray(notEqualArrayInts, _testNumbers.Length + 1);

        // Act.
        var resultForEqual = _dynamicArrayOfInts.Equals(equalArrayInts);
        var resultForNotEqual = _dynamicArrayOfInts.Equals(notEqualArrayInts);

        // Assert.
        resultForEqual.Should().BeTrue();
        resultForNotEqual.Should().BeFalse();
    }

    //[Fact]
    //public void ComparingDynamicArraysOfStringsViaEquals_ReturnsTrueIfTheyEqual()
    //{
    //    // Arrange.
    //    var equalArrayStrings = new DynamicArray<string>();
    //    var notEqualArrayStrings = new DynamicArray<string>();
    //    FillDynamicArrayWithTestStrings(_dynamicArrayOfStrings);
    //    FillDynamicArrayWithTestStrings(equalArrayStrings);
    //    FillDynamicArray(notEqualArrayStrings, _testStrings.Length, string.Empty);

    //    // Act.
    //    var resultForEqual = _dynamicArrayOfStrings.Equals(equalArrayStrings);
    //    var resultForNotEqual = _dynamicArrayOfStrings.Equals(notEqualArrayStrings);

    //    // Assert.
    //    resultForEqual.Should().BeTrue();
    //    resultForNotEqual.Should().BeFalse();
    //}

    [Theory]
    [InlineData(0, 1, 256)]
    [InlineData(1, 1, 256)]
    [InlineData(1, 2, 256)]
    [InlineData(257, 512, 512)]
    [InlineData(256, 1, 256)]
    [InlineData(1000, 500, 512)]
    [InlineData(1023, 0, 0)]
    public void ResizingDynamicArray_SetsCorrectCountAndCapacity(
        int initialCount, int newSize, int expectedCapacity)
    {
        // Arrange.
        FillDynamicArray(_dynamicArrayOfInts, initialCount);
        var expected = Enumerable.Repeat(default(int), newSize).ToArray();

        // Act.
        _dynamicArrayOfInts.Resize(newSize);
        var result = _dynamicArrayOfInts.ToArray();

        // Assert.
        _dynamicArrayOfInts.Count.Should().Be(newSize);
        _dynamicArrayOfInts.Capacity.Should().Be(expectedCapacity);
        result.Should().Equal(expected);
    }

    [Fact]
    public void LogicalNotOperator_ReturnsTrueIfArrayEmpty()
    {
        // Arrange.
        var emptyArray = new DynamicArray<int>();
        var notEmptyArray = new DynamicArray<int>();
        FillDynamicArrayWithTestNumbers(notEmptyArray);

        // Act.
        var resultForEmptyArray = !emptyArray;
        var resultForNotEmptyArray = !notEmptyArray;

        // Assert.
        resultForEmptyArray.Should().BeTrue();
        resultForNotEmptyArray.Should().BeFalse();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public void AddingItemsToEnd_AddsItemsToEndAndChangesCount(int count)
    {
        // Arrange.
        var expected = Enumerable.Range(0, count).ToArray();

        // Act.
        for (int i = 0; i < count; i++)
        {
            _dynamicArrayOfInts.Add(i);
        }
        var result = _dynamicArrayOfInts.Select(x => x).ToArray();

        // Assert.
        result.Should().Equal(expected);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(10)]
    public void AddingItemsToBegin_AddsItemsToBeginAndChangesCount(int count)
    {
        // Arrange.
        var expected = Enumerable.Range(0, count).ToArray().Reverse();

        // Act.
        for (int i = 0; i < count; i++)
        {
            _dynamicArrayOfInts.AddToBegin(i);
        }
        var result = _dynamicArrayOfInts.ToArray();

        // Assert.
        result.Should().Equal(expected);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void RemovingAllItems_ClearsDynamicArray(int count)
    {
        // Arrange.
        FillDynamicArray(_dynamicArrayOfInts, count);

        // Act.
        _dynamicArrayOfInts.Clear();

        // Assert.
        _dynamicArrayOfInts.Count.Should().Be(0);
        _dynamicArrayOfInts.Capacity.Should().Be(0);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(1)]
    public void RemoveOneItem_DeletesThatItemAndDecreasesCount(int item)
    {
        // Arrange.
        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);
        var expected = _testNumbers.Where(x => x != item).ToArray();

        // Act.
        var isRemovalSuccessful = _dynamicArrayOfInts.Remove(item);
        var result = _dynamicArrayOfInts.ToArray();

        // Assert.
        isRemovalSuccessful.Should().BeTrue();
        result.Should().Equal(expected);
    }

    [Fact]
    public void RemoveMissingItem_ReturnsFalse()
    {
        // Arrange.
        var missingItem = int.MaxValue;

        // Act.
        var result = _dynamicArrayOfInts.Remove(missingItem);

        // Assert.
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void RemoveOneItemByIndex_DeletesThatItemAndDecreasesCount(int index)
    {
        // Arrange.
        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);
        var expected = _testNumbers.Where((_, i) => i != index).ToArray();

        // Act.
        _dynamicArrayOfInts.RemoveAt(index);
        var result = _dynamicArrayOfInts.ToArray();

        // Assert.
        result.Should().Equal(expected);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void SearchingIndexOfExistingItem_ReturnsIndexOfThatItem(int item, int expectedIndex)
    {
        // Arrange.
        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);

        // Act.
        var result = _dynamicArrayOfInts.IndexOf(item);

        // Assert.
        result.Should().Be(expectedIndex);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(int.MaxValue)]
    public void SearchingIndexOfMissingItem_ReturnsItemNotFound(int item)
    {
        // Arrange.
        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);

        // Act.
        var result = _dynamicArrayOfInts.IndexOf(item);
        var resultBinary = _dynamicArrayOfInts.IndexOfBinary(item);

        // Assert.
        result.Should().Be(DynamicArray<int>.ItemNotFound);
        resultBinary.Should().Be(DynamicArray<int>.ItemNotFound);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void SearchingIndexOfItemBinary_ReturnsIndexOfThatItem(int item, int expectedIndex)
    {
        // Arrange.
        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);

        // Act.
        var result = _dynamicArrayOfInts.IndexOfBinary(item);

        // Assert.
        result.Should().Be(expectedIndex);
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, -1)]
    [InlineData(-1, 1)]
    public void ComparingDynamicArraysByCount_IndicatesWhichIsBiggerByCount(
        int countDifference, int expected)
    {
        // Arrange.
        var left = new DynamicArray<int>();
        var right = new DynamicArray<int>();
        var rightCount = _testNumbers.Length + countDifference;
        FillDynamicArrayWithTestNumbers(left);
        FillDynamicArray(right, rightCount);

        // Act.
        var result = left.CompareTo(right);

        // Assert.
        result.Should().Be(expected);
    }

    [Fact]
    public void ComparingDynamicArrayToNull_ReturnsOne()
    {
        // Arrange, Act.
        var result = _dynamicArrayOfInts.CompareTo(null);

        // Assert.
        result.Should().Be(1);
    }

    [Theory]
    [InlineData(new [] { -2, -1, 0, 1, 2 }, new [] { -2, -1, 0, 1, 2 })]
    [InlineData(new [] { 2, 1, 0, -1, -2 }, new [] { -2, -1, 0, 1, 2 })]
    [InlineData(new [] { -2, 0, -1, 2, 1 }, new [] { -2, -1, 0, 1, 2 })]
    [InlineData(new [] { -1, -1, 1, 1, 0 }, new [] { -1, -1, 0, 1, 1 })]
    [InlineData(new [] { 1, -1, 1, -1, 0 }, new [] { -1, -1, 0, 1, 1 })]
    [InlineData(new [] { 1, 1, 1 }, new [] { 1, 1, 1 })]
    public void SortingDynamicArray_SortsCorrectly(int[] values, int[] sortedArray)
    {
        // Arrange.
        FillDynamicArrayWithValues(_dynamicArrayOfInts, values);

        // Act.
        _dynamicArrayOfInts.Sort();
        var result = _dynamicArrayOfInts.ToArray();

        // Assert.
        result.Should().Equal(sortedArray);
    }

    [Theory]
    [InlineData(new [] { -2, -1, 0, 1, 2 }, new [] { -2, -1, 0, 1, 2 })]
    [InlineData(new [] { 2, 1, 0, -1, -2 }, new [] { -2, -1, 0, 1, 2 })]
    [InlineData(new [] { -2, 0, -1, 2, 1 }, new [] { -2, -1, 0, 1, 2 })]
    [InlineData(new [] { -1, -1, 1, 1, 0 }, new [] { -1, -1, 0, 1, 1 })]
    [InlineData(new [] { 1, -1, 1, -1, 0 }, new [] { -1, -1, 0, 1, 1 })]
    [InlineData(new [] { 1, 1, 1 }, new [] { 1, 1, 1 })]
    public void OrderingWholeDynamicArray_SortsItCorrectly(int[] values, int[] sortedArray)
    {
        // Arrange, Act.
        for (int i = 0; i < values.Length; i++)
        {
            _dynamicArrayOfInts.Add(values[i]);
            _dynamicArrayOfInts.Ordering(i);
        }
        var result = _dynamicArrayOfInts.ToArray();

        // Assert.
        result.Should().Equal(sortedArray);
    }

    
    private void FillDynamicArray(DynamicArray<int> dynamicArray, int count, int value = default)
    {
        for (int i = 0; i < count; i++)
        {
            dynamicArray.Add(value);
        }
    }

    private void FillDynamicArrayWithTestNumbers(DynamicArray<int> dynamicArray)
    {
        for (int i = 0; i < _testNumbers.Length; i++)
        {
            dynamicArray.Add(_testNumbers[i]);
        }
    }

    private void FillDynamicArrayWithValues(DynamicArray<int> dynamicArray, int[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            dynamicArray.Add(values[i]);
        }
    }
}