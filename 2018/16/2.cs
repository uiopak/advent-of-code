using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Day16
{
    public class Program
    {
        public static void addr(int[] register, int[] instruction)
        {
            register[instruction[3]] = register[instruction[1]] + register[instruction[2]];
        }
        public static void addi(int[] register, int[] instruction)
        {
            register[instruction[3]] = register[instruction[1]] + instruction[2];
        }
        public static void mulr(int[] register, int[] instruction)
        {
            register[instruction[3]] = register[instruction[1]] * register[instruction[2]];
        }
        public static void muli(int[] register, int[] instruction)
        {
            register[instruction[3]] = register[instruction[1]] * instruction[2];
        }
        public static void banr(int[] register, int[] instruction)
        {
            register[instruction[3]] = (register[instruction[1]] & register[instruction[2]]);
        }
        public static void bani(int[] register, int[] instruction)
        {
            register[instruction[3]] = (register[instruction[1]] & instruction[2]);
        }
        public static void borr(int[] register, int[] instruction)
        {
            register[instruction[3]] = (register[instruction[1]] | register[instruction[2]]);
        }
        public static void bori(int[] register, int[] instruction)
        {
            register[instruction[3]] = (register[instruction[1]] | instruction[2]);
        }
        public static void setr(int[] register, int[] instruction)
        {
            register[instruction[3]] = register[instruction[1]];
        }
        public static void seti(int[] register, int[] instruction)
        {
            register[instruction[3]] = instruction[1];
        }
        public static void gtir(int[] register, int[] instruction)
        {
            register[instruction[3]] = ((instruction[1] > register[instruction[2]]) ? 1 : 0);
        }
        public static void gtri(int[] register, int[] instruction)
        {
            register[instruction[3]] = ((register[instruction[1]] > instruction[2]) ? 1 : 0);
        }
        public static void gtrr(int[] register, int[] instruction)
        {
            register[instruction[3]] = ((register[instruction[1]] > register[instruction[2]]) ? 1 : 0);
        }
        public static void eqir(int[] register, int[] instruction)
        {
            register[instruction[3]] = ((instruction[1] == register[instruction[2]]) ? 1 : 0);
        }
        public static void eqri(int[] register, int[] instruction)
        {
            register[instruction[3]] = ((register[instruction[1]] == instruction[2]) ? 1 : 0);
        }
        public static void eqrr(int[] register, int[] instruction)
        {
            register[instruction[3]] = ((register[instruction[1]] == register[instruction[2]]) ? 1 : 0);
        }

        public static bool addrT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == elem.Item1[elem.Item2[1]] + elem.Item1[elem.Item2[2]])
            {
                return true;
            }
            return false;
        }
        public static bool addiT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == elem.Item1[elem.Item2[1]] + elem.Item2[2])
            {
                return true;
            }
            return false;
        }
        public static bool mulrT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == elem.Item1[elem.Item2[1]] * elem.Item1[elem.Item2[2]])
            {
                return true;
            }
            return false;
        }
        public static bool muliT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == elem.Item1[elem.Item2[1]] * elem.Item2[2])
            {
                return true;
            }
            return false;
        }
        public static bool banrT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == (elem.Item1[elem.Item2[1]] & elem.Item1[elem.Item2[2]]))
            {
                return true;
            }
            return false;
        }
        public static bool baniT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == (elem.Item1[elem.Item2[1]] & elem.Item2[2]))
            {
                return true;
            }
            return false;
        }
        public static bool borrT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == (elem.Item1[elem.Item2[1]] | elem.Item1[elem.Item2[2]]))
            {
                return true;
            }
            return false;
        }
        public static bool boriT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == (elem.Item1[elem.Item2[1]] | elem.Item2[2]))
            {
                return true;
            }
            return false;
        }
        public static bool setrT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == elem.Item1[elem.Item2[1]])
            {
                return true;
            }
            return false;
        }
        public static bool setiT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == elem.Item2[1])
            {
                return true;
            }
            return false;
        }
        public static bool gtirT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == ((elem.Item2[1] > elem.Item1[elem.Item2[2]]) ? 1 : 0))
            {
                return true;
            }
            return false;
        }
        public static bool gtriT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == ((elem.Item1[elem.Item2[1]] > elem.Item2[2]) ? 1 : 0))
            {
                return true;
            }
            return false;
        }
        public static bool gtrrT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == ((elem.Item1[elem.Item2[1]] > elem.Item1[elem.Item2[2]]) ? 1 : 0))
            {
                return true;
            }
            return false;
        }
        public static bool eqirT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == ((elem.Item2[1] == elem.Item1[elem.Item2[2]]) ? 1 : 0))
            {
                return true;
            }
            return false;
        }
        public static bool eqriT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == ((elem.Item1[elem.Item2[1]] == elem.Item2[2]) ? 1 : 0))
            {
                return true;
            }
            return false;
        }
        public static bool eqrrT(Tuple<int[], int[], int[]> elem)
        {
            if (elem.Item3[elem.Item2[3]] == ((elem.Item1[elem.Item2[1]] == elem.Item1[elem.Item2[2]]) ? 1 : 0))
            {
                return true;
            }
            return false;
        }

        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var input = File.ReadAllLines("input");
            var list = new List<Tuple<int[], int[], int[]>>();
            int poz = 0;
            for (int i = 0; i < input.Length; i += 4)
            {
                if (input[i].Length < "Before: [".Length)
                {
                    break;
                }
                string[] numbers1 = Regex.Split(input[i], @"\D+");
                string[] numbers2 = Regex.Split(input[i + 1], @"\D+");
                string[] numbers3 = Regex.Split(input[i + 2], @"\D+");
                list.Add(new Tuple<int[], int[], int[]>(
                    new int[] { int.Parse(numbers1[1]), int.Parse(numbers1[2]), int.Parse(numbers1[3]), int.Parse(numbers1[4]) },
                    new int[] { int.Parse(numbers2[0]), int.Parse(numbers2[1]), int.Parse(numbers2[2]), int.Parse(numbers2[3]) },
                    new int[] { int.Parse(numbers3[1]), int.Parse(numbers3[2]), int.Parse(numbers3[3]), int.Parse(numbers3[4]) }));
                poz += 4;
            }

            poz += 2;
            var instruction = new List<int[]>();
            for (int i = poz; i < input.Length; i++)
            {
                string[] numbers = Regex.Split(input[i], @"\D+");
                instruction.Add(new int[] { int.Parse(numbers[0]), int.Parse(numbers[1]), int.Parse(numbers[2]), int.Parse(numbers[3]) });
            }

            int c = 0;
            var listsList = new List<List<int>>();
            var addrList = new List<int>();
            listsList.Add(addrList);
            var addiList = new List<int>();
            listsList.Add(addiList);
            var mulrList = new List<int>();
            listsList.Add(mulrList);
            var muliList = new List<int>();
            listsList.Add(muliList);
            var banrList = new List<int>();
            listsList.Add(banrList);
            var baniList = new List<int>();
            listsList.Add(baniList);
            var borrList = new List<int>();
            listsList.Add(borrList);
            var boriList = new List<int>();
            listsList.Add(boriList);
            var setrList = new List<int>();
            listsList.Add(setrList);
            var setiList = new List<int>();
            listsList.Add(setiList);
            var gtirList = new List<int>();
            listsList.Add(gtirList);
            var gtriList = new List<int>();
            listsList.Add(gtriList);
            var gtrrList = new List<int>();
            listsList.Add(gtrrList);
            var eqirList = new List<int>();
            listsList.Add(eqirList);
            var eqriList = new List<int>();
            listsList.Add(eqriList);
            var eqrrList = new List<int>();
            listsList.Add(eqrrList);
            for (int i = 0; i < 16; i++)
            {
                bool addrAllOk = true;
                bool addiAllOk = true;
                bool mulrAllOk = true;
                bool muliAllOk = true;
                bool banrAllOk = true;
                bool baniAllOk = true;
                bool borrAllOk = true;
                bool boriAllOk = true;
                bool setrAllOk = true;
                bool setiAllOk = true;
                bool gtirAllOk = true;
                bool gtriAllOk = true;
                bool gtrrAllOk = true;
                bool eqirAllOk = true;
                bool eqriAllOk = true;
                bool eqrrAllOk = true;
                var selected = list.Where(x => x.Item2[0] == i);
                foreach (var tuple in selected)
                {
                    if (!addrT(tuple))
                    {
                        addrAllOk = false;
                    }
                    if (!addiT(tuple))
                    {
                        addiAllOk = false;
                    }
                    if (!mulrT(tuple))
                    {
                        mulrAllOk = false;
                    }
                    if (!muliT(tuple))
                    {
                        muliAllOk = false;
                    }
                    if (!banrT(tuple))
                    {
                        banrAllOk = false;
                    }
                    if (!baniT(tuple))
                    {
                        baniAllOk = false;
                    }
                    if (!borrT(tuple))
                    {
                        borrAllOk = false;
                    }
                    if (!boriT(tuple))
                    {
                        boriAllOk = false;
                    }
                    if (!setrT(tuple))
                    {
                        setrAllOk = false;
                    }
                    if (!setiT(tuple))
                    {
                        setiAllOk = false;
                    }
                    if (!gtirT(tuple))
                    {
                        gtirAllOk = false;
                    }
                    if (!gtriT(tuple))
                    {
                        gtriAllOk = false;
                    }
                    if (!gtrrT(tuple))
                    {
                        gtrrAllOk = false;
                    }
                    if (!eqirT(tuple))
                    {
                        eqirAllOk = false;
                    }
                    if (!eqriT(tuple))
                    {
                        eqriAllOk = false;
                    }
                    if (!eqrrT(tuple))
                    {
                        eqrrAllOk = false;
                    }
                }

                if (addrAllOk)
                {
                    addrList.Add(i);
                    Console.WriteLine(i + "addr ok");
                }

                if (addiAllOk)
                {
                    addiList.Add(i);
                    Console.WriteLine(i + "addo ok");
                }

                if (mulrAllOk)
                {
                    mulrList.Add(i);
                    Console.WriteLine(i + "mulr ok");
                }

                if (muliAllOk)
                {
                    muliList.Add(i);
                    Console.WriteLine(i + "muli ok");
                }

                if (banrAllOk)
                {
                    banrList.Add(i);
                    Console.WriteLine(i + "banr ok");
                }

                if (baniAllOk)
                {
                    baniList.Add(i);
                    Console.WriteLine(i + "bani ok");
                }

                if (borrAllOk)
                {
                    borrList.Add(i);
                    Console.WriteLine(i + "borr ok");
                }

                if (boriAllOk)
                {
                    boriList.Add(i);
                    Console.WriteLine(i + "bori ok");
                }

                if (setrAllOk)
                {
                    setrList.Add(i);
                    Console.WriteLine(i + "setr ok");
                }

                if (setiAllOk)
                {
                    setiList.Add(i);
                    Console.WriteLine(i + "seti ok");
                }

                if (gtirAllOk)
                {
                    gtirList.Add(i);
                    Console.WriteLine(i + "gtir ok");
                }

                if (gtriAllOk)
                {
                    gtriList.Add(i);
                    Console.WriteLine(i + "gtri ok");
                }

                if (gtrrAllOk)
                {
                    gtrrList.Add(i);
                    Console.WriteLine(i + "gtrr ok");
                }

                if (eqirAllOk)
                {
                    eqirList.Add(i);
                    Console.WriteLine(i + "eqir ok");
                }

                if (eqriAllOk)
                {
                    eqriList.Add(i);
                    Console.WriteLine(i + "eqri ok");
                }

                if (eqrrAllOk)
                {
                    eqrrList.Add(i);
                    Console.WriteLine(i + "eqrr ok");
                }
            }

            while (true)
            {
                bool change = false;
                for (int i = 0; i < 16; i++)
                {
                    if (listsList[i].Count == 1)
                    {
                        for (int j = 0; j < 16; j++)
                        {
                            if (listsList[j].Count > 1)
                            {
                                if (listsList[j].Contains(listsList[i].First()))
                                {
                                    change = true;
                                    listsList[j].Remove(listsList[i].First());
                                }
                            }
                        }
                    }
                }

                if (!change)
                {
                    break;
                }
            }

            int[] register = { 0, 0, 0, 0 };
            foreach (var instr in instruction)
            {
                int i = instr[0];
                if (addrList.Contains(i))
                {
                    addr(register, instr);
                }
                if (addiList.Contains(i))
                {
                    addi(register, instr);
                }
                if (mulrList.Contains(i))
                {
                    mulr(register, instr);
                }
                if (muliList.Contains(i))
                {
                    muli(register, instr);
                }
                if (banrList.Contains(i))
                {
                    banr(register, instr);
                }
                if (baniList.Contains(i))
                {
                    bani(register, instr);
                }
                if (borrList.Contains(i))
                {
                    borr(register, instr);
                }
                if (boriList.Contains(i))
                {
                    bori(register, instr);
                }
                if (setrList.Contains(i))
                {
                    setr(register, instr);
                }
                if (setiList.Contains(i))
                {
                    seti(register, instr);
                }
                if (gtirList.Contains(i))
                {
                    gtir(register, instr);
                }
                if (gtriList.Contains(i))
                {
                    gtri(register, instr);
                }
                if (gtrrList.Contains(i))
                {
                    gtrr(register, instr);
                }
                if (eqirList.Contains(i))
                {
                    eqir(register, instr);
                }
                if (eqriList.Contains(i))
                {
                    eqri(register, instr);
                }
                if (eqrrList.Contains(i))
                {
                    eqrr(register, instr);
                }
            }
            //foreach (var tuple in list)
            //{
            //    int cnt = 0;
            //    if (addr(tuple))
            //    {
            //        cnt++;
            //    }

            //    if (addi(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (mulr(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (muli(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (banr(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (bani(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (borr(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (bori(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (setr(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (seti(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (gtir(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (gtri(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (gtrr(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (eqir(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (eqri(tuple))
            //    {
            //        cnt++;
            //    }
            //    if (eqrr(tuple))
            //    {
            //        cnt++;
            //    }

            //    if (cnt>=3)
            //    {
            //        c++;
            //    }
            //}
            Console.WriteLine(register[0]);

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}