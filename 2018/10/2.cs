using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

// NOTE: add reference!!

namespace Day10
{
    public class Program
    {
        public class Point
        {
            public int X;
            public int Y;
            public int Xv;
            public int Yv;
        }

        public static void MovePoint(Point p, int t)
        {
            p.X = p.X + t * p.Xv;
            p.Y = p.Y + t * p.Yv;
        }
        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllLines("input");
            var points = new List<Point>();
            foreach (var line in input)
            {
                var tmp = line.Split('<', ',', '>', '<', ',', '>');
                points.Add(new Point
                {
                    X = int.Parse(tmp[1]),
                    Y = int.Parse(tmp[2]),
                    Xv = int.Parse(tmp[4]),
                    Yv = int.Parse(tmp[5]),
                });
            }

            int change = 1;
            int cnt = 0;
            int? xMin = null;
            int? xMax = null;
            while (true)
            {
                cnt++;
                int? yMin = null;
                int? yMax = null;
                foreach (var p in points)
                {
                    MovePoint(p, change);
                    if (yMax == null || p.Y > yMax)
                    {
                        yMax = p.Y;
                    }
                    if (yMin == null || p.Y < yMin)
                    {
                        yMin = p.Y;
                    }
                }

                if (yMax - yMin <= 10)
                {
                    Console.WriteLine(cnt + ":");
                    break;
                }
            }

            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine("RunTime " + $"{ts.TotalMilliseconds:000.000000} ms");
            Console.ReadLine();
        }
    }
}