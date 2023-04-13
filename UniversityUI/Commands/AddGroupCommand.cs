using System;
using System.Windows;
using System.Windows.Input;
using UniversityClassLibrary.NamedArray;
using UniversityClassLibrary.Student;
using UniversityUI.Components;
using UniversityUI.ViewModels;

namespace UniversityUI.Commands;

public class AddGroupCommand : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    private readonly MainWindowViewModel _mainWindow;

    public AddGroupCommand(MainWindowViewModel mainWindow) => _mainWindow = mainWindow;

    public bool CanExecute(object? parameter) => _mainWindow.SelectedFaculty is not null;

    public void Execute(object? parameter)
    {
        var addGroupWindow = new AddFacultyGroupWindow("Add group");
        addGroupWindow.NewNameSet += (_, newName) =>
        {
            var newGroup = new NamedArray<Student>(newName);
            if (addGroupWindow.IsNameWrong = !_mainWindow.AddGroup(newGroup))
            {
                MessageBox.Show(
                    "Group with such name already exists!",
                    "Invalid name",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _mainWindow.SelectedGroup = newGroup.Name;
        };
        addGroupWindow.ShowDialog();
    }

}