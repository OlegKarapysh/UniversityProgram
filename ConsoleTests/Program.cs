using System.Diagnostics;
using UniversityClassLibrary.DynamicArray;

namespace ConsoleTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var size = 1_000_000;
            var array = new DynamicArray<RefType>(size);
            Fill(array, size);
            var clock = new Stopwatch();

            Console.WriteLine("Start");
            clock.Start();
            var newArray = new DynamicArray<RefType>(array);
            clock.Stop();

            Console.WriteLine(newArray.Count);
            newArray[0].Prop = 5;
            Console.WriteLine(array[0].Prop);
            Console.WriteLine(newArray[0].Prop);
            Console.WriteLine(clock.ElapsedMilliseconds);
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