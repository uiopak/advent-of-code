using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day18
{
    public class Program
    {
        public const char o = '.';
        public const char t = '|';
        public const char l = '#';
        public static int howMany(char c, int x, int y, int xMax, int yMax, char[][] last)
        {
            int cnt = 0;
            if (x > 0 && last[y][x - 1] == c) cnt++;
            if (y > 0 && x > 0 && last[y - 1][x - 1] == c) cnt++;
            if (y > 0 && last[y - 1][x] == c) cnt++;
            if (y > 0 && x < xMax - 1 && last[y - 1][x + 1] == c) cnt++;
            if (x < xMax - 1 && last[y][x + 1] == c) cnt++;
            if (y < yMax - 1 && x < xMax - 1 && last[y + 1][x + 1] == c) cnt++;
            if (y < yMax - 1 && last[y + 1][x] == c) cnt++;
            if (y < yMax - 1 && x > 0 && last[y + 1][x - 1] == c) cnt++;
            return cnt;
        }
        public static void open(int x, int y, int xMax, int yMax, char[][] last, char[][] now)
        {
            if (howMany(t, x, y, xMax, yMax, last) >= 3)
            {
                now[y][x] = t;
            }
        }
        public static void tree(int x, int y, int xMax, int yMax, char[][] last, char[][] now)
        {
            if (howMany(l, x, y, xMax, yMax, last) >= 3)
            {
                now[y][x] = l;
            }
        }
        public static void lumberyard(int x, int y, int xMax, int yMax, char[][] last, char[][] now)
        {
            if ((howMany(l, x, y, xMax, yMax, last) < 1) || howMany(t, x, y, xMax, yMax, last) < 1)
            {
                now[y][x] = o;
            }
        }


        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Console.WriteLine((1000000000 - 7968) % 28);
            var input = File.ReadAllLines("input");
            //var input = File.ReadAllLines("test.txt");
            char[][] last = new char[input.Length][];
            char[][] now = new char[input.Length][];
            int xMax = 0;
            int yMax = 0;
            foreach (var s in input)
            {
                xMax = 0;
                last[yMax] = new char[s.Length];
                now[yMax] = new char[s.Length];
                foreach (var c in s)
                {
                    last[yMax][xMax] = c;
                    now[yMax][xMax] = c;
                    xMax++;
                }

                yMax++;
            }
            long trees = 0;
            long lumberyards = 0;
            for (int i = 0; i < 1000000000; i++)
            {
                trees = 0;
                lumberyards = 0;
                Console.WriteLine(i);
                for (int y = 0; y < yMax; y++)
                {
                    for (int x = 0; x < xMax; x++)
                    {
                        switch (last[y][x])
                        {
                            case o:
                                open(x, y, xMax, yMax, last, now);
                                break;
                            case t:
                                tree(x, y, xMax, yMax, last, now);
                                break;
                            case l:
                                lumberyard(x, y, xMax, yMax, last, now);
                                break;
                        }
                    }
                }

                for (int y = 0; y < yMax; y++)
                {
                    for (int x = 0; x < xMax; x++)
                    {
                        last[y][x] = now[y][x];
                    }
                }
                for (int y = 0; y < yMax; y++)
                {
                    for (int x = 0; x < xMax; x++)
                    {
                        switch (now[y][x])
                        {
                            case t:
                                trees++;
                                break;
                            case l:
                                lumberyards++;
                                break;
                        }
                    }
                }
                Console.WriteLine(trees * lumberyards);
            }

            trees = 0;
            lumberyards = 0;
            for (int y = 0; y < yMax; y++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    switch (now[y][x])
                    {
                        case t:
                            trees++;
                            break;
                        case l:
                            lumberyards++;
                            break;
                    }
                }
            }
            Console.WriteLine(trees * lumberyards);
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}