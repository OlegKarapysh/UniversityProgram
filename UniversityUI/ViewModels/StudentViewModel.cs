using System.Globalization;
using UniversityClassLibrary.Student;

namespace UniversityUI.ViewModels;

public class StudentViewModel : ViewModelBase
{
    public string Surname => _student.Surname;
    public string Name => _student.Name;
    public string Patronymic => _student.Patronymic ?? string.Empty;
    public string BirthYear => _student.BirthYear.ToString(CultureInfo.InvariantCulture);
    public string AverageMark => _student.AverageMark.ToString(CultureInfo.InvariantCulture);
    
    private readonly Student _student;

    public StudentViewModel(Student student) => _student = student;
}