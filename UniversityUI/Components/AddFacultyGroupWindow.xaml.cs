using System;
using System.Windows;

namespace UniversityUI.Components;

public partial class AddFacultyGroupWindow : Window
{
    public event EventHandler<string>? NewNameSet;
    public string NewName { get; set; }
    public bool IsNameWrong { get; set; }
    
    private readonly string _oldName;

    public AddFacultyGroupWindow(string title, string oldName = "")
    {
        InitializeComponent();
        DataContext = this;
        Title = title;
        _oldName = oldName;
    }

    private void OnNewNameSet() => NewNameSet?.Invoke(this, NewName);
    
    private void AddFacultyGroupWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        InputNameTextBox.Text = _oldName;
        InputNameTextBox.Focus();
        InputNameTextBox.SelectAll();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e) => Close();

    private void OkButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(NewName))
        {
            MessageBox.Show(
                "Name cannot be empty!", "Invalid name", 
                MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        OnNewNameSet();
        if (!IsNameWrong)
        {
            Close();
        }
    }
}