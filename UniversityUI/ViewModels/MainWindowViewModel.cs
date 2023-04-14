using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using UniversityClassLibrary.DynamicArray;
using UniversityClassLibrary.NamedArray;
using UniversityClassLibrary.Student;
using UniversityUI.Commands;

namespace UniversityUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand AddFacultyCommand { get; }
    public ICommand DeleteFacultyCommand { get; }
    public ICommand ChangeFacultyCommand { get; }
    public ICommand AddGroupCommand { get; }
    public ICommand DeleteGroupCommand { get; }
    public ICommand ChangeGroupCommand { get; }
    public ICommand AddStudentCommand { get; }
    public ICommand DeleteStudentCommand { get; }

    public bool IsFacultiesEnabled
    {
        get => _isFacultiesEnabled;
        set
        {
            _isFacultiesEnabled = value;
            OnPropertyChanged(nameof(IsFacultiesEnabled));
        }
    }
    public bool IsFilterEnabled
    {
        get => _isFilterEnabled;
        set
        {
            _isFilterEnabled = value;
            OnPropertyChanged(nameof(IsFilterEnabled));
        }
    }
    public bool IsStudentFilterEnabled
    {
        get => _isStudentFilterEnabled;
        set
        {
            _isStudentFilterEnabled = value;
            OnPropertyChanged(nameof(IsStudentFilterEnabled));
        }
    }

    public bool IsStudentInfoEnabled
    {
        get => _isStudentInfoEnabled;
        set
        {
            _isStudentInfoEnabled = value;
            OnPropertyChanged(nameof(IsStudentInfoEnabled));
        }
    }

    public ObservableCollection<string> FacultyNames
    {
        get => _facultyNames;
        set
        {
            _facultyNames = value;
            OnPropertyChanged(nameof(FacultyNames));
        }
    }
    public ObservableCollection<string> GroupNames
    {
        get => _groupNames;
        set
        {
            _groupNames = value;
            OnPropertyChanged(nameof(GroupNames));
        }
    }
    public ObservableCollection<string> StudentNames
    {
        get => _studentNames;
        set
        {
            _studentNames = value;
            OnPropertyChanged(nameof(StudentNames));
        }
    }

    public ObservableCollection<string> SelectedStudentInfo
    {
        get => _selectedStudentInfo;
        set
        {
            _selectedStudentInfo = value;
            OnPropertyChanged(nameof(SelectedStudentInfo));
        }
    }

    public string? SelectedFaculty
    {
        get => _selectedFaculty;
        set
        {
            _selectedFaculty = value;
            RefreshObservableGroups(_currentFaculty = GetFacultyByName(value));
            RefreshFilter();
            OnPropertyChanged(nameof(SelectedFaculty));
        }
    }
    public string? SelectedGroup
    {
        get => _selectedGroup;
        set
        {
            _selectedGroup = value;
            RefreshObservableStudents(_currentGroup = GetGroupByName(value, _currentFaculty));
            RefreshStudentFilter();
            OnPropertyChanged(nameof(SelectedGroup));
        }
    }
    public string? SelectedStudent
    {
        get => _selectedStudent;
        set
        {
            _selectedStudent = value;
            _currentStudent = GetStudentByName(value, _currentGroup);
            RefreshObservableStudentInfo(_currentStudent);
            OnPropertyChanged(nameof(SelectedStudent));
        }
    }

    public string? Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            OnPropertyChanged(nameof(Filter));
            RefreshObservableGroups(_currentFaculty);
            SelectedGroup = GroupNames.FirstOrDefault();
        }
    }
    public string? StudentFilter
    {
        get => _studentFilter;
        set
        {
            _studentFilter = value;
            OnPropertyChanged(nameof(StudentFilter));
            RefreshObservableStudents(_currentGroup);
            SelectedStudent = StudentNames.FirstOrDefault();
        }
    }

    private bool _isFacultiesEnabled;
    private bool _isFilterEnabled;
    private bool _isStudentFilterEnabled;
    private bool _isStudentInfoEnabled;
    private string? _filter;
    private string? _studentFilter;
    private string? _selectedFaculty;
    private string? _selectedGroup;
    private string? _selectedStudent;
    private NamedArray<NamedArray<Student>>? _currentFaculty;
    private NamedArray<Student>? _currentGroup;
    private Student? _currentStudent;
    private ObservableCollection<string> _facultyNames;
    private ObservableCollection<string> _groupNames;
    private ObservableCollection<string> _studentNames;
    private ObservableCollection<string> _selectedStudentInfo;
    private readonly DynamicArray<NamedArray<NamedArray<Student>>> _faculties;

    public MainWindowViewModel() : this(null) { }

    public MainWindowViewModel(DynamicArray<NamedArray<NamedArray<Student>>>? faculties)
    {
        _faculties = faculties ?? new DynamicArray<NamedArray<NamedArray<Student>>>();
        RefreshObservableFaculties();
        RefreshFacultiesEnabled();
        RefreshObservableGroups(_faculties.FirstOrDefault());
        RefreshFilter();
        SelectedFaculty = FacultyNames.FirstOrDefault();
        SelectedGroup = GroupNames.FirstOrDefault();
        AddFacultyCommand = new AddFacultyCommand(this);
        DeleteFacultyCommand = new DeleteFacultyCommand(this);
        ChangeFacultyCommand = new ChangeFacultyCommand(this);
        AddGroupCommand = new AddGroupCommand(this);
        DeleteGroupCommand = new DeleteGroupCommand(this);
        ChangeGroupCommand = new ChangeGroupCommand(this);
        AddStudentCommand = new AddStudentCommand(this);
        DeleteStudentCommand = new DeleteStudentCommand(this);
    }

    public NamedArray<NamedArray<Student>>? GetFacultyByName(string? name) => 
        _faculties.FirstOrDefault(f => f.Name == name);

    public NamedArray<Student>? GetGroupByName(
        string? groupName, NamedArray<NamedArray<Student>>? faculty) => 
            faculty?.FirstOrDefault(g => g.Name == groupName);
    
    public Student? GetStudentByName(string? name, NamedArray<Student>? group) => 
        group?.FirstOrDefault(s => s.ToString() == name);

    public bool AddFaculty(NamedArray<NamedArray<Student>> faculty)
    {
        if (FacultyExists(faculty.Name)) return false;
        
        _faculties.Add(faculty);
        _faculties.Ordering(_faculties.Count - 1);
        RefreshObservableFaculties();
        RefreshFacultiesEnabled();
        return true;
    }

    public bool AddGroup(NamedArray<Student> group)
    {
        if (GroupExists(group.Name, _currentFaculty)) return false;
        
        _currentFaculty!.Add(group);
        _currentFaculty.Ordering(_currentFaculty.Count - 1);
        RefreshFilter();
        RefreshObservableGroups(_currentFaculty);
        return true;
    }

    public void AddStudent(Student student)
    {
        _currentGroup!.AddSorted(student);
        RefreshStudentFilter();
        RefreshObservableStudents(_currentGroup);
    }
    
    public bool RenameCurrentFaculty(string newName)
    {
        if (FacultyExists(newName)) return false;
        
        _currentFaculty!.Name = newName;
        _faculties.Ordering(_faculties.IndexOf(_currentFaculty));
        RefreshObservableFaculties();
        return true;
    }

    public bool RenameCurrentGroup(string newName)
    {
        if (GroupExists(newName, _currentFaculty)) return false;

        _currentGroup!.Name = newName;
        _currentFaculty!.Ordering(_currentFaculty.IndexOf(_currentGroup));
        RefreshObservableGroups(_currentFaculty);
        Filter = string.Empty;
        return true;
    }

    public int RemoveCurrentFaculty()
    {
        if (_currentFaculty is null) return DynamicArray<int>.ItemNotFound;

        var name = _currentFaculty.Name;
        var indexOfRemovedFaculty = _faculties.IndexOf(_currentFaculty);
        _faculties.RemoveAt(indexOfRemovedFaculty);
        FacultyNames.Remove(name);
        RefreshFacultiesEnabled();
        return indexOfRemovedFaculty;
    }

    public int RemoveCurrentGroup()
    {
        if (_currentFaculty is null || _currentGroup is null) return DynamicArray<int>.ItemNotFound;

        var name = _currentGroup.Name;
        var indexOfRemovedGroup = _currentFaculty.IndexOf(_currentGroup);
        _currentFaculty.RemoveAt(indexOfRemovedGroup);
        RefreshFilter();
        GroupNames.Remove(name);
        return indexOfRemovedGroup;
    }

    public int RemoveCurrentStudent()
    {
        if (_currentFaculty is null || _currentGroup is null || _currentStudent is null)
        {
            return DynamicArray<int>.ItemNotFound;
        }
        
        var studentName = _currentStudent.ToString();
        // TODO: use IndexOfBinary method for binary search.
        var indexOfRemovedStudent = _currentGroup.IndexOf(_currentStudent);
        _currentGroup.RemoveAt(indexOfRemovedStudent);
        RefreshStudentFilter();
        StudentNames.Remove(studentName);
        return indexOfRemovedStudent;
    }

    private bool FacultyExists(string name) => GetFacultyByName(name) is not null;

    private bool GroupExists(string group, NamedArray<NamedArray<Student>>? faculty) =>
        GetGroupByName(group, faculty) is not null;

    private bool StudentExists(string name, NamedArray<Student>? group) =>
        GetStudentByName(name, group) is not null;

    private void RefreshObservableFaculties() =>
        FacultyNames = new ObservableCollection<string>(
            _faculties.Select(f => f.Name));
    
    private void RefreshObservableGroups(NamedArray<NamedArray<Student>>? faculty) =>
        GroupNames = new ObservableCollection<string>(
            faculty?
                .Select(g => g.Name)
                .Where(g => g.StartsWith(Filter ?? string.Empty))
            ?? Enumerable.Empty<string>());

    private void RefreshObservableStudents(NamedArray<Student>? group) =>
        StudentNames = new ObservableCollection<string>(
            group?
                .Select(s => s.ToString())
                .Where(s => s.StartsWith(StudentFilter ?? string.Empty))
            ?? Enumerable.Empty<string>());

    private void RefreshObservableStudentInfo(Student? student)
    {
        if (student is null)
        {
            SelectedStudentInfo = new ObservableCollection<string>();
            IsStudentInfoEnabled = false;
            return;
        }
        const byte studentPropertiesCount = 5;
        const byte surnameIndex = 0;
        const byte nameIndex = 1;
        const byte patronymicIndex = 2;
        const byte birthYearIndex = 3;
        const byte averageMarkIndex = 4;
        const byte padding = 13;
        var info = new string[studentPropertiesCount];
        info[surnameIndex] =     "Surname".PadRight(padding) + ": " + student.Surname;
        info[nameIndex] =        "Name".PadRight(padding) + ": " + student.Name;
        info[patronymicIndex] =  "Patronymic".PadRight(padding) + ": " + (student.Patronymic ?? string.Empty);
        info[birthYearIndex] =   "Birth year".PadRight(padding) + ": " + student.BirthYear;
        info[averageMarkIndex] = "Average mark".PadRight(padding) + ": " + student.AverageMark.ToString(CultureInfo.InvariantCulture);

        IsStudentInfoEnabled = true;
        SelectedStudentInfo = new ObservableCollection<string>(info);
    }
    
    private void RefreshFacultiesEnabled() => IsFacultiesEnabled = FacultyNames.Count > 0;
    
    private void RefreshFilter()
    {
        Filter = string.Empty;
        IsFilterEnabled = _currentFaculty?.Count > 0;
    }

    private void RefreshStudentFilter()
    {
        StudentFilter = string.Empty;
        IsStudentFilterEnabled = _currentGroup?.Count > 0;
    }
}