using System;
using System.Windows.Input;
using UniversityUI.ViewModels;

namespace UniversityUI.Commands;

public class DeleteGroupCommand : ICommand
{
    public event EventHandler? CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }
    
    private readonly MainWindowViewModel _mainWindow;

    public DeleteGroupCommand(MainWindowViewModel mainWindow) => _mainWindow = mainWindow;

    public bool CanExecute(object? parameter) => _mainWindow.SelectedGroup is not null;

    public void Execute(object? parameter)
    {
        var removedIndex = _mainWindow.GroupNames.IndexOf(_mainWindow.SelectedGroup);
        _mainWindow.RemoveCurrentGroup();
        _mainWindow.SelectedGroup = removedIndex > 0 ? _mainWindow.GroupNames[removedIndex - 1] : 
            _mainWindow.GroupNames.Count > 0 ? _mainWindow.GroupNames[removedIndex] : null;
    }

}