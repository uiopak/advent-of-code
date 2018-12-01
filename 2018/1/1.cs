using System.Collections.Generic;
using System.IO;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            int frequency = 0;
            var lines = File.ReadLines("input");
            foreach (var line in lines)
            {
                frequency += int.Parse(line);
            }
            System.Console.WriteLine(frequency.ToString());
        }
    }
}
