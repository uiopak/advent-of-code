using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day8
{
    public class Program
    {
        public static int Node(ref int[] numbers, ref int index)
        {
            int numberOfChildren = numbers[index++];
            int nubmerOfMeta = numbers[index++];
            int tmp = 0;
            int[] childrens = new int[numberOfChildren];
            for (int i = 0; i < numberOfChildren; i++)
            {
                childrens[i] = Node(ref numbers, ref index);
            }
            for (int i = 0; i < nubmerOfMeta; i++)
            {
                if (numberOfChildren > 0)
                {

                    if (numbers[index] > numberOfChildren)
                    {
                        index++;
                    }
                    else
                    {
                        tmp += childrens[numbers[index++] - 1];
                    }
                }
                else
                {
                    tmp += numbers[index++];
                }
            }
            return tmp;
        }


        public static void Main()
        {
            //var dir = Directory.GetCurrentDirectory();
            //Console.WriteLine(dir);


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllText("input");
            int index = 0;
            int[] numbers = Array.ConvertAll(input.Split(' ').ToArray(), int.Parse);

            Console.WriteLine(Node(ref numbers, ref index));

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}