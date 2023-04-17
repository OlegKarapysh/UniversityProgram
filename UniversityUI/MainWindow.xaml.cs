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
        var st1 = new Student("Alan", "Turing", "First")
        {
            AverageMark = 55.5f,
            BirthYear = 2000
        };
        var st2 = new Student("Bob", "Turing", "Second")
        {
            AverageMark = 15.5f,
            BirthYear = 2000
        };
        var st3 = new Student("Callen", "Turing", null)
        {
            AverageMark = 91.0f,
            BirthYear = 2000
        };
        var group = new NamedArray<Student>("KS1");
        group.Add(st1);
        group.Add(st2);
        group.Add(st3);
        var group2 = new NamedArray<Student>("KS2");
        group2.Add(st1);
        group2.Add(st3);
        var group3 = new NamedArray<Student>("KI1");
        group3.Add(st2);
        group3.Add(st3);
        var faculty = new NamedArray<NamedArray<Student>>("FcNam");
        faculty.Add(group);
        faculty.Add(group2);
        faculty.Add(group3);
        var faculties = new DynamicArray<NamedArray<NamedArray<Student>>>();
        faculties.Add(faculty);
        //_mainWindowViewModel = new MainWindowViewModel(new DynamicArray<NamedArray<NamedArray<Student>>>());
        _mainWindowViewModel = new MainWindowViewModel(faculties);
        
        InitializeComponent();
        DataContext = _mainWindowViewModel;
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e) => Close();

    private void Help_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        _mainWindowViewModel.HelpCommand.Execute(null);
    }

    private void Mark_OnExecuted(object sender, ExecutedRoutedEventArgs e)
    {
        if (_mainWindowViewModel.AverageMarkBySpecialtyCommand.CanExecute(null))
            _mainWindowViewModel.AverageMarkBySpecialtyCommand.Execute(null);
    }
}

