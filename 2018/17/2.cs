using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day17
{
    public class Program
    {
        public static int cnt = 0;
        public static void toFile(char[][] board, int yMax)
        {
            //string[] toFile = new string[(int)(yMax + 1)];
            //for (int i = 0; i < (int)(yMax + 1); i++)
            //{

            //    toFile[i] = new string(board[i]);
            //}

            //File.WriteAllLines($@"C:\Users\Adam\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\tst\WriteLines{cnt}.txt", toFile);
        }
        public static void down(int x, int y, int xMin, int xMax, int yMax, char[][] board)
        {
            while (true)
            {
                if (y + 1 <= yMax && board[y + 1][x - xMin] == '.')
                {
                    board[y + 1][x - xMin] = '|';
                    toFile(board, yMax);
                    y = y + 1;
                    continue;
                }
                else
                {
                    if (y != yMax)
                    {
                        if (board[y + 1][x - xMin] != '|')
                        {
                            var checkR = CheckRight(x, y, xMin, xMax, yMax, board);
                            var checkL = CheckLeft(x, y, xMin, xMax, yMax, board);
                            if (checkL.Item1 && checkR.Item1)
                            {
                                for (int i = checkL.Item2 - xMin; i <= checkR.Item2 - xMin; i++)
                                {
                                    board[y][i] = '~';
                                }

                                toFile(board, yMax);
                                if (board[y][x - xMin] != '|')
                                {
                                    down(x, y - 1, xMin, xMax, yMax, board);
                                }
                            }

                            if (!checkL.Item1)
                            {
                                down(checkL.Item2, y, xMin, xMax, yMax, board);
                            }

                            if (!checkR.Item1)
                            {
                                down(checkR.Item2, y, xMin, xMax, yMax, board);
                            }
                        }
                    }
                    else
                    {

                    }
                }

                break;
            }
        }

        public static Tuple<bool, int> CheckRight(int x, int y, int xMin, int xMax, int yMax, char[][] board)
        {
            while (true)
            {
                if (board[y + 1][x + 1 - xMin] == '#' || board[y + 1][x + 1 - xMin] == '~') //jest pod³o¿e
                {
                    if (board[y][x + 1 - xMin] == '#')
                    {
                        return new Tuple<bool, int>(true, x); //mówi, ¿e znalaz³ œcianê i gdzie jest ostatni x przed œcian¹
                    }
                    else
                    {
                        board[y][x + 1 - xMin] = '|';
                        x = x + 1;
                    }
                }
                else //jest spadek
                {
                    board[y][x + 1 - xMin] = '|';
                    return new Tuple<bool, int>(false, x + 1); //mówi, ¿e znalaz³ spadek i gdzie siê zaczyna
                }
            }
        }

        public static Tuple<bool, int> CheckLeft(int x, int y, int xMin, int xMax, int yMax, char[][] board)
        {
            while (true)
            {
                if (board[y + 1][x - 1 - xMin] == '#' || board[y + 1][x - 1 - xMin] == '~') //jest pod³o¿e
                {
                    if (board[y][x - 1 - xMin] == '#')
                    {
                        return new Tuple<bool, int>(true, x); //mówi, ¿e znalaz³ œcianê i gdzie jest ostatni x przed œcian¹
                    }
                    else
                    {
                        board[y][x - 1 - xMin] = '|';
                        x = x - 1;
                    }
                }
                else //jest spadek
                {
                    board[y][x - 1 - xMin] = '|';
                    return new Tuple<bool, int>(false, x - 1); //mówi, ¿e znalaz³ spadek i gdzie siê zaczyna
                }
            }
        }

        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllLines("input");
            //var input = File.ReadAllLines("test.txt");
            var xDictionary = new Dictionary<int, List<Tuple<int, int>>>();
            var yDictionary = new Dictionary<int, List<Tuple<int, int>>>();
            int? xMin = null;
            int? xMax = null;
            int? yMin = null;
            int? yMax = null;
            foreach (var line in input)
            {
                string[] numbers = Regex.Split(line, @"\D+");
                if (line[0] == 'x')
                {
                    int key = int.Parse(numbers[1]);
                    int ymin = int.Parse(numbers[2]);
                    int ymax = int.Parse(numbers[3]);
                    if (yDictionary.ContainsKey(key))
                    {
                        yDictionary[key].Add(new Tuple<int, int>(ymin, ymax));
                    }
                    else
                    {
                        yDictionary.Add(key, new List<Tuple<int, int>>() { new Tuple<int, int>(ymin, ymax) });
                    }
                    if (xMin == null || xMin > key)
                    {
                        xMin = key;
                    }
                    if (xMax == null || xMax < key)
                    {
                        xMax = key;
                    }
                    if (yMin == null || yMin > ymin)
                    {
                        yMin = ymin;
                    }
                    if (yMax == null || yMax < ymin)
                    {
                        yMax = ymin;
                    }
                }
                else
                {
                    int key = int.Parse(numbers[1]);
                    int xmin = int.Parse(numbers[2]);
                    int xmax = int.Parse(numbers[3]);
                    if (xDictionary.ContainsKey(key))
                    {
                        xDictionary[key].Add(new Tuple<int, int>(xmin, xmax));
                    }
                    else
                    {
                        xDictionary.Add(key, new List<Tuple<int, int>>() { new Tuple<int, int>(xmin, xmax) });
                    }
                    if (xMin == null || xMin > xmin)
                    {
                        xMin = xmin;
                    }
                    if (xMax == null || xMax < xmax)
                    {
                        xMax = xmax;
                    }
                    if (yMin == null || yMin > key)
                    {
                        yMin = key;
                    }
                    if (yMax == null || yMax < key)
                    {
                        yMax = key;
                    }
                }
            }
            var board = new char[(int)(yMax + 1)][];
            xMin -= 1;
            for (int y = 0; y < yMax + 1; y++)
            {
                var tmp = new char[((int)(xMax - xMin + 2))];
                for (int x = 0; x < xMax - xMin + 2; x++)
                {
                    tmp[x] = '.';
                }
                board[y] = tmp;
            }
            foreach (var item in xDictionary)
            {
                foreach (var listElem in item.Value)
                {
                    for (int i = (int)(listElem.Item1 - xMin); i <= (listElem.Item2 - xMin); i++)
                    {
                        board[(int)(item.Key)][i] = '#';
                    }
                }
            }
            foreach (var item in yDictionary)
            {
                foreach (var listElem in item.Value)
                {
                    for (int i = (int)(listElem.Item1); i <= (listElem.Item2); i++)
                    {
                        board[i][(int)(item.Key - xMin)] = '#';
                    }
                }
            }
            board[0][(int)(500 - xMin)] = '+';
            down(500, 0, (int)xMin, (int)xMax, (int)yMax, board);
            string[] toFile = new string[(int)(yMax + 1)];
            for (int i = 0; i < (int)(yMax + 1); i++)
            {

                toFile[i] = new string(board[i]);
            }
            TimeSpan ts2 = stopWatch.Elapsed;
            System.IO.File.WriteAllLines(@"C:\Users\Adam\source\repos\ConsoleApp2\ConsoleApp2\bin\Debug\WriteLines.txt", toFile);
            int cnt = 0;
            TimeSpan ts1 = stopWatch.Elapsed;
            //for (int i = 0; i < (int)(yMax + 1); i++)
            //{
            //    for (int j = 0; j < (int)(xMax - xMin + 1); j++)
            //    {
            //        if (board[i][j] == '|')
            //        {
            //            board[i][j] = '.';
            //        }

            //    }
            //}

            // dodaæ zliczanie
            // znaleŸæ czemu czasem pisze |.| zamiast |||

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            string loop = string.Format("{0:000.000000} ms", (ts - ts1).TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.WriteLine("loop " + loop);
            string loop2 = string.Format("{0:000.000000} ms", (ts1 - ts2).TotalMilliseconds);
            Console.WriteLine("loop " + loop2);
            Console.ReadLine();
        }
    }
}