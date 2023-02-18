using System.Diagnostics;
using UniversityClassLibrary.DynamicArray;

namespace ConsoleTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var size = 1_000_000;
            //var array = new DynamicArray<RefType>(size);
            //Fill(array, size);
            //var clock = new Stopwatch();

            //Console.WriteLine("Start");
            //clock.Start();
            //var newArray = new DynamicArray<RefType>(array);
            //clock.Stop();

            //Console.WriteLine(newArray.Count);
            //newArray[0].Prop = 5;
            //Console.WriteLine(array[0].Prop);
            //Console.WriteLine(newArray[0].Prop);
            //Console.WriteLine(clock.ElapsedMilliseconds);
            string name = "a";
            string surname = "b";
            string surname2 = "b";
            string patronymic = "d";
            string nullPatronymic = null;
            var stud1 = new {Name = name, Surname = surname, Patronymic = nullPatronymic };
            var stud2 = new { Name = name, Surname = surname2, Patronymic = nullPatronymic };
            Console.WriteLine((stud1.Name, stud1.Surname, stud1?.Patronymic)
                .CompareTo((stud2.Name, stud2.Surname, stud2?.Patronymic)));
            var b = stud2?.Patronymic;
        }

        static void Fill(DynamicArray<RefType> array, int size = 100)
        {
            for (int i = 0; i < size; i++)
            {
                array.Add(new RefType());
            }
        }
    }
}