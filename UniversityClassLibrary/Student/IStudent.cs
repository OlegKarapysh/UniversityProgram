namespace UniversityClassLibrary.Student;

public interface IStudent : IComparable<Student>, ICloneable
{
    string Name { get; set; }
    string Surname { get; set; }
    string? Patronymic { get; set; }
    ushort BirthYear { get; set; }
    float AverageMark { get; set; }
}
