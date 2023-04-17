using System;
using System.Windows;
using System.Windows.Input;

namespace UniversityUI.Commands;

public class HelpCommand : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter) =>
        MessageBox.Show("Program 'University', version 1.3", "About program",
            MessageBoxButton.OK, MessageBoxImage.Information);
}