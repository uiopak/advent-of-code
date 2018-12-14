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
            //var result = new int[inputLength];
            //for (int i = result.Length - 1; i >= 0; i--)
            //{
            //    result[i] = int.Parse(inputString[i]);
            //}
            LinkedList<uint> recipes = new LinkedList<uint>();
            recipes.AddLast(3);
            recipes.AddLast(7);
            LinkedListNode<uint> firstElf = recipes.First;
            LinkedListNode<uint> secondElf = recipes.Last;
            LinkedListNode<uint> lastScore = recipes.Last;
            int before = 1;

            while (true)
            {
                var newScore = firstElf.Value + secondElf.Value;
                if (newScore > 9)
                {
                    var first = newScore / 10;
                    newScore -= 10;
                    recipes.AddLast(first);
                }
                recipes.AddLast(newScore);

                if (recipes.Count > 7)
                {
                    //if (lastScore.Value== result[0])
                    //{
                    //    if (lastScore.Next.Value == result[1])
                    //    {
                    //        if (lastScore.Next.Next.Value == result[2])
                    //        {
                    //            if (lastScore.Next.Next.Next.Value == result[3])
                    //            {
                    //                if (lastScore.Next.Next.Next.Next.Value == result[4])
                    //                {
                    //                    if (lastScore.Next.Next.Next.Next.Next.Value == result[5])
                    //                    {
                    //                        break;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //before++;
                    //lastScore = lastScore.Next;
                    uint tmp = 0;
                    LinkedListNode<uint> tmpNode = lastScore;
                    for (int i = 0; i < inputLength; i++)
                    {
                        tmp += tmpNode.Value;
                        tmp *= 10;
                        tmpNode = tmpNode.Next;
                    }

                    tmp /= 10;
                    if (tmp == input)
                    {
                        break;
                    }

                    before++;
                    lastScore = lastScore.Next;
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
            Console.WriteLine(before);

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}