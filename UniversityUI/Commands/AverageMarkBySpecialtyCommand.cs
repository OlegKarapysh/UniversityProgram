using System;
using System.Windows;
using System.Windows.Input;
using UniversityUI.Components;
using UniversityUI.ViewModels;

namespace UniversityUI.Commands;

public class AverageMarkBySpecialtyCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    private readonly MainWindowViewModel _mainWindowViewModel;

    public AverageMarkBySpecialtyCommand(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
    }

    public bool CanExecute(object? parameter) => 
        _mainWindowViewModel is { IsFacultiesEnabled: true, IsFilterEnabled: true };

    public void Execute(object? parameter)
    {
        var averageMarkWindow = new AverageMarkBySpecialtyWindow(_mainWindowViewModel);
        averageMarkWindow.ShowDialog();
    }
}