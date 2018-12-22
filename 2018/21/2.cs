using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day21
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


            //Console.WriteLine((ulong)(Math.Sqrt((double)register[2])));
            //for (register[1] = 1; register[1] < (ulong)(Math.Sqrt((double)register[2])); register[1]++)
            //{
            //    if (register[2] % register[1] == 0)
            //    {
            //        register[0] += register[1] + (register[2] / register[1]);
            //    }
            //}
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
            Console.WriteLine(register[0]);
            ulong insI = 2;

            ulong cnt = 0;
            ip = 0;
            register[0] = 0;//10504829
            var lita = new List<ulong>();
            goto Ptl;
        g1: register[3] = 123;
            register[3] = register[3] & 456;
            if (register[3] != 72) goto g1;
            g4: register[3] = 0;
            register[4] = register[3] | 65536;
            register[3] = 10649702;
        g3: register[5] = register[4] & 255;
            register[3] = register[3] + register[5];
            register[3] = register[3] & 16777215;
            register[3] = register[3] * 65899;
            register[3] = register[3] & 16777215;
            if (256 > register[4]) goto g2;
            register[5] = (register[4] / 256);
            register[1] = (register[5] + 1) * 256;
        //    register[5] = 0;//r0 0 r1 0 r2 17 r3 13766818 r4 65536 r5 0
        //ptl: ;
        //    register[1] = (register[5] + 1) * 256;
        //    if (register[1]> register[4])
        //    {
        //        goto test;
        //    }

        //    register[5] = register[5] +1;
        //    goto ptl;
        //for (register[5] = 0; register[1] < register[4]; register[5]++)
        //{
        //    register[1] = (register[5] + 1) * 256;//((r4/256)+1)*256
        //}
        test:;//register[5] = register[5] + 1;//r0 =0 r1=1 r2=23 r3=13766818 r4 =65536 r5=256
            register[4] = register[5];
            goto g3;
        g2:
            if (register[0] != register[3])
            {
                if (!lita.Contains(register[3]))
                {
                    lita.Add(register[3]);

                }
                goto g4;
            }
        Ptl:;
            while (ip <= instrCnt - 1)
            {
                var instr = list[ip];

                switch (listInstr[ip])
                {
                    case "addr":
                        addr(register, instr);
                        break;
                    case "addi":
                        addi(register, instr);
                        break;
                    case "mulr":
                        mulr(register, instr);
                        break;
                    case "muli":
                        muli(register, instr);
                        break;
                    case "banr":
                        banr(register, instr);
                        break;
                    case "bani":
                        bani(register, instr);
                        break;
                    case "borr":
                        borr(register, instr);
                        break;
                    case "bori":
                        bori(register, instr);
                        break;
                    case "setr":
                        setr(register, instr);
                        break;
                    case "seti":
                        seti(register, instr);
                        break;
                    case "gtir":
                        gtir(register, instr);
                        break;
                    case "gtri":
                        gtri(register, instr);
                        break;
                    case "gtrr":
                        gtrr(register, instr);
                        break;
                    case "eqir":
                        eqir(register, instr);
                        break;
                    case "eqri":
                        eqri(register, instr);
                        break;
                    case "eqrr":
                        eqrr(register, instr);
                        //Console.WriteLine(register[3]);
                        //goto end;
                        //break;
                        if (register[3] == 6311823)
                        {
                            Console.WriteLine(register[3]);
                            goto end;
                        }
                        if (!lita.Contains(register[3]))
                        {
                            lita.Add(register[3]);

                        }
                        break;
                }
                //Console.Write(ip + ":");
                ip = (int)register[insI];
                ip++;
                //foreach (var item in register)
                //{
                //    Console.Write(item + ",");
                //}
                //Console.Write('\n');
                register[insI]++;
                cnt++;
            }
        end:;
            //Console.WriteLine(register[0]);
            //var input = File.ReadAllLines("test.txt");

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}