﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using UniversityClassLibrary.Student;
using UniversityUI.ViewModels;

namespace UniversityUI.Components;

public partial class AddStudentWindow : Window
{
    public event EventHandler<Student>? StudentSubmitted;

    private readonly List<TextBox> _textBoxes;
    private readonly string _name;
    private readonly bool _isNextButtonHidden;
    private readonly StudentInputViewModel _studentViewModel;
    private Student _student;

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
        SurnameTextBox.Text = _student.Surname;
        NameTextBox.Text = _student.Name;
        PatronymicTextBox.Text = _student.Patronymic;
        BirthYearTextBox.Text = _student.BirthYear.ToString();
        AverageMarkTextBox.Text = _student.AverageMark.ToString(CultureInfo.InvariantCulture);
    }

    private void TextBox_OnFocus(object sender, RoutedEventArgs e) => ((TextBox)sender).SelectAll();

    private void OkButton_OnClick(object sender, RoutedEventArgs e)
    {
        foreach (var inputTextBox in _textBoxes)
        {
            if (Validation.GetHasError(inputTextBox))
            {
                ShowInvalidStudentMessageBox();
                return;
            }
        }
        OnStudentSubmitted();
        Close();
    }

    private void OnStudentSubmitted()
    {
        try
        {
            _student = _studentViewModel.GetStudent();
            StudentSubmitted?.Invoke(this, _student);
        }
        catch (ArgumentException e)
        {
            ShowInvalidStudentMessageBox();
        }
    }

    private void ShowInvalidStudentMessageBox() =>
        MessageBox.Show(
            "Student info is not valid! Fix all validation issues first.",
            "Validation failed", MessageBoxButton.OK, MessageBoxImage.Error);
}