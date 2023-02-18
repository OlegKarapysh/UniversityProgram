namespace UniversityClassLibrary.Student;

public class Student : IStudent
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string? Patronymic { get; set; }
    public DateOnly BirthYear { get; set; }
    public double AverageMark
    {
        get => _averageMark;
        set => _averageMark = TrimMark(value);
    }
    public virtual IComparer<IStudent> Comparer
    {
        get => _comparer;
        set => _comparer = value ?? new StudentFullNameComparer();
    }


    private double _averageMark = 0;
    private IComparer<IStudent> _comparer = new StudentFullNameComparer();


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
        Comparer = (IComparer<IStudent>)((ICloneable)other.Comparer).Clone();
    }

    public static bool operator ==(Student left, Student right) => left.CompareTo(right) == 0;
    public static bool operator !=(Student left, Student right) => !(left == right);
    public static bool operator <(Student left, Student right) => left.CompareTo(right) < 0;
    public static bool operator >(Student left, Student right) => left.CompareTo(right) > 0;
    public static bool operator <=(Student left, Student right) => left.CompareTo(right) <= 0;
    public static bool operator >=(Student left, Student right) => left.CompareTo(right) >= 0;

    public int CompareTo(Student? other) => _comparer.Compare(this, other);

    public object Clone() => new Student(this);

    public override bool Equals(object? obj) => obj is Student other &&
        _comparer.Compare(this, other) == 0;

    public bool Equals(Student other) => _comparer.Compare(this, other) == 0;

    public override int GetHashCode() => HashCode.Combine(Name, Surname, Patronymic);


    private double TrimMark(double mark) => mark > 100 ? 100 : mark < 0 ? 0 : mark;
}
