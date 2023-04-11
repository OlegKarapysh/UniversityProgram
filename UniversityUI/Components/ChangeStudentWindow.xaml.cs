using System;
using System.Windows;

namespace UniversityUI.Components;

public partial class ChangeStudentWindow : Window
{
    public ChangeStudentWindow()
    {
        InitializeComponent();
    }
    
    private void CancelButton_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
}