using System;
using System.Windows.Input;
using UniversityClassLibrary.Student;
using UniversityUI.Components;
using UniversityUI.ViewModels;

namespace UniversityUI.Commands;

public class AddStudentCommand : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    
    private readonly MainWindowViewModel _mainWindow;

    public AddStudentCommand(MainWindowViewModel mainWindow) => _mainWindow = mainWindow;

    public bool CanExecute(object? parameter) => _mainWindow.SelectedGroup is not null;

    public void Execute(object? parameter)
    {
        var addStudentWindow = new AddStudentWindow("Add student", false, new Student());
        addStudentWindow.StudentSubmitted += (_, student) =>
        {
            _mainWindow.AddStudent(student);
            _mainWindow.SelectedStudent = student.ToString();
        };
        addStudentWindow.ShowDialog();
    }
}