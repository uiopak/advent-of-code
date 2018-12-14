using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Day14
{
    public class Program
    {
        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var inputString = File.ReadAllText("input");
            var input = int.Parse(inputString);
            var inputLength = inputString.Length;

            LinkedList<uint> recipes = new LinkedList<uint>();
            recipes.AddLast(3);
            recipes.AddLast(7);
            LinkedListNode<uint> firstElf = recipes.First;
            LinkedListNode<uint> secondElf = recipes.Last;
            LinkedListNode<uint> lastScore = recipes.Last;

            while (recipes.Count < input + 10)
            {
                var newScore = firstElf.Value + secondElf.Value;
                if (newScore > 9)
                {
                    var first = newScore / 10;
                    newScore -= 10;
                    recipes.AddLast(first);
                }

                recipes.AddLast(newScore);
                if (recipes.Count <= input)
                {
                    lastScore = recipes.Last;
                }
                if (recipes.Count <= input + 1)
                {
                    lastScore = recipes.Last.Previous;
                }
                var loopValueFirst = firstElf.Value;
                for (int i = 0; i < loopValueFirst + 1; i++)
                {
                    if (firstElf.Next == null)
                    {
                        firstElf = recipes.First;
                    }
                    else
                    {
                        firstElf = firstElf.Next;
                    }
                }
                var loopValueSecond = secondElf.Value;

                for (int i = 0; i < loopValueSecond + 1; i++)
                {
                    if (secondElf.Next == null)
                    {
                        secondElf = recipes.First;
                    }
                    else
                    {
                        secondElf = secondElf.Next;
                    }
                }
            }
            lastScore = lastScore.Next;
            for (int i = 0; i < 10; i++)
            {
                Console.Write(lastScore.Value);
                lastScore = lastScore.Next;
            }
            Console.Write('\n');

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}