using UniversityClassLibrary.HashCodes;

namespace UniversityClassLibrary.Student;

public class Student : IStudent
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public DateOnly BirthYear { get; set; }
    public float AverageMark
    {
        get => _averageMark;
        set => _averageMark = TrimMark(value);
    }
    
    private float _averageMark = 0;


    public Student() { }

    public Student(string name, string surname, string patronymic)
    {
        Name = name;
        Surname = surname;
        Patronymic = patronymic;
    }

    public Student(Student other)
    {
        Name = other.Name.Substring(0);
        Surname = other.Surname.Substring(0);
        Patronymic = other.Patronymic?.Substring(0);
        BirthYear = other.BirthYear;
        AverageMark = other.AverageMark;
    }

    public static bool operator ==(Student left, Student right) => left.CompareTo(right) == 0;
    public static bool operator !=(Student left, Student right) => !(left == right);
    public static bool operator <(Student left, Student right) => left.CompareTo(right) < 0;
    public static bool operator >(Student left, Student right) => left.CompareTo(right) > 0;
    public static bool operator <=(Student left, Student right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Student left, Student right) => left.CompareTo(right) >= 0;

    public int CompareTo(Student? other)
    {
        return other is null ? 1 : (Surname, Name, Patronymic).CompareTo(
            (other.Surname, other.Name, other.Patronymic));
    }

    public virtual object Clone() => new Student(this);

    public override bool Equals(object? obj) => obj is Student other &&
        CompareTo(other) == 0;

    public bool Equals(Student other) => CompareTo(other) == 0;

    public override int GetHashCode() => HashFNV.GetHashForStrings(Surname, Name, Patronymic);

    public override string ToString() => 
        $"Student surname: {Surname}, name: {Name}" + 
        (Patronymic is null ? "" : $", patronymic: {Patronymic}");

    
    private float TrimMark(float mark) => mark > 100 ? 100 : mark < 0 ? 0 : mark;
}
