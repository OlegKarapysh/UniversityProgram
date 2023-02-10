using UniversityClassLibrary;
using UniversityUnitTests.MocksAndFakes;
using FluentAssertions;

namespace UniversityUnitTests;

public class DynamicArrayTest
{
    private readonly DynamicArray<int> _dynamicArrayOfInts;
    private readonly DynamicArray<TestReferenceType>_dynamicArrayOfClasses;
    private int[] _testNumbers;
    private TestReferenceType[] _testReferenceTypes;

    public DynamicArrayTest()
    {
        _dynamicArrayOfInts = new DynamicArray<int>();
        _dynamicArrayOfClasses = new DynamicArray<TestReferenceType>();
        _testNumbers = new int[] {-1, 0, 1};
        _testReferenceTypes = new TestReferenceType[]
        {
            new TestReferenceType { Name = "Alice" },
            new TestReferenceType { Name = "Bob" },
            new TestReferenceType { Name = "Calister" },
            new TestReferenceType { Name = "" },
            new TestReferenceType()
        };
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

    [Fact]
    public void CheckArrayCapacityAfterChangingArray_ReturnsCorrectCapacity()
    {
        Assert.Equal(0, _dynamicArrayOfInts.Capacity);

        FillDynamicArray(_dynamicArrayOfInts, _dynamicArrayOfInts.DefaultReserveStep, default);
        Assert.Equal(_dynamicArrayOfInts.DefaultReserveStep, _dynamicArrayOfInts.Capacity);

        _dynamicArrayOfInts.PushBack(default);
        Assert.Equal(_dynamicArrayOfInts.DefaultReserveStep * 2, _dynamicArrayOfInts.Capacity);

        _dynamicArrayOfInts.RemoveAt(0);
        Assert.Equal(_dynamicArrayOfInts.DefaultReserveStep, _dynamicArrayOfInts.Capacity);

        _dynamicArrayOfInts.RemoveAll();
        Assert.Equal(0, _dynamicArrayOfInts.Capacity);
    }

    [Fact]
    public void TryGettingItemByInvalidIndex_ThrowsIndexOutOfRangeException()
    {
        FillDynamicArray(_dynamicArrayOfInts, _testNumbers.Length, default);

        Assert.Throws<IndexOutOfRangeException>(() => _dynamicArrayOfInts[_testNumbers.Length]);
        Assert.Throws<IndexOutOfRangeException>(() => _dynamicArrayOfInts[int.MaxValue]);
        Assert.Throws<IndexOutOfRangeException>(() => _dynamicArrayOfInts[-1]);
    }

    [Fact]
    public void UsingIndexerWithCorrectIndexes_ReturnsAndSetsCorrectValues()
    {
        _dynamicArrayOfInts.Resize(_testNumbers.Length);

        for (int i = 0; i < _testNumbers.Length; i++)
        {
            var expected = int.MaxValue;

            _dynamicArrayOfInts[i] = expected;

            Assert.Equal(expected, _dynamicArrayOfInts[i]);
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

    [Fact]
    public void CreatingDynamicArrayWithInvalidCapacity_ThrowsCorrectException()
    {
        Assert.Throws<OutOfMemoryException>(() => new DynamicArray<int>(int.MaxValue));
        Assert.Throws<ArgumentOutOfRangeException>(() => new DynamicArray<int>(-1));
    }

    [Fact]
    public void ComparingDynamicArraysViaEquals_ReturnsTrueIfTheyEqual()
    {
        var equalArrayInts = new DynamicArray<int>();
        var notEqualArrayInts = new DynamicArray<int>();
        var equalArrayReferenceTypes = new DynamicArray<TestReferenceType>();
        var notEqualArrayReferenceTypes= new DynamicArray<TestReferenceType>();

        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);
        FillDynamicArrayWithTestNumbers(equalArrayInts);
        FillDynamicArray(notEqualArrayInts, _testNumbers.Length, default);
        FillDynamicArrayWithTestReferenceTypes(_dynamicArrayOfClasses);
        FillDynamicArrayWithTestReferenceTypes(equalArrayReferenceTypes);
        FillDynamicArray(
            notEqualArrayReferenceTypes, _testReferenceTypes.Length, new TestReferenceType());

        Assert.True(_dynamicArrayOfInts.Equals(equalArrayInts));
        Assert.False(_dynamicArrayOfInts.Equals(notEqualArrayInts));
        Assert.True(_dynamicArrayOfClasses.Equals(equalArrayReferenceTypes));
        Assert.False(_dynamicArrayOfClasses.Equals(notEqualArrayReferenceTypes));
    }

    [Theory]
    [InlineData(0, 0)]
    [InlineData(1, 256)]
    [InlineData(256, 256)]
    [InlineData(257, 512)]
    [InlineData(1023, 1024)]
    public void ResizingArray_SetsCorrectCountAndCapacity(
        int expectedCount, int expectedCapacity) 
    {
        _dynamicArrayOfInts.Resize(expectedCount);

        Assert.Equal(expectedCount, _dynamicArrayOfInts.Count);
        Assert.Equal(expectedCapacity, _dynamicArrayOfInts.Capacity);
    }

    [Fact]
    public void LogicalNotOperator_ReturnsTrueIfArrayEmpty()
    {
        Assert.True(!_dynamicArrayOfInts);

        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);
        Assert.False(!_dynamicArrayOfInts);

        _dynamicArrayOfInts.RemoveAll();
        Assert.True(!_dynamicArrayOfInts);
    }

    [Fact]
    public void AddingItemsWithPushBack_AddsItemsToEndAndChangesCount()
    {
        for (int i = 0; i < _testNumbers.Length; i++)
        {
            _dynamicArrayOfInts.PushBack(_testNumbers[i]);

            Assert.Equal(_testNumbers[i], _dynamicArrayOfInts[i]);
            Assert.Equal(i + 1, _dynamicArrayOfInts.Count);
        }

        for (int i = 0; i < _testNumbers.Length; i++)
        {
            Assert.Equal(_testNumbers[i], _dynamicArrayOfInts[i]);
        }
    }

    [Fact]
    public void AddingItemsWithPushFront_AddsItemsToBeginAndChangesCount()
    {
        for (int i = 0; i < _testNumbers.Length; i++)
        {
            _dynamicArrayOfInts.PushFront(_testNumbers[i]);
            Assert.Equal(_testNumbers[0], _dynamicArrayOfInts[i]);
            Assert.Equal(i + 1, _dynamicArrayOfInts.Count);
        }

        _testNumbers = _testNumbers.Reverse().ToArray();
        for (int i = 0; i < _testNumbers.Length; i++)
        {
            Assert.Equal(_testNumbers[i], _dynamicArrayOfInts[i]);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void RemovingAllItems_ClearsDynamicArray(int size)
    {
        FillDynamicArray(_dynamicArrayOfInts, size, default);

        _dynamicArrayOfInts.RemoveAll();

        Assert.Equal(0, _dynamicArrayOfInts.Count);
        Assert.Equal(0, _dynamicArrayOfInts.Capacity);
    }

    [Theory]
    [InlineData(0, 2)]
    [InlineData(-1, 2)]
    [InlineData(1, 2)]
    public void RemoveOneItem_DeletesThatItemAndDecreasesCount(int item, int expectedCount)
    {
        FillDynamicArrayWithTestNumbers(_dynamicArrayOfInts);
        _testNumbers = _testNumbers.Where(x => x != item).ToArray();

        _dynamicArrayOfInts.Remove(item);
        //for (int i = 0; i < _testNumbers.Length; i++)
        //{
        //    Assert.Equal
        //}
        Assert.Equal(expectedCount, _dynamicArrayOfInts.Count);
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
        var result = _dynamicArrayOfInts.Search(item);

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
        var result = _dynamicArrayOfInts.Search(item);

        // Assert.
        result.Should().Be(-1);
    }



    private void FillDynamicArray(DynamicArray<int> dynamicArray, int count, int value)
    {
        for (int i = 0; i < count; i++)
        {
            dynamicArray.PushBack(value);
        }
    }

    private void FillDynamicArray(
        DynamicArray<TestReferenceType> dynamicArray, int count, TestReferenceType value)
    {
        for (int i = 0; i < count; i++)
        {
            dynamicArray.PushBack(value);
        }
    }

    private void FillDynamicArrayWithTestNumbers(DynamicArray<int> dynamicArray)
    {
        for (int i = 0; i < _testNumbers.Length; i++)
        {
            dynamicArray.PushBack(_testNumbers[i]);
        }
    }

    private void FillDynamicArrayWithTestReferenceTypes(
        DynamicArray<TestReferenceType> dynamicArray)
    {
        for (int i = 0; i < _testReferenceTypes.Length; i++)
        {
            dynamicArray.PushBack(_testReferenceTypes[i]);
        }
    }
}