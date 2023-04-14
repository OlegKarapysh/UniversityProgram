using System;
using System.Windows.Input;
using UniversityUI.ViewModels;

namespace UniversityUI.Commands;

public class DeleteStudentCommand : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    
    private readonly MainWindowViewModel _mainWindow;

    public DeleteStudentCommand(MainWindowViewModel mainWindow) => _mainWindow = mainWindow;

    public bool CanExecute(object? parameter) => _mainWindow.SelectedStudent is not null;

    public void Execute(object? parameter)
    {
        var indexOfRemoved = _mainWindow.RemoveCurrentStudent();
        _mainWindow.SelectedStudent = indexOfRemoved > 0 ? _mainWindow.StudentNames[indexOfRemoved - 1] : 
            _mainWindow.StudentNames.Count > 0 ? _mainWindow.StudentNames[indexOfRemoved] : null;
    }

}