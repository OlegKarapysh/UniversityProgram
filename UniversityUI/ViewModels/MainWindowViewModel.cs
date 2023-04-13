using System;
using System.Collections.ObjectModel;
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
            _currentGroup = GetGroupByName(value, _currentFaculty);
            OnPropertyChanged(nameof(SelectedGroup));
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

    private bool _isFacultiesEnabled;
    private bool _isFilterEnabled;
    private string? _filter;
    private string? _selectedFaculty;
    private string? _selectedGroup;
    private NamedArray<NamedArray<Student>>? _currentFaculty;
    private NamedArray<Student>? _currentGroup;
    private ObservableCollection<string> _facultyNames;
    private ObservableCollection<string> _groupNames;
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
    }

    public NamedArray<NamedArray<Student>>? GetFacultyByName(string? name) => 
        _faculties.FirstOrDefault(f => f.Name == name);

    public NamedArray<Student>? GetGroupByName(
        string? groupName, NamedArray<NamedArray<Student>>? faculty) => 
            faculty?.FirstOrDefault(g => g.Name == groupName);

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

    private void RefreshFilter()
    {
        Filter = string.Empty;
        IsFilterEnabled = _currentFaculty?.Count > 0;
    }

    private bool FacultyExists(string name) => GetFacultyByName(name) is not null;

    private bool GroupExists(string group, NamedArray<NamedArray<Student>>? faculty) =>
        GetGroupByName(group, faculty) is not null;

    private void RefreshObservableFaculties() =>
        FacultyNames = new ObservableCollection<string>(
            _faculties.Select(f => f.Name));

    private void RefreshFacultiesEnabled() => IsFacultiesEnabled = FacultyNames.Count > 0;

    private void RefreshObservableGroups(NamedArray<NamedArray<Student>>? faculty) =>
        GroupNames = new ObservableCollection<string>(
            faculty?
                .Select(g => g.Name)
                .Where(g => g.StartsWith(Filter ?? string.Empty))
            ?? Enumerable.Empty<string>());
}