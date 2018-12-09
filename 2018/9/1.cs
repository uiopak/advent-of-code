using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day9
{
    public class Program
    {
        public static LinkedListNode<uint> currentNode;

        public static void RemoveAtIndex(LinkedList<uint> list, int index)
        {
            if (index < list.Count / 2)
            {
                LinkedListNode<uint> current = list.First;
                LinkedListNode<uint> previous = current;
                for (int i = 0; i < index; i++)
                {
                    previous = current;
                    current = current.Next;
                }
                currentNode = current.Next;
                list.Remove(current);
            }
            else
            {
                LinkedListNode<uint> current = list.Last;
                LinkedListNode<uint> previous = current;
                for (int i = list.Count; i > index + 1; i--)
                {
                    previous = current;
                    current = current.Previous;
                }
                currentNode = current.Next;
                list.Remove(current);
            }
        }

        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int playersNo;
            uint maxMarble;

            var input = File.ReadAllText("input");
            var inp = input.Split(' ');
            playersNo = int.Parse(inp[0]);
            maxMarble = uint.Parse(inp[6]);
            LinkedList<uint> board = new LinkedList<uint>();
            currentNode = board.AddFirst(0);
            uint[] players = new uint[playersNo];
            int currentPlayer = 1;
            uint currentMarble = 0;
            int currentIndex = 0;
            uint maxScore = 0;
            int maxScorePlayer = 0;
            while (currentMarble < maxMarble)
            {
                currentMarble++;
                if (currentMarble % 23 != 0)
                {
                    if (board.Count < 2)
                    {
                        board.AddAfter(currentNode, currentMarble);
                        currentIndex += 1;
                    }
                    else if (currentIndex < board.Count - 1)
                    {
                        board.AddAfter(currentNode.Next, currentMarble);
                        currentNode = currentNode.Next.Next;
                        currentIndex += 2;
                    }
                    else
                    {
                        currentNode = board.AddAfter(board.First, currentMarble);
                        currentIndex = 1;
                    }
                }
                else
                {
                    players[currentPlayer - 1] += currentMarble;
                    if (currentIndex >= 7)
                    {
                        players[currentPlayer - 1] += currentNode.Previous.Previous.Previous.Previous.Previous.Previous.Previous.Value;
                        LinkedListNode<uint> tmpNode = currentNode.Previous.Previous.Previous.Previous.Previous.Previous.Previous;
                        currentNode = tmpNode.Next;
                        board.Remove(tmpNode);
                        currentIndex -= 7;
                    }
                    else
                    {
                        int tmp = board.Count - 7 + currentIndex;
                        players[currentPlayer - 1] += board.ElementAt(tmp);
                        RemoveAtIndex(board, tmp);
                        currentIndex = tmp;
                    }
                    if (maxScore < players[currentPlayer - 1])
                    {
                        maxScore = players[currentPlayer - 1];
                        maxScorePlayer = currentPlayer;
                    }
                }
                if (currentPlayer < playersNo)
                {
                    currentPlayer++;
                }
                else
                {
                    currentPlayer = 1;
                }
            }


            Console.WriteLine(maxScorePlayer + " score: " + maxScore);

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}