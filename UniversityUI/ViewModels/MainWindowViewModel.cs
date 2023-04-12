using System;
using System.Collections.Generic;
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
    public ICommand ChangeFacultyCommand { get; }

    public ObservableCollection<string> FacultyNames
    {
        get => _facultyNames;
        set
        {
            _facultyNames = value;
            OnPropertyChanged(nameof(FacultyNames));
        }
    }

    public string? SelectedFaculty
    {
        get => _selectedFaculty;
        set
        {
            _selectedFaculty = value;
            OnPropertyChanged(nameof(SelectedFaculty));
        }
    }

    private string? _selectedFaculty;
    private ObservableCollection<string> _facultyNames;
    private DynamicArray<NamedArray<NamedArray<Student>>> _faculties;

    public MainWindowViewModel() : this(null) { }

    public MainWindowViewModel(DynamicArray<NamedArray<NamedArray<Student>>>? faculties)
    {
        _faculties = faculties ?? new DynamicArray<NamedArray<NamedArray<Student>>>();
        RefreshObservableFaculties();
        SelectedFaculty = FacultyNames.FirstOrDefault();
        AddFacultyCommand = new AddFacultyCommand(this);
        ChangeFacultyCommand = new ChangeFacultyCommand(this);
    }

    public NamedArray<NamedArray<Student>>? GetFacultyByName(string name)
    {
        return _faculties.FirstOrDefault(f => f.Name == name);
    }

    public bool AddFaculty(NamedArray<NamedArray<Student>> faculty)
    {
        if (FacultyExists(faculty.Name)) return false;
        
        _faculties.Add(faculty);
        _faculties.Ordering(_faculties.Count - 1);
        RefreshObservableFaculties();
        
        return true;
    }
    
    public bool ChangeFacultyName(string oldName, string newName)
    {
        if (!FacultyExists(oldName)) return false;
        var faculty = GetFacultyByName(oldName);
        faculty.Name = newName;
        _faculties.Ordering(_faculties.IndexOf(faculty));
        RefreshObservableFaculties();

        return true;
    }
    
    public bool FacultyExists(string name) => GetFacultyByName(name) is not null;

    public void RefreshObservableFaculties()
    {
        FacultyNames = new ObservableCollection<string>(
            _faculties.Select(f => f.Name));
    }
}