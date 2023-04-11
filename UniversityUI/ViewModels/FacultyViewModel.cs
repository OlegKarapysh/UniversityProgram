using System.Collections.ObjectModel;
using System.Linq;
using UniversityClassLibrary.NamedArray;
using UniversityClassLibrary.Student;

namespace UniversityUI.ViewModels;

public class FacultyViewModel : ViewModelBase
{
    public string FacultyName
    {
        get => _facultyName ?? string.Empty;
        set
        {
            _facultyName = value;
            OnPropertyChanged(nameof(FacultyName));
        }
    }
    
    private readonly ObservableCollection<GroupViewModel> _faculty;
    private string _facultyName;

    public FacultyViewModel(NamedArray<NamedArray<Student>> faculty)
    {
        _faculty = new ObservableCollection<GroupViewModel>(faculty
            .Select(gr => new GroupViewModel(gr)));
        FacultyName = faculty.Name;
    }

    public override string ToString() => FacultyName;
}