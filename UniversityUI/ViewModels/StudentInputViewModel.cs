using System.Globalization;
using UniversityClassLibrary.Student;

namespace UniversityUI.ViewModels;

public class StudentInputViewModel : ViewModelBase
{
    public string Surname
    {
        get => _surname;
        set
        {
            _surname = value;
            OnPropertyChanged(nameof(Surname));
        }
    }
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged(nameof(Name));
        }
    }
    public string Patronymic
    {
        get => _patronymic;
        set
        {
            _patronymic = value;
            OnPropertyChanged(nameof(Patronymic));
        }
    }
    public string BirthYear
    {
        get => _birthYear;
        set
        {
            _birthYear = value;
            OnPropertyChanged(nameof(BirthYear));
        }
    }
    public string AverageMark
    {
        get => _averageMark;
        set
        {
            _averageMark = value;
            OnPropertyChanged(nameof(AverageMark));
        }
    }

    private string _surname;
    private string _name;
    private string _patronymic;
    private string _birthYear;
    private string _averageMark;

    public StudentInputViewModel() : this(new Student()) { }

    public StudentInputViewModel(Student? student)
    {
        var stud = student ?? new Student();
        Surname = stud.Surname;
        Name = stud.Name;
        Patronymic = stud.Patronymic ?? string.Empty;
        BirthYear = stud.BirthYear.ToString();
        AverageMark = stud.AverageMark.ToString(CultureInfo.InvariantCulture);
    }
}