using System;
using System.Windows;
using System.Windows.Input;
using UniversityUI.Components;
using UniversityUI.ViewModels;

namespace UniversityUI.Commands;

public class ChangeStudentCommand : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    
    private readonly MainWindowViewModel _mainWindow;

    public ChangeStudentCommand(MainWindowViewModel mainWindow) => _mainWindow = mainWindow;

    public bool CanExecute(object? parameter) => _mainWindow.SelectedStudent is not null;

    public void Execute(object? parameter)
    {
        var changeStudentWindow = new AddStudentWindow(
            "Change student", true, _mainWindow.CurrentStudent!);
        changeStudentWindow.StudentSubmitted += 
            (_, changedStudent) => _mainWindow.ChangeCurrentStudent(changedStudent);
        changeStudentWindow.ShowDialog();
    }
}