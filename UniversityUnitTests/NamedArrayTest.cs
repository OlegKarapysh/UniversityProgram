using UniversityClassLibrary.DynamicArray;
using UniversityClassLibrary.NamedArray;

namespace University.Tests;

public class NamedArrayTest
{
    private readonly NamedArray<int> _namedArray;

	public NamedArrayTest()
	{
		_namedArray = new NamedArray<int>();
	}

	[Fact]
	public void CreatingNewNamedArrays_CreatesCorrectNamedArrays()
	{
		// Arrange.
		var expectedName = "name";
		var expectedCapacity = _namedArray.ReserveStep;

		// Act.
		var defaultArray = new NamedArray<int>();
		var namedArray = new NamedArray<int>(expectedName);
		var namedArrayWithCapacity = new NamedArray<int>(expectedName, expectedCapacity);
		var namedArrayCopy = new NamedArray<int>(namedArrayWithCapacity);
		var isCorrectComparer = defaultArray.Comparer.Equals(new NamedArrayComparer<int>());

		// Assert.
		defaultArray.Name.Should().Be(string.Empty);
		namedArray.Name.Should().Be(expectedName);
        namedArrayWithCapacity.Name.Should().Be(expectedName);
        namedArrayWithCapacity.Capacity.Should().Be(expectedCapacity);
        namedArrayCopy.Name.Should().Be(expectedName);
        namedArrayCopy.Capacity.Should().Be(expectedCapacity);
		isCorrectComparer.Should().BeTrue();
    }

	[Theory]
	[InlineData("a")]
    [InlineData("")]
    public void ComparingTwoEqualNamedArrays_ReturnsZeroOrTrue(string name)
	{
		// Arrange.
		_namedArray.Name = name;
		var equalNamedArray = new NamedArray<int>(name);

		// Act.
		var result = _namedArray.CompareTo(equalNamedArray);
		var resultEquals = _namedArray.Equals(equalNamedArray);
		var resultOperator = _namedArray == equalNamedArray;

        // Assert.
        result.Should().Be(0);
        resultEquals.Should().BeTrue();
		resultOperator.Should().BeTrue();
	}

    [Theory]
    [InlineData("a", "b", -1)]
    [InlineData("b", "a", 1)]
	[InlineData(null, "a", -1)]
    public void ComparingTwoDifferentNamedArrays_ReturnsCorrectResult(
		string name1, string name2, int expected)
    {
        // Arrange.
        _namedArray.Name = name1;
        var anotherNamedArray = new NamedArray<int>(name2);

        // Act.
        var result = _namedArray.CompareTo(anotherNamedArray);
        result = result < 0 ? -1 : result > 0 ? 1 : 0;
        var resultEquals = _namedArray.Equals(anotherNamedArray);
        var resultOperator = _namedArray == anotherNamedArray;

        // Assert.
        result.Should().Be(expected);
        resultEquals.Should().BeFalse();
        resultOperator.Should().BeFalse();
    }
}
