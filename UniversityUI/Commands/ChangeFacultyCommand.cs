using System;
using System.Windows.Input;
using UniversityClassLibrary.NamedArray;
using UniversityClassLibrary.Student;
using UniversityUI.Components;
using UniversityUI.ViewModels;

namespace UniversityUI.Commands;

public class ChangeFacultyCommand : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    private readonly MainWindowViewModel _mainWindow;

    public ChangeFacultyCommand(MainWindowViewModel mainWindow) => _mainWindow = mainWindow;

    public bool CanExecute(object? parameter) => _mainWindow.Faculties.Count > 0;

    public void Execute(object? parameter)
    {
        var oldName = _mainWindow.SelectedFaculty.FacultyName;
        var addFacultyWindow = new AddFacultyGroupWindow(
            "Change faculty", oldName);
        addFacultyWindow.NewNameSet += (_, newName) =>
        {
            var selectedFaculty = _mainWindow.GetFacultyByName(oldName);
            selectedFaculty.FacultyName = newName;
            _mainWindow.SelectedFaculty = MainWindowViewModel.DefaultFaculty;
            _mainWindow.SelectedFaculty = selectedFaculty;
        };
        addFacultyWindow.ShowDialog();
    }
    
    //private void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}