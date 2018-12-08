using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day5
{
    class Program
    {
        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            bool nochangeFound = false;

            List<char> text = new List<char>(File.ReadAllText("input"));
            int lng = text.Count;
            while (!nochangeFound)
            {
                bool change = false;
                for (int i = 0; i < lng; i++)
                {
                    if (i + 1 < lng && text[i] != text[i + 1] && char.ToUpper(text[i]) == char.ToUpper(text[i + 1]))
                    {
                        text.RemoveAt(i + 1);
                        text.RemoveAt(i);
                        if (i > 1) i--;
                        i--;
                        lng -= 2;
                        change = true;
                    }

                }
                if (!change)
                {
                    nochangeFound = true;
                }
            }
            Console.WriteLine(lng);

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:00}s {1:000.000000} ms", ts.Seconds, ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}
