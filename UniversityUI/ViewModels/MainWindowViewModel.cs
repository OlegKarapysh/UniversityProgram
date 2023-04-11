using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using UniversityClassLibrary.NamedArray;
using UniversityClassLibrary.Student;
using UniversityUI.Commands;

namespace UniversityUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public static FacultyViewModel DefaultFaculty = new FacultyViewModel(new NamedArray<NamedArray<Student>>());
    
    public ICommand AddFacultyCommand { get; }
    public ICommand ChangeFacultyCommand { get; }
    
    // TODO: use converter or ObservableCollection<string>
    public ObservableCollection<FacultyViewModel> Faculties { get; set; }

    public FacultyViewModel? SelectedFaculty
    {
        get => _selectedFaculty;
        set
        {
            _selectedFaculty = value;
            OnPropertyChanged(nameof(SelectedFaculty));
        }
    }

    private FacultyViewModel? _selectedFaculty;

    public MainWindowViewModel()
    {
        
    }
    public MainWindowViewModel(IEnumerable<NamedArray<NamedArray<Student>>> faculties)
    {
        Faculties = new ObservableCollection<FacultyViewModel>(
            faculties.Select(f => new FacultyViewModel(f)));
        SelectedFaculty = Faculties.FirstOrDefault();
        
        AddFacultyCommand = new AddFacultyCommand(this);
        ChangeFacultyCommand = new ChangeFacultyCommand(this);
    }

    public FacultyViewModel? GetFacultyByName(string name)
    {
        return Faculties.FirstOrDefault(f => f.FacultyName == name);
    }

    public bool AddFaculty(NamedArray<NamedArray<Student>> faculty)
    {
        if (FacultyExists(faculty.Name)) return false;
        
        Faculties.Add(new FacultyViewModel(faculty));
        return true;
    }

    public bool FacultyExists(string name) => GetFacultyByName(name) is not null;
}