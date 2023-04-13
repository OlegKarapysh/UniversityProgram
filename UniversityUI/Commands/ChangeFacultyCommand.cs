using System;
using System.Windows;
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

    public bool CanExecute(object? parameter) => 
        _mainWindow.FacultyNames.Count > 0 && _mainWindow.SelectedFaculty is not null;

    public void Execute(object? parameter)
    {
        var oldName = _mainWindow.SelectedFaculty;
        var changeFacultyWindow = new AddFacultyGroupWindow(
            "Change faculty", oldName);
        changeFacultyWindow.NewNameSet += (_, newName) =>
        {
            if (changeFacultyWindow.IsNameWrong = !_mainWindow.RenameCurrentFaculty(newName))
            {
                MessageBox.Show(
                    "Faculty with such name already exists!",
                    "Invalid name",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _mainWindow.SelectedFaculty = newName;
        };
        changeFacultyWindow.ShowDialog();
    }
    
    //private void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}