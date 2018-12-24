using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day23
{
    public class Program
    {
        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllLines("input");
            var list = new List<Tuple<decimal, decimal, decimal, decimal>>();
            for (var i = 0; i < input.Length; i++)
            {
                var numbers1 = Regex.Matches(input[i], @"-?\d+");
                list.Add(new Tuple<decimal, decimal, decimal, decimal>(decimal.Parse(numbers1[0].Value), decimal.Parse(numbers1[1].Value), decimal.Parse(numbers1[2].Value), decimal.Parse(numbers1[3].Value)));
            }

            var tmp = list.OrderBy(c => c.Item4);
            var max = tmp.Last();
            decimal cnt = 0;
            foreach (var tuple in list)
            {
                if (Math.Abs((decimal)(max.Item1 - tuple.Item1)) + Math.Abs((decimal)(max.Item2 - tuple.Item2)) + Math.Abs((decimal)(max.Item3 - tuple.Item3)) <= (decimal)max.Item4)
                {
                    cnt++;
                }
            }
            Console.WriteLine(cnt);
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}