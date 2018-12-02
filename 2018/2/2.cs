using System;
using System.IO;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("input");

            foreach (var line in lines)
            {
                foreach (var line2 in lines)
                {
                    int differ = 0;
                    int differIndex = 0;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] != line2[i])
                        {
                            differ++;
                            differIndex = i;
                        }
                    }
                    if (differ == 1)
                    {
                        Console.WriteLine(line.Remove(differIndex, 1));
                        //var intersect = line.Intersect(line2);
                        //var lineInter = line.Where(c => intersect.Contains(c));
                        //var lineInter2 = line2.Where(c => intersect.Contains(c));
                        //if (lineInter.Count() > lineInter2.Count())
                        //{
                        //    Console.WriteLine(String.Join("", lineInter2));
                        //}
                        //else
                        //{
                        //    Console.WriteLine(String.Join("", lineInter));
                        //}
                        goto end;

                    }
                }
            }
        end:;
        }
    }
}