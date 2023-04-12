using System;
using System.Windows;
using System.Windows.Input;
using UniversityClassLibrary.NamedArray;
using UniversityClassLibrary.Student;
using UniversityUI.Components;
using UniversityUI.ViewModels;

namespace UniversityUI.Commands;

public class AddFacultyCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    private readonly MainWindowViewModel _mainWindow;

    public AddFacultyCommand(MainWindowViewModel mainWindow) => _mainWindow = mainWindow;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
        var addFacultyWindow = new AddFacultyGroupWindow("Add faculty");
        addFacultyWindow.NewNameSet += (_, newName) =>
        {
            if (_mainWindow.FacultyExists(newName))
            {
                MessageBox.Show(
                    "Faculty with such name already exists!",
                    "Invalid name",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var newFaculty = new NamedArray<NamedArray<Student>>(newName);
            _mainWindow.AddFaculty(newFaculty);
            _mainWindow.SelectedFaculty = newFaculty.Name;
        };
        addFacultyWindow.ShowDialog();
    }
}