using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input");

            var tab = new int[1000, 1000];
            int mult = 0;
            List<int[]> list = new List<int[]>();

            foreach (var item in lines)
            {
                var result = new Regex(@"\d+").Matches(item)
                              .Cast<Match>()
                              .Select(m => System.Int32.Parse(m.Value))
                              .ToArray();

                list.Add(result);

                for (int i = result[1]; i < result[1] + result[3]; i++)
                {
                    for (int j = result[2]; j < result[2] + result[4]; j++)
                    {
                        tab[i, j]++;
                    }
                }
            }
            foreach (var result in list)
            {
                bool dontoverlap = true;
                for (int i = result[1]; i < result[1] + result[3]; i++)
                {
                    for (int j = result[2]; j < result[2] + result[4]; j++)
                    {
                        if (tab[i, j] > 1)
                        {
                            dontoverlap = false;
                        }
                    }
                }
                if (dontoverlap)
                {
                    System.Console.WriteLine(result[0]);
                    goto end;
                }
            }
        end:;
            System.Console.ReadLine();
        }
    }
}
