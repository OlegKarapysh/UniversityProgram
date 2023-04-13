using System;
using System.Windows.Input;
using UniversityUI.ViewModels;

namespace UniversityUI.Commands;

public class DeleteFacultyCommand : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    private readonly MainWindowViewModel _mainWindow;

    public DeleteFacultyCommand(MainWindowViewModel mainWindow) => _mainWindow = mainWindow;

    public bool CanExecute(object? parameter) =>
        _mainWindow.FacultyNames.Count > 0 && _mainWindow.SelectedFaculty is not null;

    public void Execute(object? parameter)
    {
        var indexOfRemoved = _mainWindow.RemoveCurrentFaculty();
        _mainWindow.SelectedFaculty = 
            indexOfRemoved > 0 ? _mainWindow.FacultyNames[indexOfRemoved - 1] : 
            _mainWindow.FacultyNames.Count > 0 ? _mainWindow.FacultyNames[indexOfRemoved] : null;
    }

}