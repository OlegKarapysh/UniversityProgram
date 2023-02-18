using UniversityClassLibrary.Student;

namespace University.Tests;

public class StudentTest
{
    private readonly Student _student;
	private string _name;
	private string _surname;
	private string _patronymic;

	public StudentTest()
	{
		_student = new Student();
		_name = "a";
		_surname = "b";
		_patronymic = "c";
	}

	[Fact]
	public void CreatingDefaultStudent_ReturnsCorrectStudent()
	{
		// Assert.
		_student.Name.Should().Be(string.Empty);
        _student.Surname.Should().Be(string.Empty);
        _student.Patronymic.Should().BeNull();
		_student.BirthYear.Should().Be(default);
        _student.AverageMark.Should().Be(default);
        _student.Comparer.Should().Be(new StudentFullNameComparer());
    }

	[Fact]
	public void CreatingNewNamedStudent_ReturnsNamedStudent()
	{
		// Act.
		var namedStudent = new Student(_name, _surname, _patronymic);

		// Assert.
		namedStudent.Name.Should().Be(_name);
		namedStudent.Surname.Should().Be(_surname);
		namedStudent.Patronymic.Should().Be(_patronymic);
		namedStudent.BirthYear.Should().Be(default);
		namedStudent.AverageMark.Should().Be(default);
		namedStudent.Comparer.Should().Be(new StudentFullNameComparer());
	}

	[Fact]
	public void CopyingStudent_ReturnsEqualStudent()
	{
        // Arrange.
		var birthYear = new DateOnly(2000, 1, 1);
		var averageMark = 99;
        _student.AverageMark = averageMark;
		_student.BirthYear = birthYear;
		_student.Surname = _surname;
		_student.Name = _name;
		_student.Patronymic = _patronymic;

		// Act.
		var studentCopy = new Student(_student);

		// Assert.
		studentCopy.Name.Should().Be(_name);
		studentCopy.Surname.Should().Be(_surname);
		studentCopy.Patronymic.Should().Be(_patronymic);
		studentCopy.BirthYear.Should().Be(birthYear);
		studentCopy.AverageMark.Should().Be(averageMark);
		studentCopy.Comparer.Should().Be(new StudentFullNameComparer());
	}

	[Fact]
	public void ComparingEqualStudents_ReturnsTrueOrZero()
	{
        // Arrange.
		var namedStudent = new Student(_name, _surname, _patronymic);
        var namedStudentCopy = new Student(_name, _surname, _patronymic);

		// Act.
		var resultCompareTo = namedStudent.CompareTo(namedStudentCopy);
		var resultOperator = namedStudent == namedStudentCopy;
		var resultEquals = namedStudent.Equals(namedStudentCopy);

		// Assert.
		resultCompareTo.Should().Be(0);
		resultOperator.Should().BeTrue();
		resultEquals.Should().BeTrue();
	}

	[Theory]
	[InlineData("a", "a", "a")]
	[InlineData("", "b", "c")]
    [InlineData("a", "b", "b")]
    [InlineData("a", "b", null)]
    public void ComparingDifferentStudents_ReturnsCorrectComparisonResult(
		string name, string surname, string? patronymic)
	{
		// Arrange.
		var student = new Student(_name, _surname, _patronymic);
		var precedingStudent = new Student(name, surname, patronymic);
		
		// Act.
		var resultPreceding = precedingStudent.CompareTo(student);
		var resultFollowing = student.CompareTo(precedingStudent);
		var resultEquals = student.Equals(precedingStudent);
		var resultOperator = student == precedingStudent;

		// Assert.
		resultPreceding.Should().Be(-1);
		resultFollowing.Should().Be(1);
		resultEquals.Should().BeFalse();
		resultOperator.Should().BeFalse();
	}
}
