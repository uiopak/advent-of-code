using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace Day25
{
    public class Program
    {
        public static int ManhattanDistance(Tuple<int, int, int, int> star1, Tuple<int, int, int, int> star2)
        {
            return Math.Abs(star1.Item1 - star2.Item1) + Math.Abs(star1.Item2 - star2.Item2) +
                   Math.Abs(star1.Item3 - star2.Item3) + Math.Abs(star1.Item4 - star2.Item4);
        }
        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllLines("input");
            var stars = new List<Tuple<int, int, int, int>>();
            foreach (var s in input)
            {
                var numbers = Regex.Matches(s, @"-?\d+");
                stars.Add(new Tuple<int, int, int, int>(int.Parse(numbers[0].Value), int.Parse(numbers[1].Value), int.Parse(numbers[2].Value), int.Parse(numbers[3].Value)));
            }

            var constellationsList = new List<List<Tuple<int, int, int, int>>>();
            foreach (var star in stars)
            {
                bool added = false;
                foreach (var constellation in constellationsList)
                {
                    var tmp = constellation.Find(c => ManhattanDistance(c, star) <= 3);
                    if (tmp != null)
                    {
                        constellation.Add(star);
                        added = true;
                        break;
                    }
                }
                if (constellationsList.Count == 0 || !added)
                {
                    var tmpList = new List<Tuple<int, int, int, int>>
                    {
                        star
                    };
                    constellationsList.Add(tmpList);
                }
            }

            var wasChange = true;
            while (wasChange)
            {
                wasChange = false;
                var tpmConstellationList = constellationsList.ToList();
                foreach (var constellation1 in tpmConstellationList)
                {
                    if (constellationsList.Contains(constellation1))
                    {
                        foreach (var constellation2 in tpmConstellationList)
                        {
                            if (constellationsList.Contains(constellation2) && !constellation2.Equals(constellation1))
                            {
                                bool added = false;
                                foreach (var star in constellation2)
                                {
                                    var tmp = constellation1.Find(c => ManhattanDistance(c, star) <= 3);
                                    if (tmp != null)
                                    {
                                        added = true;
                                        break;
                                    }
                                }

                                if (added)
                                {
                                    wasChange = true;
                                    foreach (var star in constellation2)
                                    {
                                        constellation1.Add(star);
                                    }
                                    constellationsList.Remove(constellation2);
                                }
                            }
                        }
                    }
                }
                Console.WriteLine(constellationsList.Count);
            }
            Console.WriteLine(constellationsList.Count);
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}