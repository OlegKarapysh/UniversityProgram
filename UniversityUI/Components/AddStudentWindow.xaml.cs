using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using UniversityClassLibrary.Student;
using UniversityUI.ViewModels;

namespace UniversityUI.Components;

public partial class AddStudentWindow : Window
{
    private const ushort DefaultBirthYear = 2000;
    private readonly List<TextBox> _textBoxes;
    private readonly string _name;
    private readonly bool _isNextButtonHidden;
    private readonly Student _student;
    private readonly StudentInputViewModel _studentViewModel;

    public AddStudentWindow(string name, bool isNextButtonHidden, Student student)
    {
        _name = name;
        _isNextButtonHidden = isNextButtonHidden;
        _student = student;
        _studentViewModel = new StudentInputViewModel(student);
        
        InitializeComponent();
        DataContext = _studentViewModel;
        _textBoxes = new List<TextBox>
        {
            NameTextBox, SurnameTextBox, PatronymicTextBox, AverageMarkTextBox, BirthYearTextBox
        };
    }

    private void CancelButton_OnClick(object sender, RoutedEventArgs e) => Close();

    private void AddStudentWindow_OnLoaded(object sender, RoutedEventArgs e)
    {
        Title = _name;
        SurnameTextBox.Focus();
        NextButton.Visibility = _isNextButtonHidden ? Visibility.Collapsed : Visibility.Visible;
        BirthYearTextBox.Text = _student.BirthYear == default(ushort) ? 
            DefaultBirthYear.ToString() : _student.BirthYear.ToString();
    }

    private void TextBox_OnFocus(object sender, RoutedEventArgs e) => ((TextBox)sender).SelectAll();

    private void OkButton_OnClick(object sender, RoutedEventArgs e)
    {
        foreach (var inputTextBox in _textBoxes)
        {
            if (Validation.GetHasError(inputTextBox))
            {
                MessageBox.Show(
                    "Student info is not valid! Fix all validation issues first.",
                    "Validation failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}