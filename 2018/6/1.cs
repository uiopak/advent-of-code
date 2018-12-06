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

        static int Distance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2 - smallestX) + Math.Abs(y1 - y2 - smallestY);
        }
        public class Point
        {
            public int x;
            public int y;
        }
        public class PointDist
        {
            public int point;
            public int dist;
            public int numb;
        }
        public static PointDist[,] pd;
        public static List<Point> points;
        public static void SetDist(Point p, int id, int x, int y)
        {
            int dis = Distance(p.x, p.y, x, y);
            if (pd[x, y].dist > dis)
            {
                pd[x, y].dist = dis;
                pd[x, y].point = id;
                pd[x, y].numb = 1;
            }
            else if (pd[x, y].dist == dis)
            {
                pd[x, y].numb++;
            }
        }


        public static void Main()
        {
            //var dir = Directory.GetCurrentDirectory();
            //Console.WriteLine(dir);
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
            pd = new PointDist[boardSizeX, boardSizeY];
            for (int i = 0; i < boardSizeX; i++)
            {
                for (int j = 0; j < boardSizeY; j++)
                {
                    pd[i, j] = new PointDist() { point = 0, dist = boardSizeX * boardSizeY, numb = 0 };
                }
            }
            for (int i = 0; i < pointTotal; i++)
            {
                for (int j = 0; j < boardSizeX; j++)
                {
                    for (int k = 0; k < boardSizeY; k++)
                    {
                        SetDist(points[i], i, j, k);
                    }
                }
            }
            int maxCnt = 0;
            for (int i = 0; i < pointTotal; i++)
            {
                for (int j = 0; j < boardSizeX; j++)
                {
                    for (int k = 0; k < boardSizeY; k++)
                    {
                        if (j == 0)
                        {
                            if (pd[j, k].numb == 1 && pd[j, k].point == i)
                            {
                                goto next;
                            }
                        }
                        if (j == boardSizeX - 1)
                        {
                            if (pd[j, k].numb == 1 && pd[j, k].point == i)
                            {
                                goto next;
                            }
                        }
                        if (k == 0)
                        {
                            if (pd[j, k].numb == 1 && pd[j, k].point == i)
                            {
                                goto next;
                            }
                        }
                        if (k == boardSizeY - 1)
                        {
                            if (pd[j, k].numb == 1 && pd[j, k].point == i)
                            {
                                goto next;
                            }
                        }
                    }
                }


                int cnt = 0;
                for (int j = 0; j < boardSizeX; j++)
                {
                    for (int k = 0; k < boardSizeY; k++)
                    {
                        if (pd[j, k].numb == 1 && pd[j, k].point == i)
                        {
                            cnt++;
                        }
                    }
                }
                if (maxCnt < cnt) maxCnt = cnt;

                next:;
            }
            Console.WriteLine(maxCnt);

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}
