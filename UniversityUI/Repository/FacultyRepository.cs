using System.Collections.Generic;
using System.Linq;
using UniversityClassLibrary.NamedArray;
using UniversityClassLibrary.Student;

namespace UniversityUI.Repository;

using StudentGroup = NamedArray<Student>;
using Faculty = NamedArray<NamedArray<Student>>;

public class FacultyRepository
{
    private readonly List<Faculty> _faculties = new List<Faculty>();

    public void AddFaculty(string facultyName) => _faculties.Add(new Faculty(facultyName));

    public bool RemoveFaculty(string facultyName)
    {
        return _faculties.Remove(_faculties
            .Where(f => f.Name == facultyName)
            .First());
    }

    public bool RenameFaculty(string oldName, string newName)
    {
        return true;
    }
}