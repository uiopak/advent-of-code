using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day19
{
    public class Program
    {
        public static void addr(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = register[instruction[1]] + register[instruction[2]];
        }
        public static void addi(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = register[instruction[1]] + instruction[2];
        }
        public static void mulr(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = register[instruction[1]] * register[instruction[2]];
        }
        public static void muli(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = register[instruction[1]] * instruction[2];
        }
        public static void banr(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = (register[instruction[1]] & register[instruction[2]]);
        }
        public static void bani(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = (register[instruction[1]] & instruction[2]);
        }
        public static void borr(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = (register[instruction[1]] | register[instruction[2]]);
        }
        public static void bori(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = (register[instruction[1]] | instruction[2]);
        }
        public static void setr(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = register[instruction[1]];
        }
        public static void seti(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = instruction[1];
        }
        public static void gtir(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = ((instruction[1] > register[instruction[2]]) ? (ulong)1 : 0);
        }
        public static void gtri(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = ((register[instruction[1]] > instruction[2]) ? (ulong)1 : 0);
        }
        public static void gtrr(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = ((register[instruction[1]] > register[instruction[2]]) ? (ulong)1 : 0);
        }
        public static void eqir(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = ((instruction[1] == register[instruction[2]]) ? (ulong)1 : 0);
        }
        public static void eqri(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = ((register[instruction[1]] == instruction[2]) ? (ulong)1 : 0);
        }
        public static void eqrr(ulong[] register, ulong[] instruction)
        {
            register[instruction[3]] = ((register[instruction[1]] == register[instruction[2]]) ? (ulong)1 : 0);
        }


        public static void Main()
        {
            //var dir = Directory.GetCurrentDirectory();
            //Console.WriteLine(dir);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //int playersNo = 453;
            //int playersNo = 10;
            //int maxMarble = 1618;
            //int playersNo = 13;
            //int maxMarble = 7999;
            //int maxMarble = 70784;

            var input = File.ReadAllLines("input");


            var list = new List<ulong[]>();
            var listInstr = new List<string>();
            var register = new ulong[6];
            var instrCnt = input.Length;
            var ip = 0;
            int poz = 0;
            for (var i = 0; i < input.Length; i++)
            {
                //if (input[i].Length < "Before: [".Length)
                //{
                //    break;
                //}
                var test = input[i].Split(' ');
                listInstr.Add(test[0]);
                string[] numbers1 = Regex.Split(input[i], @"\D+");
                //string[] numbers2 = Regex.Split(input[i + 1], @"\D+");
                //string[] numbers3 = Regex.Split(input[i + 2], @"\D+");
                list.Add(
                    new ulong[] { 0, ulong.Parse(numbers1[1]), ulong.Parse(numbers1[2]), ulong.Parse(numbers1[3]) });
                poz += 1;
            }
            ulong insI = 4;

            /*
            //register[0] = 0;
            while (ip<=instrCnt-1)
            {
                var instr = list[ip];
                if (listInstr[ip]=="addr")
                {
                    addr(register, instr);
                }
                else if (listInstr[ip]=="addi")
                {
                    addi(register, instr);
                }
                else if (listInstr[ip]=="mulr")
                {
                    mulr(register, instr);
                }
                else if (listInstr[ip]=="muli")
                {
                    muli(register, instr);
                }
                else if (listInstr[ip]=="banr")
                {
                    banr(register, instr);
                }
                else if (listInstr[ip]=="bani")
                {
                    bani(register, instr);
                }
                else if (listInstr[ip]=="borr")
                {
                    borr(register, instr);
                }
                else if (listInstr[ip]=="bori")
                {
                    bori(register, instr);
                }
                else if (listInstr[ip]=="setr")
                {
                    setr(register, instr);
                }
                else if (listInstr[ip]=="seti")
                {
                    seti(register, instr);
                }
                else if (listInstr[ip]=="gtir")
                {
                    gtir(register, instr);
                }
                else if (listInstr[ip]=="gtri")
                {
                    gtri(register, instr);
                }
                else if (listInstr[ip]=="gtrr")
                {
                    gtrr(register, instr);
                }
                else if (listInstr[ip]=="eqir")
                {
                    eqir(register, instr);
                }
                else if (listInstr[ip]=="eqri")
                {
                    eqri(register, instr);
                }
                else if (listInstr[ip]=="eqrr")
                {
                    eqrr(register, instr);
                }
                ip = register[insI];
                ip++;
                /*Console.Write(ip + ":");
                foreach (var item in register)
                {
                    Console.Write(item + ",");
                }
                Console.Write('\n');*/
            /*
            register[insI]++;
        }
        Console.WriteLine(register[0]);*/
            ulong cnt = 0;
            ip = 0;
            register[0] = 0;
            register[1] = 0;
            register[2] = 10551298;
            //register[2] = 898;            
            register[3] = 0;
            register[4] = 0;
            register[5] = 0;
            register[insI] = 0;
            //Console.WriteLine((ulong)(Math.Sqrt((double)register[2])));
            for (register[1] = 1; register[1] < (ulong)(Math.Sqrt((double)register[2])); register[1]++)
            {
                if (register[2] % register[1] == 0)
                {
                    register[0] += register[1] + (register[2] / register[1]);
                }
            }
            //for (register[1] = 1; register[1] <= register[2]; register[1]++)
            //{
            //    for (register[3] = 1; register[3] <= register[2]; register[3]++)
            //    {
            //        if (register[3] * register[1] == register[2])
            //        {
            //            register[0] += register[1];
            //        }
            //    }
            //}
            while (false)
            {
            r1: register[1] = 1;
            r2: register[3] = 1;
            r3:
                register[5] = register[1] * register[3];
                if (register[5] == register[2])
                {
                    register[0] += register[1];
                }
                register[3] += 1;
                if (register[3] > register[2])
                {
                    register[5] = 1;
                }
                else
                {
                    register[5] = 0;
                }
                if (register[5] == 0)
                {
                    goto r3;
                }

                register[1]++;
                if (register[1] > register[2])
                {
                    register[5] = 1;
                }
                else
                {
                    register[5] = 0;
                }
                if (register[5] == 0)
                {
                    goto r2;
                }

                break;
            }
            //while (ip <= instrCnt - 1)
            //{
            //    var instr = list[ip];
            //    if (listInstr[ip] == "addr")
            //    {
            //        addr(register, instr);
            //    }
            //    else if (listInstr[ip] == "addi")
            //    {
            //        addi(register, instr);
            //    }
            //    else if (listInstr[ip] == "mulr")
            //    {
            //        mulr(register, instr);
            //    }
            //    else if (listInstr[ip] == "muli")
            //    {
            //        muli(register, instr);
            //    }
            //    else if (listInstr[ip] == "banr")
            //    {
            //        banr(register, instr);
            //    }
            //    else if (listInstr[ip] == "bani")
            //    {
            //        bani(register, instr);
            //    }
            //    else if (listInstr[ip] == "borr")
            //    {
            //        borr(register, instr);
            //    }
            //    else if (listInstr[ip] == "bori")
            //    {
            //        bori(register, instr);
            //    }
            //    else if (listInstr[ip] == "setr")
            //    {
            //        setr(register, instr);
            //    }
            //    else if (listInstr[ip] == "seti")
            //    {
            //        seti(register, instr);
            //    }
            //    else if (listInstr[ip] == "gtir")
            //    {
            //        gtir(register, instr);
            //    }
            //    else if (listInstr[ip] == "gtri")
            //    {
            //        gtri(register, instr);
            //    }
            //    else if (listInstr[ip] == "gtrr")
            //    {
            //        gtrr(register, instr);
            //    }
            //    else if (listInstr[ip] == "eqir")
            //    {
            //        eqir(register, instr);
            //    }
            //    else if (listInstr[ip] == "eqri")
            //    {
            //        eqri(register, instr);
            //    }
            //    else if (listInstr[ip] == "eqrr")
            //    {
            //        eqrr(register, instr);
            //    }
            //    Console.Write(ip + ":");
            //    ip = (int)register[insI];
            //    ip++;
            //    foreach (var item in register)
            //    {
            //        Console.Write(item + ",");
            //    }
            //    Console.Write('\n');
            //    register[insI]++;
            //    cnt++;
            //}
            Console.WriteLine(register[0]);
            //var input = File.ReadAllLines("test.txt");

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}