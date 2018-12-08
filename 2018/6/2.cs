using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day6
{
    public class Program
    {
        static int smallestX = 0;
        static int largestX = 0;
        static int smallestY = 0;
        static int largestY = 0;

        public static List<Point> points;
        static int Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2 - smallestX) + Math.Abs(y1 - y2 - smallestY);
        }
        public class Point
        {
            public int x;
            public int y;
        }

        public static void Main()
        {
            int pointTotal = 0;
            int boardSizeX = 0;
            int boardSizeY = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            points = new List<Point>();

            var input = File.ReadAllLines("input");
            pointTotal = input.Count();
            bool start = true;

            foreach (var line in input)
            {
                var test = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int tmpX = int.Parse(test[0]);
                int tmpY = int.Parse(test[1]);
                if (start)
                {
                    smallestX = tmpX;
                    largestX = tmpX;
                    smallestY = tmpY;
                    largestY = tmpY;
                    start = false;
                }
                if (smallestX > tmpX)
                {
                    smallestX = tmpX;
                }
                if (largestX < tmpX)
                {
                    largestX = tmpX;
                }
                if (smallestY > tmpY)
                {
                    smallestY = tmpY;
                }
                if (largestY < tmpY)
                {
                    largestY = tmpY;
                }
                points.Add(new Point()
                {
                    x = int.Parse(test[0]),
                    y = int.Parse(test[1])
                });
            }
            boardSizeX = largestX - smallestX;
            boardSizeY = largestY - smallestY;

            int maxCnt = 0;
            for (int j = 0; j < boardSizeX; j++)
            {
                for (int k = 0; k < boardSizeY; k++)
                {
                    int sum = 0;
                    for (int i = 0; i < pointTotal; i++)
                    {
                        sum += Distance(points[i].x, points[i].y, j, k);
                    }
                    if (sum < 10000)
                    {
                        maxCnt++;
                    }
                }
            }

            Console.WriteLine(maxCnt);

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:00}s {1:000.000000} ms", ts.Seconds, ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}
