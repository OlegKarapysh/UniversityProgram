using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniversityClassLibrary.DynamicArray;
using UniversityClassLibrary.NamedArray;
using UniversityClassLibrary.Student;
using UniversityUI.Components;
using UniversityUI.ViewModels;

namespace UniversityUI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly MainWindowViewModel _mainWindowViewModel;
    
    public MainWindow()
    {
        var st = new Student("nam", "surnam", "patro")
        {
            AverageMark = 4,
            BirthYear = 2000
        };
        var group = new NamedArray<Student>("GrNam");
        group.Add(st);
        var faculty = new NamedArray<NamedArray<Student>>("FcNam");
        faculty.Add(group);
        var faculties = new DynamicArray<NamedArray<NamedArray<Student>>>();
        faculties.Add(faculty);
        //_mainWindowViewModel = new MainWindowViewModel(new List<NamedArray<NamedArray<Student>>>());
        _mainWindowViewModel = new MainWindowViewModel(faculties);
        
        InitializeComponent();
        DataContext = _mainWindowViewModel;
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void AddStudentButton_OnClick(object sender, RoutedEventArgs e)
    {
        var addStudentWindow = new AddStudentWindow();
        addStudentWindow.ShowDialog();
    }

    private void ChangeStudentButton_OnClick(object sender, RoutedEventArgs e)
    {
        var changeStudentWindow = new ChangeStudentWindow();
        changeStudentWindow.ShowDialog();
    }

    private void AddGroupButton_OnClick(object sender, RoutedEventArgs e)
    {
        var addGroupWindow = new AddFacultyGroupWindow("Add group");
        addGroupWindow.ShowDialog();
    }

    private void ChangeGroupButton_OnClick(object sender, RoutedEventArgs e)
    {
        var changeGroupWindow = new AddFacultyGroupWindow("Change group", "old group name");
        changeGroupWindow.ShowDialog();
    }
}

