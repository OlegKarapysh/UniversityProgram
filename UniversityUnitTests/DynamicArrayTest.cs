using UniversityClassLibrary.DynamicArray;

namespace UniversityUnitTests;

public class DynamicArrayTest
{
    private readonly DynamicArray<int> _dynamicArrayOfInts;
    private readonly DynamicArray<string>_dynamicArrayOfStrings;
    private int[] _testNumbers;
    private string[] _testStrings;

    public DynamicArrayTest()
    {
        _dynamicArrayOfInts = new DynamicArray<int>();
        _dynamicArrayOfStrings = new DynamicArray<string>();
        _testNumbers = new int[] {-1, 0, 1};
        _testStrings = new string[] { "Alice", "Bob", "Calister", "" };
    }


    //[Fact]
    //public void CountItemsAfterChangingArray_ReturnsCorrectCount()
    //{
    //    Assert.Equal(0, _dynamicArray.Count);

    //    _dynamicArray.PushBack(1);
    //    Assert.Equal(1, _dynamicArray.Count);

    //    _dynamicArray.PushFront(2);
    //    Assert.Equal(2, _dynamicArray.Count);

    //    _dynamicArray.PushSorted(3);
    //    Assert.Equal(3, _dynamicArray.Count);

    //    _dynamicArray.Remove(3);
    //    Assert.Equal(2, _dynamicArray.Count);

    //    _dynamicArray.RemoveAll();
    //    Assert.Equal(0, _dynamicArray.Count);
    //}

    //[Fact]
    //public void CheckArrayCapacityAfterChangingArray_ReturnsCorrectCapacity()
    //{
    //    Assert.Equal(0, _dynamicArrayOfInts.Capacity);

    //    FillDynamicArray(_dynamicArrayOfInts, _dynamicArrayOfInts.DefaultReserveStep, default);
    //    Assert.Equal(_dynamicArrayOfInts.DefaultReserveStep, _dynamicArrayOfInts.Capacity);

    //    _dynamicArrayOfInts.PushBack(default);
    //    Assert.Equal(_dynamicArrayOfInts.DefaultReserveStep * 2, _dynamicArrayOfInts.Capacity);

    //    _dynamicArrayOfInts.RemoveAt(0);
    //    Assert.Equal(_dynamicArrayOfInts.DefaultReserveStep, _dynamicArrayOfInts.Capacity);

    //    _dynamicArrayOfInts.RemoveAll();
    //    Assert.Equal(0, _dynamicArrayOfInts.Capacity);
    //}

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 1)]
    [InlineData(3, int.MaxValue)]
    [InlineData(3, int.MinValue)]
    public void TryGettingItemByInvalidIndex_ThrowsIndexOutOfRangeException(
        int count, int invalidIndex)
    {
        // Arrange.
        FillDynamicArray(_dynamicArrayOfInts, count, default);
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
    public void ComparingDynamicArraysOfIntsViaEquals_ReturnsTrueIfTheyEqual()
    {
        // Arrange.
        var equalArrayInts = new DynamicArray<int>();
        var notEqualArrayInts = new DynamicArray<int>();
        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);
        FillDynamicArrayWithTestNumbers(equalArrayInts);
        FillDynamicArray(notEqualArrayInts, _testNumbers.Length, default);

        // Act.
        var resultForEqual = _dynamicArrayOfInts.Equals(equalArrayInts);
        var resultForNotEqual = _dynamicArrayOfInts.Equals(notEqualArrayInts);

        // Assert.
        resultForEqual.Should().BeTrue();
        resultForNotEqual.Should().BeFalse();
    }

    [Fact]
    public void ComparingDynamicArraysOfStringsViaEquals_ReturnsTrueIfTheyEqual()
    {
        // Arrange.
        var equalArrayStrings = new DynamicArray<string>();
        var notEqualArrayStrings = new DynamicArray<string>();
        FillDynamicArrayWithTestStrings(_dynamicArrayOfStrings);
        FillDynamicArrayWithTestStrings(equalArrayStrings);
        FillDynamicArray(notEqualArrayStrings, _testStrings.Length, string.Empty);

        // Act.
        var resultForEqual = _dynamicArrayOfStrings.Equals(equalArrayStrings);
        var resultForNotEqual = _dynamicArrayOfStrings.Equals(notEqualArrayStrings);

        // Assert.
        resultForEqual.Should().BeTrue();
        resultForNotEqual.Should().BeFalse();
    }

    [Theory]
    [InlineData(0, 1, 256)]
    [InlineData(1, 1, 256)]
    [InlineData(1, 2, 256)]
    [InlineData(257, 512, 512)]
    public void EnlargingArrayByUsingResize_SetsCorrectCountAndCapacity(
        int initialCount, int newSize, int expectedCapacity) 
    {
        // Arrange.
        FillDynamicArray(_dynamicArrayOfInts, initialCount, default);
        var expected = Enumerable.Repeat(default(int), initialCount).ToArray();

        // Act.
        _dynamicArrayOfInts.Resize(newSize);
        var result = _dynamicArrayOfInts.ToArray();

        // Assert.
        _dynamicArrayOfInts.Count.Should().Be(initialCount);
        _dynamicArrayOfInts.Capacity.Should().Be(expectedCapacity);
        result.Should().Equal(expected);
    }

    [Theory]
    [InlineData(256, 1, 256)]
    [InlineData(1000, 500, 512)]
    [InlineData(1023, 0, 0)]
    public void ReducingArrayByUsingResize_SetsCorrectCountAndCapacity(
        int initialCount, int newSize, int expectedCapacity)
    {
        // Arrange.
        FillDynamicArray(_dynamicArrayOfInts, initialCount, default);
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
        FillDynamicArray(_dynamicArrayOfInts, count, default);

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
        _dynamicArrayOfInts.Remove(item);
        var result = _dynamicArrayOfInts.ToArray();

        // Assert.
        result.Should().Equal(expected);
    }

    [Theory]
    [InlineData(-1, 0)]
    [InlineData(0, 1)]
    [InlineData(1, 2)]
    public void SearchingExistingItems_ReturnsIndexOfThatItem(int item, int expectedIndex)
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
    public void SearchingMissingItems_ReturnsMinusOne(int item)
    {
        // Arrange.
        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);

        // Act.
        var result = _dynamicArrayOfInts.IndexOf(item);

        // Assert.
        result.Should().Be(-1);
    }



    private void FillDynamicArray(DynamicArray<int> dynamicArray, int count, int value)
    {
        for (int i = 0; i < count; i++)
        {
            dynamicArray.Add(value);
        }
    }

    private void FillDynamicArray(
        DynamicArray<string> dynamicArray, int count, string value)
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

    private void FillDynamicArrayWithTestStrings(
        DynamicArray<string> dynamicArray)
    {
        for (int i = 0; i < _testStrings.Length; i++)
        {
            dynamicArray.Add(_testStrings[i]);
        }
    }
}