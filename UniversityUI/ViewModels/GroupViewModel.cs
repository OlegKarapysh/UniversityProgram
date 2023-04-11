using System.Collections.ObjectModel;
using System.Linq;
using UniversityClassLibrary.NamedArray;
using UniversityClassLibrary.Student;

namespace UniversityUI.ViewModels;

public class GroupViewModel : ViewModelBase
{
    public string GroupName
    {
        get => _groupName ?? string.Empty;
        set
        {
            _groupName = value;
            OnPropertyChanged(nameof(GroupName));
        }
    }
    
    private readonly ObservableCollection<StudentViewModel> _group;
    private string _groupName;

    public GroupViewModel(NamedArray<Student> group)
    {
        _group = new ObservableCollection<StudentViewModel>(group.Select(st => new StudentViewModel(st)));
        _groupName = group.Name;
    }

    public override string ToString() => GroupName;
}