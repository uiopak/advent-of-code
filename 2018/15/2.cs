using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Day13
{
    public class Program
    {
        public enum Direct
        {
            up,
            right,
            left,
            down
        }

        public static int elfPower = 4;
        public class Character
        {
            public char character;
            public char enemy;
            public int x;
            public int y;
            public int hitPoints;
            public int attackPower;
            public List<char> colors;
            public Character(char c, int x, int y)
            {
                character = c;
                this.x = x;
                this.y = y;
                hitPoints = 200;
                if (character == 'E')
                {
                    attackPower = elfPower;
                }
                else
                {
                    attackPower = 3;
                }
                colors = new List<char>();
                enemy = character == 'E' ? 'G' : 'E';
            }
        }

        public static void FillColor(char color, List<char[]> map, int x, int y)
        {
            if (map[y][x] == '.')
            {
                map[y][x] = color;
            }
            if (x > 0 && map[y][x - 1] == '.')
            {
                FillColor(color, map, x - 1, y);
            }
            if (x + 1 < map[y].Length && map[y][x + 1] == '.')
            {
                FillColor(color, map, x + 1, y);
            }
            if (y > 0 && map[y - 1][x] == '.')
            {
                FillColor(color, map, x, y - 1);
            }
            if (y + 1 < map.Count && map[y + 1][x] == '.')
            {
                FillColor(color, map, x, y + 1);
            }
        }

        public static void addToDictionary(int x, int y, int distance, Dictionary<Tuple<int, int>, int> tmpDictionary, List<char[]> map, List<char[]> test)
        {
            if (!tmpDictionary.ContainsKey(new Tuple<int, int>(x - 1, y)))
            {
                if (x - 1 > 0 && map[y][x - 1] != '#' && map[y][x - 1] != 'E' && map[y][x - 1] != 'G')
                {
                    tmpDictionary.Add(new Tuple<int, int>(x - 1, y), distance);
                    //test[y][x - 1] = ' ';
                }
            }
            else if (tmpDictionary[new Tuple<int, int>(x - 1, y)] > distance)
            {
                tmpDictionary[new Tuple<int, int>(x - 1, y)] = distance;
            }
            if (!tmpDictionary.ContainsKey(new Tuple<int, int>(x + 1, y)))
            {
                if (x + 1 < map[y].Length && map[y][x + 1] != '#' && map[y][x + 1] != 'E' && map[y][x + 1] != 'G')
                {
                    tmpDictionary.Add(new Tuple<int, int>(x + 1, y), distance);
                    //test[y][x + 1] = ' ';
                }
            }
            else if (tmpDictionary[new Tuple<int, int>(x + 1, y)] > distance)
            {
                tmpDictionary[new Tuple<int, int>(x + 1, y)] = distance;
            }
            if (!tmpDictionary.ContainsKey(new Tuple<int, int>(x, y - 1)))
            {
                if (y - 1 > 0 && map[y - 1][x] != '#' && map[y - 1][x] != 'E' && map[y - 1][x] != 'G')
                {
                    tmpDictionary.Add(new Tuple<int, int>(x, y - 1), distance);
                    //test[y - 1][x] = ' ';
                }
            }
            else if (tmpDictionary[new Tuple<int, int>(x, y - 1)] > distance)
            {
                tmpDictionary[new Tuple<int, int>(x, y - 1)] = distance;
            }
            if (!tmpDictionary.ContainsKey(new Tuple<int, int>(x, y + 1)))
            {
                if (y + 1 < map.Count && map[y + 1][x] != '#' && map[y + 1][x] != 'E' && map[y + 1][x] != 'G')
                {
                    tmpDictionary.Add(new Tuple<int, int>(x, y + 1), distance);
                    //test[y + 1][x] = ' ';
                }
            }
            else if (tmpDictionary[new Tuple<int, int>(x, y + 1)] > distance)
            {
                tmpDictionary[new Tuple<int, int>(x, y + 1)] = distance;
            }
            //foreach (var VARIABLE in test)
            //{
            //    Console.WriteLine(VARIABLE);
            //}
        }

        public static void addToDictionaryOne(int x, int y, int distance, Dictionary<Tuple<int, int>, int> tmpDictionary, List<char[]> map, List<char[]> test)
        {
            if (!tmpDictionary.ContainsKey(new Tuple<int, int>(x, y)))
            {
                if (map[y][x] != '#' && map[y][x] != 'E' && map[y][x] != 'G')
                {
                    tmpDictionary.Add(new Tuple<int, int>(x, y), distance);
                    //test[y][x] = ' ';
                }
            }
            else if (tmpDictionary[new Tuple<int, int>(x, y)] > distance)
            {
                tmpDictionary[new Tuple<int, int>(x, y)] = distance;
            }
            //foreach (var VARIABLE in test)
            //{
            //    Console.WriteLine(VARIABLE);
            //}
        }

        public static bool checkForEnemy(int x, int y, char enemy, List<char[]> map)
        {
            if (x - 1 > 0)
            {
                if (map[y][x - 1] == enemy)
                {
                    return true;
                }
            }

            if (x + 1 < map[y].Length)
            {
                if (map[y][x + 1] == enemy)
                {
                    return true;
                }
            }

            if (y - 1 > 0)
            {
                if (map[y - 1][x] == enemy)
                {
                    return true;
                }
            }

            if (y + 1 < map.Count)
            {
                if (map[y + 1][x] == enemy)
                {
                    return true;
                }
            }

            return false;
        }

        public static Tuple<int, int> checkForExactEnemy(int x, int y, Character enemy, List<char[]> map)
        {
            if (y - 1 > 0)
            {
                if (y - 1 == enemy.y && x == enemy.x)
                {
                    return new Tuple<int, int>(x, y);
                }
            }

            if (x - 1 > 0)
            {
                if (y == enemy.y && x - 1 == enemy.x)
                {
                    return new Tuple<int, int>(x, y);
                }
            }

            if (x + 1 < map[y].Length)
            {
                if (y == enemy.y && x + 1 == enemy.x)
                {
                    return new Tuple<int, int>(x, y);
                }
            }

            if (y + 1 < map.Count)
            {
                if (y + 1 == enemy.y && x == enemy.x)
                {
                    return new Tuple<int, int>(x, y);
                }
            }

            return new Tuple<int, int>(-1, -1);
        }

        public static Tuple<int, Tuple<int, int>> getDistance(int startX, int startY, Character enemy, List<char[]> map, List<char[]> test)
        {

            int distance = 1;
            var tmpDictionary = new Dictionary<Tuple<int, int>, int>();
            addToDictionaryOne(startX, startY, distance, tmpDictionary, map, test);
            //foreach (var VARIABLE in test)
            //{
            //    Console.WriteLine(VARIABLE);
            //}
            foreach (var keyValuePair in tmpDictionary)
            {
                var res = checkForExactEnemy(keyValuePair.Key.Item1, keyValuePair.Key.Item2, enemy, map);
                if (!res.Equals(new Tuple<int, int>(-1, -1)))
                {
                    return new Tuple<int, Tuple<int, int>>(distance, res);
                }
            }
            while (true)
            {
                distance++;
                var things = tmpDictionary.Where(x => x.Value == distance - 1).Select(thing => thing.Key).ToList();
                foreach (var keyValuePair in things)
                {
                    var res = checkForExactEnemy(keyValuePair.Item1, keyValuePair.Item2, enemy, map);

                    if (!res.Equals(new Tuple<int, int>(-1, -1)))
                    {
                        return new Tuple<int, Tuple<int, int>>(distance - 1, res);
                    }
                    addToDictionary(keyValuePair.Item1, keyValuePair.Item2, distance, tmpDictionary, map, test);
                }
            }
        }

        public static void attack(int x, int y, Character player, List<char[]> map, Dictionary<Tuple<int, int>, Character> players)
        {
            var tmpList = new List<Tuple<int, int>>();
            if (map[y - 1][x] == player.enemy)
            {
                tmpList.Add(new Tuple<int, int>(x, y - 1));
            }
            if (map[y][x - 1] == player.enemy)
            {
                tmpList.Add(new Tuple<int, int>(x - 1, y));
            }
            if (map[y][x + 1] == player.enemy)
            {
                tmpList.Add(new Tuple<int, int>(x + 1, y));
            }
            if (map[y + 1][x] == player.enemy)
            {
                tmpList.Add(new Tuple<int, int>(x, y + 1));
            }

            var hitPoints = tmpList.OrderBy(o => players[new Tuple<int, int>(o.Item1, o.Item2)].hitPoints)
                .ThenBy(o => players[new Tuple<int, int>(o.Item1, o.Item2)].y)
                .ThenBy(o => players[new Tuple<int, int>(o.Item1, o.Item2)].x).ToList();
            //var minimum = players[new Tuple<int, int>(hitPoints.First().Item1, hitPoints.First().Item2)].hitPoints;
            //var minHitPointsNumber = hitPoints.Count(o => players[new Tuple<int, int>(o.Item1, o.Item2)].hitPoints == minimum);
            //int cnt = 0;
            //if (minHitPointsNumber > 1)
            //{
            //    //tmpList= tmpList.OrderBy(x => x.)
            //    //    .ThenBy(x => x.x).ToList();
            //    for (int i = 0; i < hitPoints.Count; i++)
            //    {
            //        if (players[new Tuple<int, int>(hitPoints[i].Item1, hitPoints[i].Item2)].hitPoints == minimum)
            //        {
            //            players[new Tuple<int, int>(hitPoints[i].Item1, hitPoints[i].Item2)].hitPoints -=
            //                player.attackPower;
            //            if (players[new Tuple<int, int>(hitPoints[i].Item1, hitPoints[i].Item2)].hitPoints <= 0)
            //            {
            //                map[players[new Tuple<int, int>(hitPoints.First().Item1, hitPoints.First().Item2)].y][players[new Tuple<int, int>(hitPoints.First().Item1, hitPoints.First().Item2)].x] = '.';
            //                players.Remove(new Tuple<int, int>(hitPoints[i].Item1, hitPoints[i].Item2));
            //            }
            //            return;
            //        }
            //    }
            //}
            //else
            //{
            players[new Tuple<int, int>(hitPoints.First().Item1, hitPoints.First().Item2)].hitPoints -=
                player.attackPower;
            if (players[new Tuple<int, int>(hitPoints.First().Item1, hitPoints.First().Item2)].hitPoints <= 0)
            {
                map[players[new Tuple<int, int>(hitPoints.First().Item1, hitPoints.First().Item2)].y]
                    [players[new Tuple<int, int>(hitPoints.First().Item1, hitPoints.First().Item2)].x] = '.';
                players.Remove(new Tuple<int, int>(hitPoints.First().Item1, hitPoints.First().Item2));
            }
            return;
            //}

            int a = 12;
        }

        public static void clearLines(ref List<char[]> playerLines, Dictionary<Tuple<int, int>, Character> players,
            List<string> cleanList)
        {
            var linesClean = new List<char[]>();
            foreach (var line in cleanList)
            {
                linesClean.Add(line.ToCharArray());
            }
            playerLines = linesClean;
        }

        public static void createLines(ref List<char[]> playerLines, Dictionary<Tuple<int, int>, Character> players,
            List<string> cleanList)
        {
            clearLines(ref playerLines, players, cleanList);
            char color = 'a';
            int colorCnt = 0;
            int y = 0;
            foreach (var player in players)
            {
                playerLines[player.Value.y][player.Value.x] = player.Value.character;
            }
            //foreach (var VARIABLE in playerLines)
            //{
            //    Console.WriteLine(VARIABLE);
            //}
            foreach (var line in playerLines)
            {
                int x = 0;
                foreach (var testChar in line)
                {
                    if (testChar == '.')
                    {
                        FillColor((char)(color + colorCnt), playerLines, x, y);
                        colorCnt++;
                    }
                    x++;
                }
                y++;
            }
        }

        public static void countColors(List<char[]> playerLines, Dictionary<Tuple<int, int>, Character> players)
        {
            foreach (var player in players)
            {
                player.Value.colors = new List<char>();
                if (player.Value.x > 0)
                {
                    char tmp = playerLines[player.Value.y][player.Value.x - 1];
                    if (tmp != '#' && tmp != 'E' && tmp != 'G')
                    {
                        if (!player.Value.colors.Contains(tmp))
                        {
                            player.Value.colors.Add(tmp);
                        }
                    }
                }
                if (player.Value.x + 1 < playerLines[0].Length)
                {
                    char tmp = playerLines[player.Value.y][player.Value.x + 1];
                    if (tmp != '#' && tmp != 'E' && tmp != 'G')
                    {
                        if (!player.Value.colors.Contains(tmp))
                        {
                            player.Value.colors.Add(tmp);
                        }
                    }
                }
                if (player.Value.y > 0)
                {
                    char tmp = playerLines[player.Value.y - 1][player.Value.x];
                    if (tmp != '#' && tmp != 'E' && tmp != 'G')
                    {
                        if (!player.Value.colors.Contains(tmp))
                        {
                            player.Value.colors.Add(tmp);
                        }
                    }
                }
                if (player.Value.y + 1 < playerLines.Count)
                {
                    char tmp = playerLines[player.Value.y + 1][player.Value.x];
                    if (tmp != '#' && tmp != 'E' && tmp != 'G')
                    {
                        if (!player.Value.colors.Contains(tmp))
                        {
                            player.Value.colors.Add(tmp);
                        }
                    }
                }
            }
        }


        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            elfPower = 19;
            var input = File.ReadAllLines("input");
            //var input = File.ReadAllLines("prob.txt");
            //var input = File.ReadAllLines("test5.txt");

            while (true)
            {
                int elfsCnt = 0;
                var linesClean = new List<char[]>();
                var linesWithPlayers = new List<char[]>();
                var players = new Dictionary<Tuple<int, int>, Character>();
                int tmpY = 0;
                int tmpX = 0;
                foreach (var line in input)
                {
                    var tmpStringClean = new char[line.Length];
                    var tmpStringWithPlayers = new char[line.Length];
                    tmpX = 0;
                    foreach (var character in line)
                    {
                        switch (character)
                        {
                            case 'E':
                            case 'G':
                                tmpStringClean[tmpX] = '.';
                                if (character == 'E')
                                {
                                    elfsCnt++;
                                }
                                //tmpStringClean += '.';
                                players.Add(new Tuple<int, int>(tmpX, tmpY), new Character(character, tmpX, tmpY));
                                break;
                            default:
                                tmpStringClean[tmpX] = character;
                                //tmpStringClean += character;
                                break;
                        }
                        tmpStringWithPlayers[tmpX] += character;
                        //tmpStringWithPlayers += character;
                        tmpX++;
                    }

                    linesClean.Add(tmpStringClean);
                    linesWithPlayers.Add(tmpStringWithPlayers);
                    tmpY++;
                }

                //foreach (var VARIABLE in linesClean)
                //{
                //    Console.WriteLine(VARIABLE);
                //}
                //Console.WriteLine("Initially:");
                //foreach (var VARIABLE in linesWithPlayers)
                //{
                //    Console.WriteLine(VARIABLE);
                //}
                // przechodzimy po ka¿dej '.' i oznaczamy kolorem a,b,c itd.
                // potem ka¿da postaæ zaznacza obok czego jest
                // mo¿na sprawdziæ czy jest mo¿liwoœæ ruchu
                // ka¿dy, który ma mo¿liwoœæ ruchu robi sobie mapê odleg³oœci
                // jak ma kilka w tej samej odleg³oœci, to wybiera zgodnie z zasadami 
                // czyli bli¿ej górnego lewego rogu
                // zaznacz równie¿ gdzie s¹ przeciwnicy do których mo¿e dojœæ
                // jak wybierze gdzie idzie to musi jeszcze wygraæ jak, zgodnie z zasadami
                int round = 1;
                var cleanListCopy = new List<string>();
                foreach (var line in linesClean)
                {
                    cleanListCopy.Add(new string(line));
                }
                while (true)
                {
                    //Console.WriteLine(round);
                    //char color = 'a';
                    //int colorCnt = 0;
                    //int y = 0;
                    //foreach (var player in players)
                    //{
                    //    linesWithPlayers[player.Value.y][player.Value.x] = player.Value.character;
                    //}
                    //foreach (var VARIABLE in linesWithPlayers)
                    //{
                    //    Console.WriteLine(VARIABLE);
                    //}
                    //foreach (var line in linesWithPlayers)
                    //{
                    //    int x = 0;
                    //    foreach (var testChar in line)
                    //    {
                    //        if (testChar == '.')
                    //        {
                    //            FillColor((char)(color + colorCnt), linesWithPlayers, x, y);
                    //            colorCnt++;
                    //        }

                    //        x++;
                    //    }

                    //    y++;
                    //}

                    createLines(ref linesWithPlayers, players, cleanListCopy);



                    int a2 = 12;
                    //foreach (var player in players)
                    //{
                    //    player.Value.colors = new List<char>();
                    //    if (player.Value.x > 0)
                    //    {
                    //        char tmp = linesWithPlayers[player.Value.y][player.Value.x - 1];
                    //        if (tmp != '#' && tmp != 'E' && tmp != 'G')
                    //        {
                    //            if (!player.Value.colors.Contains(tmp))
                    //            {
                    //                player.Value.colors.Add(tmp);
                    //            }
                    //        }
                    //    }
                    //    if (player.Value.x + 1 < linesWithPlayers[0].Length)
                    //    {
                    //        char tmp = linesWithPlayers[player.Value.y][player.Value.x + 1];
                    //        if (tmp != '#' && tmp != 'E' && tmp != 'G')
                    //        {
                    //            if (!player.Value.colors.Contains(tmp))
                    //            {
                    //                player.Value.colors.Add(tmp);
                    //            }
                    //        }
                    //    }
                    //    if (player.Value.y > 0)
                    //    {
                    //        char tmp = linesWithPlayers[player.Value.y - 1][player.Value.x];
                    //        if (tmp != '#' && tmp != 'E' && tmp != 'G')
                    //        {
                    //            if (!player.Value.colors.Contains(tmp))
                    //            {
                    //                player.Value.colors.Add(tmp);
                    //            }
                    //        }
                    //    }
                    //    if (player.Value.y + 1 < linesWithPlayers.Count)
                    //    {
                    //        char tmp = linesWithPlayers[player.Value.y + 1][player.Value.x];
                    //        if (tmp != '#' && tmp != 'E' && tmp != 'G')
                    //        {
                    //            if (!player.Value.colors.Contains(tmp))
                    //            {
                    //                player.Value.colors.Add(tmp);
                    //            }
                    //        }
                    //    }
                    //}

                    countColors(linesWithPlayers, players);
                    //foreach (var VARIABLE in linesWithPlayers)
                    //{
                    //    Console.WriteLine(VARIABLE);
                    //}
                    //Console.WriteLine();
                    var things = players.Select(thing => thing.Value)
                        .OrderBy(x => x.y)
                        .ThenBy(x => x.x).ToList();
                    foreach (var player in things)
                    {
                        createLines(ref linesWithPlayers, players, cleanListCopy);
                        countColors(linesWithPlayers, players);
                        if (players.ContainsKey(new Tuple<int, int>(player.x, player.y)))
                        {
                            if (round == 24)
                            {
                                int t = 12;
                            }
                            var tmpAccessibleEnemyList = new List<Character>();
                            foreach (var enemy in players)
                            {
                                if (enemy.Value.character == player.enemy && player.colors.Any(i => enemy.Value.colors.Contains(i)))
                                {
                                    tmpAccessibleEnemyList.Add(enemy.Value);
                                }
                            }

                            if (checkForEnemy(player.x, player.y, player.enemy, linesWithPlayers))
                            {
                                attack(player.x, player.y, player, linesWithPlayers, players);
                                createLines(ref linesWithPlayers, players, cleanListCopy);
                                countColors(linesWithPlayers, players);
                            }
                            else if (tmpAccessibleEnemyList.Count > 0)
                            {
                                //foreach (var enemy in tmpAccessibleEnemyList)
                                //tu b³¹d zawsze idzie w stronê najwy¿szego
                                //var enemy = tmpAccessibleEnemyList.OrderBy(orderY => orderY.y).ThenBy(orderX => orderX.x).First();
                                if (player.character == 'E' && round == 5)
                                {
                                    var tre = 12;
                                }
                                Direct direct;

                                List<Tuple<int, Tuple<int, int>, Direct>> dir = new List<Tuple<int, Tuple<int, int>, Direct>>();
                                int upI = Int32.MaxValue;
                                Character upEnemy;
                                List<Character> upEnemies;
                                if (player.y > 0)
                                {
                                    char tmp = linesWithPlayers[player.y - 1][player.x];
                                    if (tmp == '.')
                                    {
                                        int r = 12;
                                    }
                                    var test = new List<char[]>(linesWithPlayers);
                                    upEnemies = tmpAccessibleEnemyList.Where(e => e.colors.Contains(tmp)).OrderBy(e => getDistance(player.x, player.y - 1, e, linesWithPlayers, test).Item1)
                                        .ThenBy(e => getDistance(player.x, player.y - 1, e, linesWithPlayers, test).Item2.Item2).ThenBy(e => getDistance(player.x, player.y - 1, e, linesWithPlayers, test).Item2.Item1).ToList();
                                    upEnemy = upEnemies.FirstOrDefault();
                                    if (upEnemy != null && (tmp != '#' && tmp != 'E' && tmp != 'G' && upEnemy.colors.Contains(tmp)))
                                    {
                                        var up = getDistance(player.x, player.y - 1, upEnemy, linesWithPlayers, test);
                                        upI = up.Item1;
                                        dir.Add(new Tuple<int, Tuple<int, int>, Direct>(up.Item1, up.Item2, Direct.up));
                                        //createLines(linesWithPlayers, players, cleanListCopy);
                                        //countColors(linesWithPlayers, players);
                                    }
                                }
                                int leftI = Int32.MaxValue;
                                Character leftEnemy;
                                List<Character> leftEnemies;
                                if (player.x > 0)
                                {
                                    char tmp = linesWithPlayers[player.y][player.x - 1];
                                    if (tmp == '.')
                                    {
                                        int r = 12;
                                    }
                                    var test = new List<char[]>(linesWithPlayers);
                                    leftEnemies = tmpAccessibleEnemyList.Where(e => e.colors.Contains(tmp)).OrderBy(e => getDistance(player.x - 1, player.y, e, linesWithPlayers, test).Item1)
                                        .ThenBy(e => getDistance(player.x - 1, player.y, e, linesWithPlayers, test).Item2.Item2).ThenBy(e => getDistance(player.x - 1, player.y, e, linesWithPlayers, test).Item2.Item1).ToList();
                                    leftEnemy = leftEnemies.FirstOrDefault();
                                    if (leftEnemy != null && (tmp != '#' && tmp != 'E' && tmp != 'G' && leftEnemy.colors.Contains(tmp)))
                                    {
                                        var left = getDistance(player.x - 1, player.y, leftEnemy, linesWithPlayers, test);
                                        leftI = left.Item1;
                                        dir.Add(new Tuple<int, Tuple<int, int>, Direct>(left.Item1, left.Item2, Direct.left));
                                        //createLines(linesWithPlayers, players, cleanListCopy);
                                        //countColors(linesWithPlayers, players);
                                    }
                                }
                                int rightI = Int32.MaxValue;
                                Character rightEnemy;
                                List<Character> rightEnemies;

                                if (player.x + 1 < linesWithPlayers[0].Length)
                                {
                                    char tmp = linesWithPlayers[player.y][player.x + 1];
                                    if (tmp == '.')
                                    {
                                        int r = 12;
                                    }
                                    var test = new List<char[]>(linesWithPlayers);
                                    rightEnemies = tmpAccessibleEnemyList.Where(e => e.colors.Contains(tmp)).OrderBy(e => getDistance(player.x + 1, player.y, e, linesWithPlayers, test).Item1)
                                        .ThenBy(e => getDistance(player.x + 1, player.y, e, linesWithPlayers, test).Item2.Item2).ThenBy(e => getDistance(player.x + 1, player.y, e, linesWithPlayers, test).Item2.Item1).ToList();
                                    rightEnemy = rightEnemies.FirstOrDefault();
                                    if (rightEnemy != null && (tmp != '#' && tmp != 'E' && tmp != 'G' && rightEnemy.colors.Contains(tmp)))
                                    {
                                        var right = getDistance(player.x + 1, player.y, rightEnemy, linesWithPlayers, test);
                                        rightI = right.Item1;
                                        dir.Add(new Tuple<int, Tuple<int, int>, Direct>(right.Item1, right.Item2, Direct.right));
                                        //createLines(linesWithPlayers, players, cleanListCopy);
                                        //countColors(linesWithPlayers, players);
                                    }
                                }

                                int downI = Int32.MaxValue;
                                Character downEnemy;
                                List<Character> downEnemies;
                                if (player.y + 1 < linesWithPlayers.Count)
                                {
                                    char tmp = linesWithPlayers[player.y + 1][player.x];
                                    if (tmp == '.')
                                    {
                                        int r = 12;
                                    }
                                    var test = new List<char[]>(linesWithPlayers);
                                    downEnemies = tmpAccessibleEnemyList.Where(e => e.colors.Contains(tmp)).OrderBy(e => getDistance(player.x, player.y + 1, e, linesWithPlayers, test).Item1)
                                        .ThenBy(e => getDistance(player.x, player.y + 1, e, linesWithPlayers, test).Item2.Item2).ThenBy(e => getDistance(player.x, player.y + 1, e, linesWithPlayers, test).Item2.Item1).ToList();
                                    downEnemy = downEnemies.FirstOrDefault();
                                    if (downEnemy != null && (tmp != '#' && tmp != 'E' && tmp != 'G' && downEnemy.colors.Contains(tmp)))
                                    {
                                        var down = getDistance(player.x, player.y + 1, downEnemy, linesWithPlayers, test);
                                        downI = down.Item1;
                                        dir.Add(new Tuple<int, Tuple<int, int>, Direct>(down.Item1, down.Item2, Direct.down));
                                        //createLines(linesWithPlayers, players, cleanListCopy);
                                        //countColors(linesWithPlayers, players);
                                    }
                                }

                                if (upI != Int32.MaxValue || downI != Int32.MaxValue || leftI != Int32.MaxValue || rightI != Int32.MaxValue)
                                {
                                    var dirToGo = dir.OrderBy(t => t.Item1).ThenBy(t => t.Item2.Item2).ThenBy(t => t.Item2.Item1)
                                        .First();

                                    players.Remove(new Tuple<int, int>(player.x, player.y));
                                    if (dirToGo.Item3 == Direct.up)
                                    {
                                        //move up
                                        linesWithPlayers[player.y][player.x] = '.';
                                        linesWithPlayers[player.y - 1][player.x] = player.character;
                                        player.y = player.y - 1;
                                        players.Add(new Tuple<int, int>(player.x, player.y), player);
                                        int a = 12;
                                    }
                                    else if (dirToGo.Item3 == Direct.left)
                                    {
                                        //move left
                                        linesWithPlayers[player.y][player.x] = '.';
                                        linesWithPlayers[player.y][player.x - 1] = player.character;
                                        player.x = player.x - 1;
                                        players.Add(new Tuple<int, int>(player.x, player.y), player);
                                        int a = 12;
                                    }
                                    else if (dirToGo.Item3 == Direct.right)
                                    {
                                        //move right
                                        linesWithPlayers[player.y][player.x] = '.';
                                        linesWithPlayers[player.y][player.x + 1] = player.character;
                                        player.x = player.x + 1;
                                        players.Add(new Tuple<int, int>(player.x, player.y), player);
                                        int a = 12;
                                    }
                                    else
                                    {
                                        //move down
                                        linesWithPlayers[player.y][player.x] = '.';
                                        linesWithPlayers[player.y + 1][player.x] = player.character;
                                        player.y = player.y + 1;
                                        players.Add(new Tuple<int, int>(player.x, player.y), player);
                                        int a = 12;
                                    }

                                    //if (up > 0 && up <= right && up <= left && up <= down)
                                    //{
                                    //    //move up
                                    //    linesWithPlayers[player.y][player.x] = '.';
                                    //    linesWithPlayers[player.y - 1][player.x] = player.character;
                                    //    player.y = player.y - 1;
                                    //    players.Add(new Tuple<int, int>(player.x, player.y), player);
                                    //    int a = 12;
                                    //}
                                    //else if (left > 0 && left < up && left <= right && left <= down)
                                    //{
                                    //    //move left
                                    //    linesWithPlayers[player.y][player.x] = '.';
                                    //    linesWithPlayers[player.y][player.x - 1] = player.character;
                                    //    player.x = player.x - 1;
                                    //    players.Add(new Tuple<int, int>(player.x, player.y), player);
                                    //    int a = 12;
                                    //}
                                    //else if (right > 0 && right < up && right < left && right <= down)
                                    //{
                                    //    //move right
                                    //    linesWithPlayers[player.y][player.x] = '.';
                                    //    linesWithPlayers[player.y][player.x + 1] = player.character;
                                    //    player.x = player.x + 1;
                                    //    players.Add(new Tuple<int, int>(player.x, player.y), player);
                                    //    int a = 12;
                                    //}
                                    //else
                                    //{
                                    //    //move down
                                    //    linesWithPlayers[player.y][player.x] = '.';
                                    //    linesWithPlayers[player.y + 1][player.x] = player.character;
                                    //    player.y = player.y + 1;
                                    //    players.Add(new Tuple<int, int>(player.x, player.y), player);
                                    //    int a = 12;
                                    //}
                                    //int a = 12;
                                }
                                if (checkForEnemy(player.x, player.y, player.enemy, linesWithPlayers))
                                {
                                    attack(player.x, player.y, player, linesWithPlayers, players);
                                }
                                createLines(ref linesWithPlayers, players, cleanListCopy);
                                countColors(linesWithPlayers, players);
                                //sprawdzanie odleg³oœci z ponad pod po lewej i po prawej, gdy wiele najkrótszych ta która jest szybciej czytana
                                //foreach (var VARIABLE in linesWithPlayers)
                                //{
                                //    Console.WriteLine(VARIABLE);
                                //}
                                //Console.WriteLine();
                            }
                        }
                        else
                        {
                            int y = 12;
                        }

                    }
                    //sprawdzanie czy w players jedynie jeden typ
                    var firstPlayerChar = players.First().Value.character;
                    bool allSame = true;
                    foreach (var player in players)
                    {
                        if (player.Value.character != firstPlayerChar)
                        {
                            allSame = false;
                            break;
                        }
                    }

                    if (allSame)
                    {
                        break;
                    }

                    //Console.WriteLine("\nAfter {0} round:", round);
                    //foreach (var VARIABLE in linesWithPlayers)
                    //{
                    //    Console.WriteLine(VARIABLE);
                    //}
                    //foreach (var VARIABLE in linesWithPlayers)
                    //{
                    //    Console.WriteLine(VARIABLE);
                    //}
                    //Console.WriteLine();

                    linesWithPlayers = linesClean;
                    linesClean = new List<char[]>();
                    foreach (var line in cleanListCopy)
                    {
                        linesClean.Add(line.ToCharArray());
                    }
                    round++;

                }
                //Console.WriteLine("\n End:");
                int sum = 0;
                if (players.First().Value.character != 'E' || players.Count != elfsCnt)
                {
                    elfPower++;
                    //continue;
                }

                foreach (var player in players)
                {
                    sum += player.Value.hitPoints;
                }
                foreach (var VARIABLE in linesWithPlayers)
                {
                    Console.WriteLine(VARIABLE);
                }
                Console.WriteLine(round);
                Console.WriteLine("\nOutcome: {0}", round * sum);
                Console.WriteLine("\nOutcome: {0} (round -1)*sum", (round - 1) * sum);
                Console.WriteLine(elfPower);
                break;
            }

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            //Console.ReadLine();
        }
    }
}