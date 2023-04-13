using System;
using System.Windows;
using System.Windows.Input;
using UniversityUI.Components;
using UniversityUI.ViewModels;

namespace UniversityUI.Commands;

public class ChangeGroupCommand : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    
    private readonly MainWindowViewModel _mainWindow;

    public ChangeGroupCommand(MainWindowViewModel mainWindow) => _mainWindow = mainWindow;

    public bool CanExecute(object? parameter) => _mainWindow.SelectedGroup is not null;

    public void Execute(object? parameter)
    {
        var oldName = _mainWindow.SelectedGroup;
        var changeGroupWindow = new AddFacultyGroupWindow(
            "Change group", oldName);
        changeGroupWindow.NewNameSet += (_, newName) =>
        {
            if (changeGroupWindow.IsNameWrong = !_mainWindow.RenameCurrentGroup(newName))
            {
                MessageBox.Show(
                    "Group with such name already exists!",
                    "Invalid name",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _mainWindow.SelectedGroup = newName;
        };
        changeGroupWindow.ShowDialog();
    }

}