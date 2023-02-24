namespace UniversityClassLibrary.HashCodes;

public class HashFNV
{
    public const int FnvOffsetBasis = unchecked((int)2166136261);
    public const int FnvPrime = 16777619;
    
    public static int GetHashForString(string? s)
    {
        if (s is null)
        {
            return 1;
        }
        unchecked
        {
            var hash = FnvOffsetBasis;
            foreach (var symbol in s)
            {
                hash ^= symbol;
                hash *= FnvPrime;
            }

            return hash;
        }
    }

    public static int GetHashForStrings(params string[] strings)
    {
        unchecked
        {
            var hash = FnvOffsetBasis;
            foreach (var s in strings)
            {
                hash ^= GetHashForString(s);
                hash *= FnvPrime;
            }

            return hash;
        }
    }
}

