using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day20
{
    unsafe public class Program
    {
        public class expr
        {
            public List<expr> list;
            public string reg;
            public expr()
            {
                list = new List<expr>();
            }
        }

        public static HashSet<string> diction;

        public static void regStringList(ref List<string> prev, expr reg)
        {
            //var tmpList = new List<string>();
            //if (reg.reg != null)
            //{
            //    if (prev.Count > 0)
            //    {
            //        for (int i = 0; i < prev.Count; i++)
            //        {
            //            prev[i] = new string((prev[i] + reg.reg).ToCharArray());
            //        }
            //    }
            //    else
            //    {
            //        prev.Add(new string((reg.reg).ToCharArray()));
            //    }
            //}
            if (reg.list.Count > 0)
            {
                foreach (var r in reg.list)
                {
                    if (r.list.Count > 0)
                    {
                        if (r.reg != null)
                        {
                            if (prev.Count > 0)
                            {
                                for (int i = 0; i < prev.Count; i++)
                                {
                                    prev[i] = new string((prev[i] + r.reg).ToCharArray());
                                }
                            }
                            else
                            {
                                prev.Add(new string((r.reg).ToCharArray()));
                            }
                        }
                        var tmpList2 = new List<string>();
                        foreach (var expr in r.list)
                        {
                            var tmpList3 = new List<string>();
                            var tmpExpr = new expr();
                            tmpExpr.list.Add(expr);
                            regStringList(ref tmpList3, tmpExpr);
                            foreach (var tmp3 in tmpList3)
                            {
                                tmpList2.Add(tmp3);
                            }
                        }

                        if (prev.Count > 0)
                        {
                            var tmpList4 = new List<string>();
                            foreach (var p in prev)
                            {
                                foreach (var r2 in tmpList2)
                                {
                                    tmpList4.Add(new string((p + r2).ToCharArray()));
                                }
                            }

                            prev = tmpList4;
                        }
                        else
                        {
                            prev = tmpList2;
                        }

                        //regStringList(ref tmpList, r);
                    }
                    else
                    {
                        //if (reg.list.Count>1)
                        //{
                        //    tmpList.Add(new string((r.reg).ToCharArray()));
                        //}
                        //else
                        //{
                        if (prev.Count > 0)
                        {
                            for (int i = 0; i < prev.Count; i++)
                            {
                                prev[i] = new string((prev[i] + r.reg).ToCharArray());
                            }
                        }
                        else
                        {
                            prev.Add(new string((r.reg).ToCharArray()));
                        }

                        //}
                    }
                }
            }
            else
            {
                if (prev.Count > 0)
                {
                    for (int i = 0; i < prev.Count; i++)
                    {
                        prev[i] = new string((prev[i] + reg.reg).ToCharArray());
                    }
                }
                else
                {
                    prev.Add(new string((reg.reg).ToCharArray()));
                }
            }

            //if (reg.list.Count > 1 && prev.Count > 0)
            //{

            //    var tmpList2 = new List<string>();
            //    foreach (var t in prev)
            //    {
            //        foreach (var r in tmpList)
            //        {
            //            tmpList2.Add(new string((t + r).ToCharArray()));
            //        }
            //    }
            //    prev = tmpList2;
            //}
            //else if(reg.list.Count > 1)
            //{
            //    prev = tmpList;
            //}

            //return tmpList;
        }
        //dla list stringów robimy sobie gdzie wyl¹dujemy czyli idziemy x+1 x-1 y+1 y-1
        //robimy sobie dictionary punktów koñcowych najkrótsz¹ drog¹ do danego punktu
        //zwracamy najd³uzsz¹ drogê
        public static bool addExpr(expr parent, ref int nc, ref int no, ref int poz, string input)
        {
            var tmpExpr = new expr();
            //var tmpExprBig = new expr();
            bool run = true;
            var tmpCharList = new List<char>();
            while (run)
            {
                if (input.Length > poz)
                {
                    char c = input[poz];
                    switch (c)
                    {
                        case 'N':
                        case 'E':
                        case 'W':
                        case 'S':
                            poz++;
                            tmpCharList.Add(c);
                            break;
                        case '(':
                            no++;
                            poz++;
                            if (tmpCharList.Count > 0)
                            {
                                tmpExpr.reg = new string(tmpCharList.ToArray());
                                //tmpExprBig.list.Add(tmpExpr);
                                parent.list.Add(tmpExpr);
                                //tmpExpr = new expr();
                            }
                            tmpExpr = new expr();
                            bool tmb = addExpr(tmpExpr, ref nc, ref no, ref poz, input);
                            parent.list.Add(tmpExpr);
                            if (!tmb)
                            {
                                //run = false;
                            }
                            //run = tmb;
                            tmpExpr = new expr();
                            //parent.list.Add(tmpExpr);
                            //tmpExpr = new expr();
                            tmpCharList = new List<char>();
                            break;
                        case '|':
                            poz++;
                            if (tmpCharList.Count > 0)
                            {
                                tmpExpr.reg = new string(tmpCharList.ToArray());
                                parent.list.Add(tmpExpr);
                            }
                            //tmpExpr = new expr();
                            bool tmb2 = addExpr(parent, ref nc, ref no, ref poz, input);
                            return false;

                            //parent.list.Add(tmpExpr);
                            //run = tmb2;

                            tmpExpr = new expr();
                            tmpCharList = new List<char>();
                            break;
                        case ')':
                            poz++;
                            nc++;
                            if (tmpCharList.Count > 0)
                            {
                                int a = 12;
                                tmpExpr.reg = new string(tmpCharList.ToArray());
                                parent.list.Add(tmpExpr);
                            }
                            return true;
                            run = false;
                            break;
                        case '^':
                            poz++;
                            break;
                        case '$':
                            if (tmpCharList.Count > 0)
                            {
                                tmpExpr.reg = new string(tmpCharList.ToArray());
                                parent.list.Add(tmpExpr);
                            }

                            run = false;
                            break;
                        default:
                            poz++;
                            break;
                    }
                }
            }

            return true;
        }

        public static int findEnd(int poz, int o, string input)//przerobiæ aby zlicza³ ( i )
        {
            int c = 0;
            while (true)
            {
                if (input[poz] == '$')
                {
                    return poz;
                }
                if (input[poz] == ')')
                {
                    c++;
                }
                if (input[poz] == '(')
                {
                    o++;
                }

                if (o == c)
                {
                    return poz;
                }
                else
                {
                    poz++;
                }
            }
        }

        public static int getDistance(List<char> str)
        {
            int n = str.Count(c => c == 'N');
            int e = str.Count(c => c == 'E');
            int s = str.Count(c => c == 'S');
            int w = str.Count(c => c == 'W');
            return Math.Abs(n - s) + Math.Abs(e - w);
        }

        public static Tuple<int, int> addExpres(ref int poz, int end, string input)
        {
            var tmpStr = new List<List<char>>();
            var tmpInt = new List<List<char>>();
            Tuple<int, int> distanceDoors = null;
            bool run = true;
            var tmpCharList = new List<char>();
            while (run)
            {
                if (end >= poz)
                {
                    char c = input[poz];
                    switch (c)
                    {
                        case 'N':
                        case 'E':
                        case 'W':
                        case 'S':
                            poz++;
                            tmpCharList.Add(c);
                            break;
                        case '(':
                            poz++;
                            var distanceDoorsTmp4 = new Tuple<int, int>(getDistance(tmpCharList), tmpCharList.Count);
                            if (distanceDoors != null)
                            {
                                distanceDoors = new Tuple<int, int>(distanceDoors.Item1 + distanceDoorsTmp4.Item1, distanceDoors.Item2 + distanceDoorsTmp4.Item2);
                            }
                            else
                            {
                                distanceDoors = distanceDoorsTmp4;
                            }
                            //if (tmpStr.Count > 0 && tmpCharList.Count > 0)
                            //{
                            //    for (var i = 0; i < tmpStr.Count; i++)
                            //    {
                            //        for (var j = 0; j < tmpCharList.Count; j++)
                            //        {
                            //            tmpStr[i].Add(tmpCharList[j]);
                            //        }
                            //    }
                            //}
                            //else if (tmpStr.Count == 0 && tmpCharList.Count >= 0)
                            //{
                            //    tmpStr.Add(tmpCharList);
                            //}
                            tmpCharList = new List<char>();
                            var tmp = addExpres(ref poz, findEnd(poz, 1, input), input);
                            //if (tmp.Item1 != 0&&distanceDoors.Item1!=0)
                            //{
                            //    if (distanceDoors.Item2 < tmp.Item2)
                            //    {
                            //        distanceDoors=new Tuple<int, int>(distanceDoors.Item1+tmp.Item1,distanceDoors.Item2+tmp.Item2);
                            //    }
                            //}
                            //else
                            //{
                            //    if (distanceDoors.Item2 > tmp.Item2)
                            //    {
                            //    }
                            //}
                            distanceDoors = new Tuple<int, int>(distanceDoors.Item1 + tmp.Item1, distanceDoors.Item2 + tmp.Item2);
                            //var tmpList2 = new List<List<char>>();
                            //for (var i = 0; i < tmpStr.Count; i++)
                            //{
                            //    for (var j = 0; j < tmp.Count; j++)
                            //    {
                            //        tmpList2.Add(tmpStr[i].Concat(tmp[j]).ToList());
                            //    }
                            //}
                            ////foreach (var s1 in tmpStr)
                            ////{
                            ////    foreach (var s2 in tmp)
                            ////    {
                            ////        tmpList2.Add(new string((s1 + s2).ToCharArray()));
                            ////    }
                            ////}
                            //tmpStr = tmpList2;
                            break;
                        case '|':
                            poz++;
                            var distanceDoorsTmp3 = new Tuple<int, int>(getDistance(tmpCharList), tmpCharList.Count);
                            if (distanceDoors != null)
                            {
                                distanceDoors = new Tuple<int, int>(distanceDoors.Item1 + distanceDoorsTmp3.Item1, distanceDoors.Item2 + distanceDoorsTmp3.Item2);
                            }
                            else
                            {
                                distanceDoors = distanceDoorsTmp3;
                            }
                            //if (tmpStr.Count > 0 && tmpCharList.Count > 0)
                            //{
                            //    for (var i = 0; i < tmpStr.Count; i++)
                            //    {
                            //        for (var j = 0; j < tmpCharList.Count; j++)
                            //        {
                            //            tmpStr[i].Add(tmpCharList[j]);
                            //        }
                            //    }
                            //}
                            //else if (tmpStr.Count == 0 && tmpCharList.Count >= 0)
                            //{
                            //    tmpStr.Add(tmpCharList);
                            //}
                            tmpCharList = new List<char>();
                            var next = addExpres(ref poz, findEnd(poz, 1, input), input);
                            if (next.Item1 != 0 && distanceDoors.Item1 != 0)
                            {
                                if (distanceDoors.Item2 < next.Item2)
                                {
                                    distanceDoors = next;
                                }
                            }
                            else
                            {
                                if ((next.Item1 == 0 && distanceDoors.Item1 == 0))
                                {
                                    if (distanceDoors.Item2 > next.Item2)
                                    {
                                        distanceDoors = next;
                                    }
                                }
                                else if ((next.Item1 == 0 && distanceDoors.Item1 != 0))
                                {

                                }
                                else if ((next.Item1 != 0 && distanceDoors.Item1 == 0))
                                {
                                    distanceDoors = next;
                                }
                            }
                            //var tmpList = new List<string>();
                            //int a = 12;
                            //foreach (var s2 in next)
                            //{
                            //    tmpStr.Add(s2);
                            //}
                            //tmpStr = tmpList;
                            break;
                        case ')':
                            poz++;
                            var distanceDoorsTmp = new Tuple<int, int>(getDistance(tmpCharList), tmpCharList.Count);
                            if (distanceDoors != null)
                            {
                                distanceDoors = new Tuple<int, int>(distanceDoors.Item1 + distanceDoorsTmp.Item1, distanceDoors.Item2 + distanceDoorsTmp.Item2);
                            }
                            else
                            {
                                distanceDoors = distanceDoorsTmp;
                            }
                            //if (tmpStr.Count > 0 && tmpCharList.Count > 0)
                            //{
                            //    for (var i = 0; i < tmpStr.Count; i++)
                            //    {
                            //        for (var j = 0; j < tmpCharList.Count; j++)
                            //        {
                            //            tmpStr[i].Add(tmpCharList[j]);
                            //        }
                            //    }
                            //}
                            //else if (tmpStr.Count == 0 && tmpCharList.Count >= 0)
                            //{
                            //    tmpStr.Add(tmpCharList);
                            //}
                            tmpCharList = new List<char>();
                            run = false;
                            break;
                        case '^':
                            poz++;
                            break;
                        case '$':
                            poz++;
                            var distanceDoorsTmp2 = new Tuple<int, int>(getDistance(tmpCharList), tmpCharList.Count);
                            if (distanceDoors != null)
                            {
                                distanceDoors = new Tuple<int, int>(distanceDoors.Item1 + distanceDoorsTmp2.Item1, distanceDoors.Item2 + distanceDoorsTmp2.Item2);
                            }
                            else
                            {
                                distanceDoors = distanceDoorsTmp2;
                            }                            //if (tmpStr.Count > 0 && tmpCharList.Count > 0)
                            //{
                            //    for (var i = 0; i < tmpStr.Count; i++)
                            //    {
                            //        for (var j = 0; j < tmpCharList.Count; j++)
                            //        {
                            //            tmpStr[i].Add(tmpCharList[j]);
                            //        }
                            //    }
                            //}
                            //else if (tmpStr.Count == 0 && tmpCharList.Count >= 0)
                            //{
                            //    tmpStr.Add(tmpCharList);
                            //}
                            tmpCharList = new List<char>();
                            run = false;
                            break;
                        default:
                            poz++;
                            break;
                    }
                }
                else
                {
                    break;
                }
            }
            //if (tmpStr.Count > 0 && tmpCharList.Count > 0)
            //{
            //    for (var i = 0; i < tmpStr.Count; i++)
            //    {
            //        for (var j = 0; j < tmpCharList.Count; j++)
            //        {
            //            tmpStr[i].Add(tmpCharList[j]);
            //        }
            //    }
            //}
            //else
            if (distanceDoors == null)
            {
                distanceDoors = new Tuple<int, int>(getDistance(tmpCharList), tmpCharList.Count);
            }
            return distanceDoors;
        }

        public static List<string> addStr(expr reg)
        {
            //var tmpList = new List<string>();
            //if (reg.reg != null)
            //{
            //    if (prev.Count > 0)
            //    {
            //        for (int i = 0; i < prev.Count; i++)
            //        {
            //            prev[i] = new string((prev[i] + reg.reg).ToCharArray());
            //        }
            //    }
            //    else
            //    {
            //        prev.Add(new string((reg.reg).ToCharArray()));
            //    }
            //}
            var tst = new List<List<string>>();
            foreach (var expr in reg.list)
            {
                var test = new List<string>();
                regStringList(ref test, expr);
                tst.Add(test);
            }
            var tst2 = new List<string>();
            for (int i = 0; i < tst.Count; i++)
            {
                if (tst2.Count > 0)
                {

                    var tmpList2 = new List<string>();
                    foreach (var t in tst2)
                    {
                        foreach (var r in tst[i])
                        {
                            tmpList2.Add(new string((t + r).ToCharArray()));
                        }
                    }
                    tst2 = tmpList2;
                }
                if (tst2.Count == 0)
                {
                    tst2 = tst[i];
                }
            }

            return tst2;
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

            var input = File.ReadAllText("input");
            int poz = 0;
            //var tmpExpr = new expr();
            // zamieniæ ( na (( ) na )) i | na )|(
            //int no = 0;
            //int nc = 0;
            //addExpr(tmpExpr, ref nc, ref no, ref poz, input);
            poz = 0;
            var t = addExpres(ref poz, findEnd(poz, 1, input), input);
            //List<string> prev = new List<string>();
            //regStringList(ref prev, tmpExpr);
            //List<string> prev2 = new List<string>();
            //var tmpExpr2 = new expr();
            //tmpExpr2.list.Add(tmpExpr.list[1]);
            //var tst = addStr(tmpExpr);
            //regStringList(ref prev2, tmpExpr.list[1]);
            var dic = new Dictionary<Tuple<int, int>, int>();
            Console.WriteLine(t.Item2);
            //prev = t;
            //foreach (var str in t)
            //{
            //    int n = str.Count(c => c == 'N');
            //    int e = str.Count(c => c == 'E');
            //    int s = str.Count(c => c == 'S');
            //    int w = str.Count(c => c == 'W');
            //    if (dic.TryGetValue(new Tuple<int, int>(n - s, e - w), out var min))
            //    {
            //        if (min > str.Count)
            //        {
            //            dic[new Tuple<int, int>(n - s, e - w)] = str.Count;
            //        }
            //    }
            //    else
            //    {
            //        dic.Add(new Tuple<int, int>(n - s, e - w), str.Count);
            //    }
            //}

            //Console.WriteLine(dic.Values.Max());
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}