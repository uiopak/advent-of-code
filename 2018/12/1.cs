using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day12
{
    public class Program
    {
        public class Rule
        {
            public string match;
            public string replace;
        }

        public static bool checkCurrent(ref string oldPots,
            ref string newPots, Rule rule, ref int poz,
            ref int head, ref int added)
        {
            int localPoz = poz + added;
            if ((poz < 2 ? '.' : oldPots[poz - 2]) == rule.match[0] &&
                (poz < 2 ? '.' : oldPots[poz - 1]) == rule.match[1] &&
                oldPots[poz] == rule.match[2] &&
                (poz > oldPots.Length - 3 ? '.' : oldPots[poz + 1]) == rule.match[3] &&
                (poz > oldPots.Length - 3 ? '.' : oldPots[poz + 2]) == rule.match[4])
            {
                if (poz < 2 && rule.replace == "#")
                {
                    head -= 2;
                    newPots = ".." + newPots;
                    localPoz += 2;
                    added += 2;
                }
                else if (poz > oldPots.Length - 2 && rule.replace == "#")
                {
                    newPots = newPots + "..";
                }
                newPots = newPots.Insert(localPoz, rule.replace).Substring(0, newPots.Length);
                return true;
            }
            return false;
        }
        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllLines("input");
            bool first = true;
            bool second = false;
            var rules = new List<Rule>();
            string potsString = "";

            foreach (var line in input)
            {
                if (first)
                {
                    var tmp = line.Split(' ');
                    first = false;
                    second = true;
                    potsString = tmp[2];
                }
                else if (second)
                {
                    second = false;
                }
                else
                {
                    var tmp = line.Split(' ');
                    rules.Add(new Rule { match = tmp[0], replace = tmp[2] });
                }
            }
            potsString = ".." + potsString;
            potsString = potsString + "..";

            int head = -2;
            long generation = 0;
            while (generation < 20)
            {
                string newPots = "";
                for (int i = 0; i < potsString.Length; i++)
                {
                    newPots += ".";
                }
                int add = 0;
                for (int i = 0; i < potsString.Length; i++)
                {
                    foreach (var rule in rules)
                    {
                        if (checkCurrent(ref potsString, ref newPots, rule, ref i, ref head, ref add))
                        {
                            break;
                        }
                    }
                }
                potsString = newPots;
                generation++;
            }
            int counter = 0;
            for (int i = 0; i < potsString.Length; i++)
            {
                if (potsString[i] == '#')
                {
                    counter += i + head;
                }
            }
            Console.WriteLine(counter);
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}