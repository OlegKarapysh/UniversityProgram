namespace UniversityClassLibrary.Student;

public class StudentFullNameComparer : IComparer<IStudent>, ICloneable
{
    public int Compare(IStudent? left, IStudent? right)
    {
        if (ReferenceEquals(left, right))
        {
            return 0;
        }
        if (left is null)
        {
            return -1;
        }
        if (right is null)
        {
            return 1;
        }

        return (left.Surname, left.Name, left?.Patronymic).CompareTo(
            (right.Surname, right.Name, right?.Patronymic));
    }

    public object Clone() => new StudentFullNameComparer();

    public override bool Equals(object? obj) => obj is StudentFullNameComparer;
}
