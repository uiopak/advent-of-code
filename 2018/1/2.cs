using System.Collections.Generic;
using System.IO;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            int frequency = 0;
            HashSet<int> seen = new HashSet<int>();

            while (true)
            {
                var lines = File.ReadLines("input");
                foreach (var line in lines)
                {
                    frequency += int.Parse(line);
                    if (seen.Contains(frequency))
                    {
                        goto end;
                    }

                    seen.Add(frequency);
                }
            }
        end:
            System.Console.WriteLine(frequency.ToString());
        }
    }
}
