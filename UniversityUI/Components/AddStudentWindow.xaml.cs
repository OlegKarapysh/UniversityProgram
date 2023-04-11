using System.Windows;

namespace UniversityUI.Components;

public partial class AddStudentWindow : Window
{
    public AddStudentWindow()
    {
        InitializeComponent();
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}