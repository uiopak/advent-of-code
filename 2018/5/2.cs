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
            //var dir = Directory.GetCurrentDirectory();
            //Console.WriteLine(dir);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            bool nochangeFound = false;

            //List<char> textO = new List<char>(File.ReadAllText("input"));
            string textO = File.ReadAllText("input");
            string Units = new string(textO.Distinct().ToArray());
            Units = new string(Units.ToUpper().Distinct().ToArray());
            bool first = true;
            int smallestPoly = 0;
            foreach (var unit in Units)
            {
                //List<char> text = new List<char>(textO);
                var textWithoutChar = textO.Replace(unit.ToString(), string.Empty);
                textWithoutChar = textWithoutChar.Replace(char.ToLower(unit).ToString(), string.Empty);
                List<char> text = new List<char>(textWithoutChar);

                for (int i = 0; i < text.Count; i++)
                {
                    if (text[i] == char.ToLower(unit) || text[i] == unit)
                    {
                        text.RemoveAt(i);
                        i--;
                    }
                }
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
                if (first)
                {
                    smallestPoly = lng;
                    first = false;
                }
                else if (lng < smallestPoly)
                {
                    smallestPoly = lng;
                }
            }
            Console.WriteLine(smallestPoly);
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:00}s {1:000.000000} ms", ts.Seconds, ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}
