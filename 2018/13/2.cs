using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Day13
{
    public class Program
    {
        public enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }
        public enum Intersection
        {
            Left,
            Straight,
            Right
        }
        public class Cart
        {
            public Direction direction;
            public Intersection nextIntersection;
            public int x;
            public int y;
            public Cart(char c, int x, int y)
            {
                setDirection(c);
                nextIntersection = Intersection.Left;
                this.x = x;
                this.y = y;
            }

            public Intersection getCurrentAndChangeTooNextIntersection()
            {
                Intersection tmp = nextIntersection;
                nextIntersection = (Intersection)((int)(nextIntersection + 1) % 3);
                return tmp;
            }

            public bool move(List<string> lines, Dictionary<Tuple<int, int>, Cart> dic, List<string> clean)
            {
                char nextF;
                int newX;
                int newY;
                dic.Remove(new Tuple<int, int>(x, y));
                switch (direction)
                {
                    case Direction.Up:
                        nextF = lines[y - 1][x];
                        newX = x;
                        newY = y - 1;
                        break;
                    case Direction.Right:
                        nextF = lines[y][x + 1];
                        newX = x + 1;
                        newY = y;
                        break;
                    case Direction.Down:
                        nextF = lines[y + 1][x];
                        newX = x;
                        newY = y + 1;
                        break;
                    case Direction.Left:
                        nextF = lines[y][x - 1];
                        newX = x - 1;
                        newY = y;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                switch (nextF)
                {
                    case '-':
                    case '|':
                        break;
                    case '\\':
                        switch (direction)
                        {
                            case Direction.Up:
                                direction = Direction.Left;
                                break;
                            case Direction.Right:
                                direction = Direction.Down;
                                break;
                            case Direction.Down:
                                direction = Direction.Right;
                                break;
                            case Direction.Left:
                                direction = Direction.Up;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    case '/':
                        switch (direction)
                        {
                            case Direction.Up:
                                direction = Direction.Right;
                                break;
                            case Direction.Right:
                                direction = Direction.Up;
                                break;
                            case Direction.Down:
                                direction = Direction.Left;
                                break;
                            case Direction.Left:
                                direction = Direction.Down;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        break;
                    case '+':
                        switch (nextIntersection)
                        {
                            case Intersection.Left:
                                direction = (Direction)((int)(4 + direction - 1) % 4);
                                break;
                            case Intersection.Straight:
                                break;
                            case Intersection.Right:
                                direction = (Direction)((int)(4 + direction + 1) % 4);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        nextIntersection = (Intersection)((int)(nextIntersection + 1) % 3);
                        break;
                    default:
                        break;
                }
                Cart tmp;
                if (dic.TryGetValue(new Tuple<int, int>(newX, newY), out tmp))
                {
                    //Console.WriteLine(newX + "," + newY);
                    var theString = lines[newY];
                    char c = clean[newY][newX];
                    var aStringBuilder = new StringBuilder(theString);
                    aStringBuilder.Remove(newX, 1);
                    aStringBuilder.Insert(newX, c);
                    lines[newY] = aStringBuilder.ToString();
                    dic.Remove(new Tuple<int, int>(newX, newY));
                    return true;
                }
                else
                {
                    x = newX;
                    y = newY;
                    var theString = lines[y];
                    char c;
                    switch (direction)
                    {
                        case Direction.Up:
                            c = '^';
                            break;
                        case Direction.Right:
                            c = '>';
                            break;
                        case Direction.Down:
                            c = 'v';
                            break;
                        case Direction.Left:
                            c = '<';
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    var aStringBuilder = new StringBuilder(theString);
                    aStringBuilder.Remove(x, 1);
                    aStringBuilder.Insert(x, c);
                    lines[y] = aStringBuilder.ToString();
                    if (dic.Count == 0)
                    {
                        Console.WriteLine(newX + "," + newY);

                        return false;
                    }
                    dic.Add(new Tuple<int, int>(newX, newY), this);
                }

                return true;
            }

            public void setDirection(char c)
            {
                switch (c)
                {
                    case '>':
                        direction = Direction.Right;
                        break;
                    case '<':
                        direction = Direction.Left;
                        break;
                    case '^':
                        direction = Direction.Up;
                        break;
                    case 'v':
                        direction = Direction.Down;
                        break;

                }
            }
        }


        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllLines("input");
            //var input = File.ReadAllLines("test.txt");
            var linesClean = new List<string>();
            var linesWithPlayers = new List<string>();
            var CartDictionary = new Dictionary<Tuple<int, int>, Cart>();
            int tmpY = 0;
            int tmpX = 0;
            foreach (var line in input)
            {
                var tmpStringClean = "";
                var tmpStringWithPlayers = "";
                tmpX = 0;
                foreach (var character in line)
                {
                    switch (character)
                    {
                        case '<':
                        case '>':
                            tmpStringClean += '-';
                            CartDictionary.Add(new Tuple<int, int>(tmpX, tmpY), new Cart(character, tmpX, tmpY));
                            break;
                        case '^':
                        case 'v':
                            tmpStringClean += '|';
                            CartDictionary.Add(new Tuple<int, int>(tmpX, tmpY), new Cart(character, tmpX, tmpY));
                            break;
                        default:
                            tmpStringClean += character;
                            break;
                    }
                    tmpStringWithPlayers += character;
                    tmpX++;
                }

                linesClean.Add(tmpStringClean);
                linesWithPlayers.Add(tmpStringWithPlayers);
                tmpY++;
            }

            while (true)
            {
                var cleanListCopy = new List<string>(linesClean);
                for (int i = 0; i < linesClean.Count; i++)
                {
                    for (int j = 0; j < linesClean[0].Length; j++)
                    {
                        if (linesWithPlayers[i][j] == '<' || linesWithPlayers[i][j] == '>' || linesWithPlayers[i][j] == '^' || linesWithPlayers[i][j] == 'v')
                        {
                            Cart tmp;
                            if (CartDictionary.TryGetValue(new Tuple<int, int>(j, i), out tmp))
                            {
                                if (!CartDictionary[new Tuple<int, int>(j, i)].move(linesClean, CartDictionary, cleanListCopy))
                                {
                                    goto end;
                                }
                            }
                        }
                    }
                }

                linesWithPlayers = linesClean;
                linesClean = cleanListCopy;
            }
        end:;

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}