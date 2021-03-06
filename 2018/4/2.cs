
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adventDay4
{
    class Program
    {
        public struct guard
        {
            public int id;
            public int[] minutes;
            public int[] sleep;
            public int maxDay;
            public int maxDaySleep;
            public int mostOftenMinute;
            public int mostOftenMinuteTime;
        }
        static void Main(string[] args)
        {
            var dir = Directory.GetCurrentDirectory();
            Console.WriteLine(dir);

            var lines = File.ReadLines("input");
            var arr = lines.OrderBy(x => x).ToArray();
            System.Console.WriteLine(arr);

            List<guard> guards = new List<guard>();

            int lastG = 0;
            int line = 0;
            int sleepStart = 0;
            int gmaxsleep = 0;
            int maxSleep = 0;
            int mostOftenMinute = 0;
            int mostOftenMinuteTime = 0;
            foreach (var a in arr)
            {
                var split = a.Split(new[] { '[', '-', '-', ' ', ':', ']', ' ', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                if (split[5] == "falls")
                {
                    line = 1;
                }
                if (line == 0)
                {
                    var sp = split[6].Split(new[] { '#', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                    lastG = int.Parse(sp[0]);
                    if (guards.Count == 0 || !guards.Any(x => x.id == lastG))
                    {
                        guards.Add(new guard
                        {
                            id = lastG,
                            minutes = new int[60],
                            sleep = new int[1],
                            maxDay = 0,
                            maxDaySleep = 0,
                            mostOftenMinute = 0,
                            mostOftenMinuteTime = 0
                        });
                    }
                    line++;
                }
                else if (line == 1)
                {
                    sleepStart = int.Parse(split[4]);
                    line++;
                }
                else
                {
                    line = 0;
                    var g = guards.First(x => x.id == lastG);
                    for (int i = sleepStart; i < int.Parse(split[4]); i++)
                    {
                        g.minutes[i]++;
                    }
                    int tmp = guards.First(x => x.id == lastG).sleep[0] + (int.Parse(split[4]) - sleepStart);

                    g.sleep[0] = tmp;
                    if (guards.First(x => x.id == lastG).sleep[0] > maxSleep)
                    {
                        maxSleep = guards.First(x => x.id == lastG).sleep[0];
                        gmaxsleep = g.id;
                    }
                }
            }
            int bestg = 0;
            int bestm = 0;
            int besti = 0;
            foreach (var item in guards)
            {
                int licz = 0;
                foreach (var item2 in item.minutes)
                {
                    if (item2 > bestm)
                    {
                        bestm = item2; ;
                        besti = licz;

                        bestg = item.id;
                    }
                    licz++;
                }
            }
            System.Console.WriteLine(bestg * besti);
        }
    }
}

