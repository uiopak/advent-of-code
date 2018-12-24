using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace Day23
{
    public class Program
    {

        static void ArrayExample1(Context ctx)
        {
            Console.WriteLine("ArrayExample1");

            Goal g = ctx.MkGoal(true);
            ArraySort asort = ctx.MkArraySort(ctx.IntSort, ctx.MkBitVecSort(32));
            ArrayExpr aex = (ArrayExpr)ctx.MkConst(ctx.MkSymbol("MyArray"), asort);
            Expr sel = ctx.MkSelect(aex, ctx.MkInt(0));
            g.Assert(ctx.MkEq(sel, ctx.MkBV(42, 32)));
            Symbol xs = ctx.MkSymbol("x");
            IntExpr xc = (IntExpr)ctx.MkConst(xs, ctx.IntSort);

            Symbol fname = ctx.MkSymbol("f");
            Sort[] domain = { ctx.IntSort };
            FuncDecl fd = ctx.MkFuncDecl(fname, domain, ctx.IntSort);
            Expr[] fargs = { ctx.MkConst(xs, ctx.IntSort) };
            IntExpr fapp = (IntExpr)ctx.MkApp(fd, fargs);

            g.Assert(ctx.MkEq(ctx.MkAdd(xc, fapp), ctx.MkInt(123)));

            Solver s = ctx.MkSolver();
            foreach (BoolExpr a in g.Formulas)
                s.Assert(a);
            Console.WriteLine("Solver: " + s);

            Status q = s.Check();
            Console.WriteLine("Status: " + q);


            Console.WriteLine("Model = " + s.Model);

            //Console.WriteLine("Interpretation of MyArray:\n" + s.Model.ConstInterp(aex.FuncDecl));
            Console.WriteLine("Interpretation of x:\n" + s.Model.ConstInterp(xc));
            Console.WriteLine("Interpretation of f:\n" + s.Model.FuncInterp(fd));
            //Console.WriteLine("Interpretation of MyArray as Term:\n" + s.Model.ConstInterp(aex.FuncDecl));
        }
        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllLines("input");
            var list = new List<Tuple<int, int, int, int>>();
            for (var i = 0; i < input.Length; i++)
            {
                var numbers1 = Regex.Matches(input[i], @"-?\d+");
                list.Add(new Tuple<int, int, int, int>(int.Parse(numbers1[0].Value), int.Parse(numbers1[1].Value), int.Parse(numbers1[2].Value), int.Parse(numbers1[3].Value)));
            }
            //zad 1
            //var tmp = list.OrderBy(c => c.Item4);
            //var max = tmp.Last();
            //int cnt = 0;
            //foreach (var tuple in list)
            //{
            //    if (Math.Abs((int)(max.Item1 - tuple.Item1)) + Math.Abs((int)(max.Item2 - tuple.Item2)) + Math.Abs((int)(max.Item3 - tuple.Item3)) <= (int)max.Item4)
            //    {
            //        cnt++;
            //    }
            //}

            //var inRangeList = new List<List<int>>();
            //foreach (var tuple in list)
            //{
            //    var tmpList = new List<int>();
            //    for (int i = 0; i < list.Count; i++)
            //    {
            //        if (Math.Abs((int)(list[i].Item1 - tuple.Item1)) + Math.Abs((int)(list[i].Item2 - tuple.Item2)) + Math.Abs((int)(list[i].Item3 - tuple.Item3)) <= list[i].Item4 + tuple.Item4)
            //        {
            //            tmpList.Add(i);
            //        }
            //    }
            //    inRangeList.Add(tmpList);
            //}

            //int? min1 = null;
            //int? max1 = null;
            //int? min2 = null;
            //int? max2 = null;
            //int? min3 = null;
            //int? max3 = null;
            //foreach (var tuple in list)
            //{
            //    if (min1 == null || min1 > tuple.Item1 - tuple.Item4)
            //    {
            //        min1 = tuple.Item1 - tuple.Item4;
            //    }
            //    if (max1 == null || max1 < tuple.Item1 + tuple.Item4)
            //    {
            //        max1 = tuple.Item2 + tuple.Item4;
            //    }
            //    if (min2 == null || min2 > tuple.Item2 - tuple.Item4)
            //    {
            //        min2 = tuple.Item2 - tuple.Item4;
            //    }
            //    if (max2 == null || max2 < tuple.Item2 + tuple.Item4)
            //    {
            //        max2 = tuple.Item2 + tuple.Item4;
            //    }
            //    if (min3 == null || min3 > tuple.Item3 - tuple.Item4)
            //    {
            //        min3 = tuple.Item3 - tuple.Item4;
            //    }
            //    if (max3 == null || max3 < tuple.Item3 + tuple.Item4)
            //    {
            //        max3 = tuple.Item3 + tuple.Item4;
            //    }
            //}

            //int cnt2 = 0;
            //for (var i = min1; i < max1; i++)
            //{
            //    for (var j = min2; j < max2; j++)
            //    {
            //        for (var k = min3; k < max3; k++)
            //        {
            //            cnt2++;
            //        }
            //    }
            //}

            //var IntersectList = new List<List<int>>();
            //foreach (var v1 in inRangeList)
            //{
            //    var tmpL = new List<int>();
            //    foreach (var v2 in v1)
            //    {
            //        tmpL.Add(v1.Intersect(inRangeList[v2]).Count());
            //    }
            //    IntersectList.Add(tmpL);
            //}

            Console.WriteLine("SimpleExample");

            using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            {
                //var tester = new Solver(ctx,);
                /* do something with the context */

                /* be kind to dispose manually and not wait for the GC. */
                var o = ctx.MkOptimize();
                var x = ctx.MkIntConst("x");
                var y = ctx.MkIntConst("y");
                var z = ctx.MkIntConst("z");
                var range_count = ctx.MkIntConst("sum");
                //var ra = ctx.int
                Sort int_type = ctx.MkIntSort();
                Sort array_type = ctx.MkArraySort(int_type, int_type);
                IntExpr[] a = new IntExpr[list.Count];

                /* create arrays */
                for (int i = 0; i < list.Count; i++)
                {
                    a[i] = (IntExpr)ctx.MkConst(String.Format("in_range_{0}", i), ctx.MkIntSort());
                }
                for (int i = 0; i < list.Count; i++)
                {
                    var tmp1 = ctx.MkSub(x, ctx.MkInt(list[i].Item1));
                    var abs1 = ctx.MkITE(ctx.MkGe(tmp1, ctx.MkInt(0)), tmp1, -tmp1);
                    var tmp2 = ctx.MkSub(y, ctx.MkInt(list[i].Item2));
                    var abs2 = ctx.MkITE(ctx.MkGe(tmp2, ctx.MkInt(0)), tmp2, -tmp2);
                    var tmp3 = ctx.MkSub(z, ctx.MkInt(list[i].Item3));
                    var abs3 = ctx.MkITE(ctx.MkGe(tmp3, ctx.MkInt(0)), tmp3, -tmp3);
                    var tmp4 = ctx.MkInt(list[i].Item4);
                    var el1 = a[i];
                    var el2 = ctx.MkITE(ctx.MkLe(ctx.MkAdd((IntExpr)abs1, (IntExpr)abs2, (IntExpr)abs3), tmp4), ctx.MkInt(1), ctx.MkInt(0));
                    var el = ctx.MkEq(el1, el2);
                    o.Add(el);
                    //o.Add(Math.Abs((int)(list[i].Item1 - (int)x.)) + Math.Abs((int)(list[i].Item2 - tuple.Item2)) + Math.Abs((int)(list[i].Item3 - tuple.Item3)) <= list[i].Item4 + tuple.Item4);
                }

                var sum = ctx.MkAdd(a[0], a[1]);
                for (int i = 2; i < a.Length; i++)
                {
                    sum = ctx.MkAdd(sum, a[i]);
                }
                o.Add(ctx.MkEq(range_count, sum));
                var dist = ctx.MkIntConst("dist");
                var dX = (IntExpr)(ctx.MkITE(ctx.MkGt(x, ctx.MkInt(0)), x, -x));
                var dY = (IntExpr)(ctx.MkITE(ctx.MkGt(y, ctx.MkInt(0)), y, -y));
                var dZ = (IntExpr)(ctx.MkITE(ctx.MkGt(z, ctx.MkInt(0)), z, -z));
                o.Add(ctx.MkEq(dist, ctx.MkAdd(dX, dY, dZ)));
                var h1 = o.MkMaximize(range_count);
                var h2 = o.MkMinimize(dist);
                Console.WriteLine(o.Check());
                Console.WriteLine(o.Model);
                Console.WriteLine("Interpretation of distance:\n" + o.Model.ConstInterp(dist));
                //Solver s = ctx.MkSolver();
                ctx.Dispose();
            }
            //using (Context ctx = new Context(new Dictionary<string, string>() { { "model", "true" } }))
            //{
            //    ArrayExample1(ctx);
            //}

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}