using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day2
{
    class Program
    {
        static void Main(string[] args)
        {
            int two = 0;
            int three = 0;

            var lines = File.ReadLines("input");
            foreach (var line in lines)
            {
                bool boolTwo = false;
                bool boolThree = false;
                string distinct = new string(line.Distinct().ToArray());
                foreach (var distinctChar in distinct)
                {
                    int tmp = line.Count(x => x == distinctChar);
                    switch (tmp)
                    {
                        case 2:
                            boolTwo = true;
                            break;
                        case 3:
                            boolThree = true;
                            break;
                        default:
                            break;
                    }
                }
                if (boolTwo)
                {
                    two++;
                }
                if (boolThree)
                {
                    three++;
                }
            }

            System.Console.WriteLine((two * three).ToString());
        }
    }
}
