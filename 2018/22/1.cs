using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day22
{
    public class Program
    {
        public static Dictionary<Tuple<int, int>, int> geolDic = new Dictionary<Tuple<int, int>, int>();
        public static Dictionary<Tuple<int, int>, int> erosDic = new Dictionary<Tuple<int, int>, int>();
        public static int geologicIndex(int x, int y, int targetX, int targetY, int depth)
        {
            var key = new Tuple<int, int>(x, y);
            if (geolDic.TryGetValue(key, out int value))
            {
                return value;
            }
            else
            {
                if ((x == 0 && y == 0) || (x == targetX && y == targetY))
                {
                    geolDic.Add(key, 0);
                    return 0;
                }
                else if (y == 0)
                {
                    int tmp = x * 16807;
                    geolDic.Add(key, tmp);
                    return tmp;
                }
                else if (x == 0)
                {
                    int tmp = y * 48271;
                    geolDic.Add(key, tmp);
                    return tmp;
                }
                else
                {
                    int tmp = erosionLevel(x - 1, y, targetX, targetY, depth) * erosionLevel(x, y - 1, targetX, targetY, depth);
                    geolDic.Add(key, tmp);
                    return tmp;
                }
            }

        }
        public static int erosionLevel(int x, int y, int targetX, int targetY, int depth)
        {
            var key = new Tuple<int, int>(x, y);
            if (erosDic.TryGetValue(key, out int value))
            {
                return value;
            }
            else
            {
                int tmp = (geologicIndex(x, y, targetX, targetY, depth) + depth) % 20183;
                erosDic.Add(key, tmp);
                return tmp;
            }
        }

        public static int riskLevel(int x, int y, int targetX, int targetY, int depth)
        {
            return erosionLevel(x, y, targetX, targetY, depth) % 3;
        }

        public static void Main()
        {
            //var dir = Directory.GetCurrentDirectory();
            //Console.WriteLine(dir);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            int depth = 10647;
            int targetX = 7;
            int targetY = 770;
            //int depth = 510;
            //int targetX = 10;
            //int targetY = 10;
            char[,] mapa = new char[7, 770];

            int totalRisk = 0;
            for (int x = 0; x <= targetX; x++)
            {
                for (int y = 0; y <= targetY; y++)
                {
                    totalRisk += riskLevel(x, y, targetX, targetY, depth);
                }
            }
            Console.WriteLine(totalRisk);
            //var input = File.ReadAllLines("test.txt");

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}