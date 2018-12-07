using System;
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
            //var dir = Directory.GetCurrentDirectory();
            //Console.WriteLine(dir);


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllLines("input");
            //var input = File.ReadAllLines("test.txt");
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
            Dictionary<char, int> doing = new Dictionary<char, int>();
            int sec = 0;
            int workers = 5;
            int cnt = instrList.Count;
            while (done.Count < cnt)
            {
                for (int i = 0; i < doing.Count; i++)
                {
                    if (doing.ElementAt(i).Value == sec)
                    {
                        done.Add(doing.ElementAt(i).Key);
                        doing.Remove(doing.ElementAt(i).Key);
                        i--;
                    }
                }
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
                        if (doing.Count < workers)
                        {
                            doing.Add(instr, sec + 60 + instr - 'A' + 1);
                            instrList.Remove(instr);
                            j = -1;
                        }

                    outside:;
                    }
                    else
                    {
                        if (doing.Count < workers)
                        {
                            doing.Add(instr, sec + 60 + instr - 'A' + 1);
                            instrList.Remove(instr);
                            j = -1;
                        }
                    }
                }
                sec++;

            }
            foreach (var item in done)
            {
                Console.Write(item);
            }
            Console.WriteLine(sec - 1);

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}