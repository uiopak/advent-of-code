using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace Day11
{
    public class Program
    {
        public static int GetPower(int x, int y, int gridSN)
        {
            int rackID = x + 10;
            int powerLevel = rackID * y;
            powerLevel += gridSN;
            powerLevel *= rackID;
            if (powerLevel < 100)
            {
                return 0;
            }
            else
            {
                return (int)Math.Abs(powerLevel / 100 % 10) - 5;
            }
        }
        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllText("input");
            int gridSN = int.Parse(input);
            int[,] array = new int[300, 300];
            for (int x = 0; x < 300; x++)
            {
                for (int y = 0; y < 300; y++)
                {
                    array[x, y] = GetPower(x + 1, y + 1, gridSN) + ((y - 1 < 0) ? 0 : array[x, y - 1]) + ((x - 1 < 0) ? 0 : array[x - 1, y]) - ((x - 1 < 0 || y - 1 < 0) ? 0 : array[x - 1, y - 1]);
                }
            }
            int maxPower = 0;
            int xM = 0;
            int yM = 0;
            int size = 0;

            int s = 3;
            {
                for (int x = 0; x + s < 300; x++)
                {
                    for (int y = 0; y + s < 300; y++)
                    {
                        int total = array[x, y] - array[x + s, y] - array[x, y + s] + array[x + s, y + s];
                        if (total > maxPower)
                        {
                            maxPower = total;
                            xM = x;
                            yM = y;
                            size = s;
                        }
                    }
                }
            }

            Console.WriteLine((xM + 2) + "," + (yM + 2) + " " + maxPower);

            TimeSpan ts = stopWatch.Elapsed;
            Console.WriteLine("RunTime " + $"{ts.TotalMilliseconds:000.000000} ms");
            Console.ReadLine();
        }
    }
}