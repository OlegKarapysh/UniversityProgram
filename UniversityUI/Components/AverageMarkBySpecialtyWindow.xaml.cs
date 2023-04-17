using System.Windows;
using System.Windows.Controls;
using UniversityUI.ViewModels;

namespace UniversityUI.Components;

public partial class AverageMarkBySpecialtyWindow : Window
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    
    public AverageMarkBySpecialtyWindow(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        InitializeComponent();
        DataContext = _mainWindowViewModel;
    }
    
    private void TextBox_OnFocus(object sender, RoutedEventArgs e) => ((TextBox)sender).SelectAll();
    
    private void ExitButton_OnClick(object sender, RoutedEventArgs e) => Close();

    private void AverageMarkBySpecialtyWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        InputSpecialtyTextBox.Focus();
    }
}