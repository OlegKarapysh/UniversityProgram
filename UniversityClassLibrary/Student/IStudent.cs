using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityClassLibrary.Student;

public interface IStudent : IComparable<Student>, ICloneable
{
    string Name { get; set; }
    string Surname { get; set; }
    string? Patronymic { get; set; }
    DateOnly BirthYear { get; set; }
    float AverageMark { get; set; }
    IComparer<IStudent> Comparer { get; set; }
}
