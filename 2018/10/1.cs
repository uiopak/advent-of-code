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
                    bool setMinX = false;
                    bool setMaxX = false;
                    foreach (Point p1 in points)
                    {
                        if (xMax == null || p1.X > xMax)
                        {
                            setMaxX = true;
                            xMax = p1.X;
                        }
                        if (xMin == null || p1.X < xMin)
                        {
                            setMinX = true;
                            xMin = p1.X;
                        }
                    }

                    if (setMaxX && setMinX)
                    {
                        char[,] text = new char[(int)(yMax - yMin + 1), (int)(xMax - xMin + 1)];
                        foreach (Point p1 in points)
                        {
                            text[(int)(p1.Y - yMin), (int)(p1.X - xMin)] = '#';
                        }
                        //Console.WriteLine(cnt + ":");
                        for (int i = 0; i < yMax - yMin + 1; i++)
                        {
                            for (int j = 0; j < xMax - xMin + 1; j++)
                            {
                                Console.Write(text[i, j] == '#' ? '#' : ' ');
                            }
                            Console.Write('\n');
                        }
                        break;
                    }
                }
            }

            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine("RunTime " + $"{ts.TotalMilliseconds:000.000000} ms");
            Console.ReadLine();
        }
    }
}