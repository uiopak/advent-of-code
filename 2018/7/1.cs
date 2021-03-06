﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Day7
{
    public class Program
    {
        class Instruction
        {
            public HashSet<char> req = new HashSet<char>();
        }


        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllLines("input");
            
            Dictionary<char, Instruction> instructions = new Dictionary<char, Instruction>();
            HashSet<char> allInstr = new HashSet<char>();

            foreach (var line in input)
            {
                var test = line.Split(' ');
                if (instructions.ContainsKey(test[7].ToCharArray().First()))
                {
                    instructions[test[7].ToCharArray().First()].req.Add(test[1].ToCharArray().First());
                }
                else
                {
                    instructions.Add(test[7].ToCharArray().First(), new Instruction());
                    instructions[test[7].ToCharArray().First()].req.Add(test[1].ToCharArray().First());
                }
                if (!allInstr.Contains(test[1].ToCharArray().First()))
                {
                    allInstr.Add(test[1].ToCharArray().First());
                }
                if (!allInstr.Contains(test[7].ToCharArray().First()))
                {
                    allInstr.Add(test[7].ToCharArray().First());
                }
            }

            var instrList = allInstr.OrderBy(x => x).ToList();
            HashSet<char> done = new HashSet<char>();
            while (instrList.Count > 0)
            {
                for (int j = 0; j < instrList.Count; j++)
                {
                    var instr = instrList.ElementAt(j);

                    if (instructions.ContainsKey(instr))
                    {

                        for (int i = 0; i < instructions[instr].req.Count; i++)
                        {
                            if (!done.Contains(instructions[instr].req.ElementAt(i)))
                            {
                                goto outside;
                            }
                        }
                        done.Add(instr);
                        instrList.Remove(instr);
                        j = -1;

                    outside:;
                    }
                    else
                    {
                        done.Add(instr);
                        instrList.Remove(instr);
                        j = -1;
                    }
                }
            }
            foreach (var item in done)
            {
                Console.Write(item);
            }
            Console.WriteLine();

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}