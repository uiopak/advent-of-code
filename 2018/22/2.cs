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
        public static Dictionary<Tuple<int, int>, int> riskDic = new Dictionary<Tuple<int, int>, int>();
        public static Dictionary<Tuple<int, int>, Tuple<int, int>> timeDic = new Dictionary<Tuple<int, int>, Tuple<int, int>>();
        //rock    item1 Torch  item2 Gear
        //wet     item1 None   item2 Gear
        //narrow  item1 Torch  item2 None
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
            var key = new Tuple<int, int>(x, y);
            if (riskDic.TryGetValue(key, out int value))
            {
                return value;
            }
            else
            {
                int tmp = erosionLevel(x, y, targetX, targetY, depth) % 3;
                riskDic.Add(key, tmp);
                return tmp;
            }
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
            for (int x = 0; x <= targetX + 40; x++)
            {
                for (int y = 0; y <= targetY + 20; y++)
                {
                    totalRisk += riskLevel(x, y, targetX, targetY, depth);
                }
            }

            int time = 0;
            timeDic.Add(new Tuple<int, int>(0, 0), new Tuple<int, int>(0, 7));
            Tuple<int, int> endtmp;
            List<int> lista = new List<int>();
            List<int> listaCnt = new List<int>();
            int tU = riskLevel(targetX, targetY - 1, targetX, targetY, depth);
            int tD = riskLevel(targetX, targetY + 1, targetX, targetY, depth);
            int tL = riskLevel(targetX - 1, targetY, targetX, targetY, depth);
            int tR = riskLevel(targetX + 1, targetY, targetX, targetY, depth);
            int min = 0;
            //for (int h = 0; h < 140; h++)
            while (true)
            {
                int cnt = 0;
                for (int x = 0; x <= targetX + 40; x++)
                {
                    for (int y = 0; y <= targetY + 20; y++)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            var key = new Tuple<int, int>(x, y);
                            var currentTime = 0;
                            if (timeDic.TryGetValue(key, out var currentValue))
                            {
                                currentTime = i == 0 ? currentValue.Item1 : currentValue.Item2;
                                //for (int j = 0; j < 2; j++)
                                {
                                    if (i == 0)
                                    {
                                        switch (riskLevel(x, y, targetX, targetY, depth))
                                        {
                                            case 0://stone Torch
                                                if (x > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x - 1, y);
                                                    switch (riskLevel(x - 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    cnt++;
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value1.Item2);
                                                                    cnt++;
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value1.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (y > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y - 1);
                                                    switch (riskLevel(x, y - 1, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    cnt++;
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value0.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value1.Item2);
                                                                    cnt++;
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value1.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (x + 1 <= targetX + 40)
                                                {
                                                    var key0 = new Tuple<int, int>(x + 1, y);
                                                    switch (riskLevel(x + 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    cnt++;
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value0.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value1.Item2);
                                                                    cnt++;
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value1.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (y + 1 <= targetY + 20)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y + 1);
                                                    switch (riskLevel(x, y + 1, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    cnt++;
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value0.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value1.Item2);
                                                                    cnt++;
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value1.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                    }
                                                }

                                                break;
                                            case 1://wet None
                                                if (x > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x - 1, y);
                                                    switch (riskLevel(x - 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    cnt++;
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value0.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (y > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y - 1);
                                                    switch (riskLevel(x, y - 1, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    cnt++;
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value0.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (x + 1 <= targetX + 40)
                                                {
                                                    var key0 = new Tuple<int, int>(x + 1, y);
                                                    switch (riskLevel(x + 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    cnt++;
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value0.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (y + 1 <= targetY + 20)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y + 1);
                                                    switch (riskLevel(x, y + 1, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    cnt++;
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value0.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                    }
                                                }

                                                break;
                                            case 2://narrow Torch //start b³¹d
                                                if (x > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x - 1, y);
                                                    switch (riskLevel(x - 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    cnt++;
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value0.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value2))
                                                            {
                                                                if (value2.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value2.Item2);
                                                                    cnt++;
                                                                    if (value2.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value2.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value2.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value2.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (y > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y - 1);
                                                    switch (riskLevel(x, y - 1, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    cnt++;
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value0.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value2))
                                                            {
                                                                if (value2.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value2.Item2);
                                                                    cnt++;
                                                                    if (value2.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value2.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value2.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value2.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (x + 1 <= targetX + 40)
                                                {
                                                    var key0 = new Tuple<int, int>(x + 1, y);
                                                    switch (riskLevel(x + 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    cnt++;
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value0.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value2))
                                                            {
                                                                if (value2.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value2.Item2);
                                                                    cnt++;
                                                                    if (value2.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value2.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value2.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value2.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (y + 1 <= targetY + 20)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y + 1);
                                                    switch (riskLevel(x, y + 1, targetX, targetY, depth))
                                                    {
                                                        case 0://stone
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value0.Item2);
                                                                    cnt++;
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value0.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value2))
                                                            {
                                                                if (value2.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value2.Item2);
                                                                    cnt++;
                                                                    if (value2.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value2.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value2.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value2.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }
                                                            break;
                                                    }
                                                }

                                                break;
                                        }
                                    }
                                    else
                                    {
                                        switch (riskLevel(x, y, targetX, targetY, depth))
                                        {
                                            case 0://rock gear
                                                if (x > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x - 1, y);
                                                    switch (riskLevel(x - 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0://rock
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value0.Item2);
                                                                    }
                                                                }

                                                                //if (value0.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }

                                                            break;
                                                        case 2:


                                                            break;
                                                    }
                                                }

                                                if (y > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y - 1);
                                                    switch (riskLevel(x, y - 1, targetX, targetY, depth))
                                                    {
                                                        case 0://rock
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 1);
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value0.Item2);
                                                                    }
                                                                }

                                                                //if (value0.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }

                                                            break;
                                                        case 2:


                                                            break;
                                                    }
                                                }

                                                if (x + 1 <= targetX + 40)
                                                {
                                                    var key0 = new Tuple<int, int>(x + 1, y);
                                                    switch (riskLevel(x + 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0://rock
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value0.Item2);
                                                                    }
                                                                }

                                                                //if (value0.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }

                                                            break;
                                                        case 2:


                                                            break;
                                                    }
                                                }

                                                if (y + 1 <= targetY + 20)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y + 1);
                                                    switch (riskLevel(x, y + 1, targetX, targetY, depth))
                                                    {
                                                        case 0://rock
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value0.Item2);
                                                                    }
                                                                }

                                                                //if (value0.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }

                                                            break;
                                                        case 2:


                                                            break;
                                                    }
                                                }

                                                break;
                                            case 1://wet Gear
                                                if (x > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x - 1, y);
                                                    switch (riskLevel(x - 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0://rock
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value0.Item2);
                                                                    }
                                                                }

                                                                //if (value0.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }

                                                            break;
                                                        case 2:
                                                            break;
                                                    }
                                                }

                                                if (y > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y - 1);
                                                    switch (riskLevel(x, y - 1, targetX, targetY, depth))
                                                    {
                                                        case 0://rock
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value0.Item2);
                                                                    }
                                                                }

                                                                //if (value0.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }

                                                            break;
                                                        case 2:
                                                            break;
                                                    }
                                                }

                                                if (x + 1 <= targetX + 40)
                                                {
                                                    var key0 = new Tuple<int, int>(x + 1, y);
                                                    switch (riskLevel(x + 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0://rock
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value0.Item2);
                                                                    }
                                                                }

                                                                //if (value0.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }

                                                            break;
                                                        case 2:
                                                            break;
                                                    }
                                                }

                                                if (y + 1 <= targetY + 20)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y + 1);
                                                    switch (riskLevel(x, y + 1, targetX, targetY, depth))
                                                    {
                                                        case 0://rock
                                                            if (timeDic.TryGetValue(key0, out var value0))
                                                            {
                                                                if (value0.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value0.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value0.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value0.Item2);
                                                                    }
                                                                }

                                                                //if (value0.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value1.Item2);
                                                                    }
                                                                }

                                                                //if (value1.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }

                                                            break;
                                                        case 2:
                                                            break;
                                                    }
                                                }

                                                break;
                                            case 2://narrow None
                                                if (x > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x - 1, y);
                                                    switch (riskLevel(x - 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0:
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value1.Item2);
                                                                    cnt++;
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value1.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }

                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value2))
                                                            {
                                                                if (value2.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value2.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value2.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value2.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value2.Item2);
                                                                    }
                                                                }

                                                                //if (value2.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (y > 0)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y - 1);
                                                    switch (riskLevel(x, y - 1, targetX, targetY, depth))
                                                    {
                                                        case 0:
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value1.Item2);
                                                                    cnt++;
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value1.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }

                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value2))
                                                            {
                                                                if (value2.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value2.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value2.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value2.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value2.Item2);
                                                                    }
                                                                }

                                                                //if (value2.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (x + 1 <= targetX + 40)
                                                {
                                                    var key0 = new Tuple<int, int>(x + 1, y);
                                                    switch (riskLevel(x + 1, y, targetX, targetY, depth))
                                                    {
                                                        case 0:
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value1.Item2);
                                                                    cnt++;
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value1.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }

                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value2))
                                                            {
                                                                if (value2.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value2.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value2.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value2.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value2.Item2);
                                                                    }
                                                                }

                                                                //if (value2.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                    }
                                                }

                                                if (y + 1 <= targetY + 20)
                                                {
                                                    var key0 = new Tuple<int, int>(x, y + 1);
                                                    switch (riskLevel(x, y + 1, targetX, targetY, depth))
                                                    {
                                                        case 0:
                                                            break;
                                                        case 1://wet
                                                            if (timeDic.TryGetValue(key0, out var value1))
                                                            {
                                                                if (value1.Item1 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(currentTime + 1, value1.Item2);
                                                                    cnt++;
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value1.Item2 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(value1.Item1, currentTime + 8);
                                                                    }
                                                                }

                                                                //if (value1.Item2 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 1, currentTime + 8);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 1, currentTime + 8));
                                                            }

                                                            break;
                                                        case 2://narrow
                                                            if (timeDic.TryGetValue(key0, out var value2))
                                                            {
                                                                if (value2.Item2 > currentTime + 1)
                                                                {
                                                                    timeDic[key0] = new Tuple<int, int>(value2.Item1, currentTime + 1);
                                                                    cnt++;
                                                                    if (value2.Item1 > currentTime + 8)
                                                                    {
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    if (value2.Item1 > currentTime + 8)
                                                                    {
                                                                        cnt++;
                                                                        timeDic[key0] = new Tuple<int, int>(currentTime + 8, value2.Item2);
                                                                    }
                                                                }

                                                                //if (value2.Item1 > currentTime + 8)
                                                                //{
                                                                //    timeDic[key0] = new Tuple<int, int>(currentTime + 8, currentTime + 1);
                                                                //}
                                                            }
                                                            else
                                                            {
                                                                cnt++;
                                                                timeDic.Add(key0, new Tuple<int, int>(currentTime + 8, currentTime + 1));
                                                            }
                                                            break;
                                                    }
                                                }

                                                break;
                                        }
                                    }
                                }
                            }

                        }

                        //totalRisk += riskLevel(x, y, targetX, targetY, depth);
                    }
                }
                endtmp = timeDic[new Tuple<int, int>(targetX, targetY)];
                if (!lista.Contains(endtmp.Item1))
                {
                    lista.Add(endtmp.Item1);
                    min = lista.Min();
                }
                tU = riskLevel(targetX, targetY - 1, targetX, targetY, depth);
                tD = riskLevel(targetX, targetY + 1, targetX, targetY, depth);
                tL = riskLevel(targetX - 1, targetY, targetX, targetY, depth);
                tR = riskLevel(targetX + 1, targetY, targetX, targetY, depth);
                listaCnt.Add(cnt);
                if (cnt == 0)
                {
                    break;
                }
            }

            //for (int i = 0; i <= targetX + 40; i++)
            //{
            //    for (int j = 0; j < targetY + 20; j++)
            //    {
            //        Console.Write(string.Format("{0:00} ", timeDic[new Tuple<int, int>(i, j)].Item1));
            //    }
            //    Console.Write('\n');
            //}
            //Console.WriteLine();

            //for (int i = 0; i <= targetX + 40; i++)
            //{
            //    for (int j = 0; j < targetY + 20; j++)
            //    {
            //        Console.Write(string.Format("{0:00} ", timeDic[new Tuple<int, int>(i, j)].Item2));
            //    }
            //    Console.Write('\n');
            //}

            var end = timeDic[new Tuple<int, int>(targetX, targetY)];
            var up = timeDic[new Tuple<int, int>(targetX, targetY - 1)];
            var down = timeDic[new Tuple<int, int>(targetX, targetY + 1)];
            var left = timeDic[new Tuple<int, int>(targetX - 1, targetY)];
            var right = timeDic[new Tuple<int, int>(targetX + 1, targetY)];
            tU = riskLevel(targetX, targetY - 1, targetX, targetY, depth);
            tD = riskLevel(targetX, targetY + 1, targetX, targetY, depth);
            tL = riskLevel(targetX - 1, targetY, targetX, targetY, depth);
            tR = riskLevel(targetX + 1, targetY, targetX, targetY, depth);
            Console.WriteLine(end.Item1);

            //var input = File.ReadAllLines("test.txt");

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            //Console.ReadLine();
        }
    }
}