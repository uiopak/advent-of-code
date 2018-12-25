using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace Day24
{
    public class Program
    {
        public enum attackType
        {
            cold,
            radiation,
            slashing,
            bludgeoning,
            fire
        }
        public enum groupType
        {
            Immune,
            Infection
        }

        public class Group
        {
            public groupType Gt;
            public bool target;
            public int Units;
            public int Hp;
            public int Damage;
            public attackType DamageType;
            public int Initiative;
            public List<attackType> Immune;
            public List<attackType> Weak;
            public int Boost = 0;
            public Group ChosenEnemy = null;
            public Group(int units, int hp, int damage, int initiative, groupType gt)
            {
                Units = units;
                Hp = hp;
                Damage = damage;
                Initiative = initiative;
                Immune = new List<attackType>();
                Weak = new List<attackType>();
                Gt = gt;
                target = false;
            }

            public int TheoreticalDamage(Group enemy)
            {
                if (enemy.Immune.Contains(DamageType))
                {
                    return 0;
                }
                else if (enemy.Weak.Contains(DamageType))
                {
                    return Units * (Damage + Boost) * 2;
                }
                return Units * (Damage + Boost);
            }

            public bool Attack()
            {
                if (ChosenEnemy != null)
                {
                    int damage = TheoreticalDamage(ChosenEnemy);
                    ChosenEnemy.Units -= (damage / ChosenEnemy.Hp);
                    if (ChosenEnemy.Units > 0)
                    {
                        ChosenEnemy.target = false;
                        return false;
                    }
                    else
                    {
                        ChosenEnemy.target = false;
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public static void Main()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var input = File.ReadAllLines("input");
            bool immuneSystem = false;
            bool infection = false;
            var immuneGroups = new List<Group>();
            var infectionGroups = new List<Group>();
            foreach (var s in input)
            {
                var split = s.Split(' ');
                if (s == "")
                {
                    immuneSystem = false;
                }
                if (immuneSystem)
                {
                    var numbers = Regex.Matches(s, @"-?\d+");
                    var tmpGroup = new Group(int.Parse(numbers[0].Value), int.Parse(numbers[1].Value), int.Parse(numbers[2].Value), int.Parse(numbers[3].Value), groupType.Immune);
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i] == "(immune" || split[i] == "immune")
                        {
                            i++;//skipping to
                            bool check = true;
                            while (check)
                            {
                                i++;
                                switch (split[i])
                                {
                                    case "cold;":
                                    case "cold)":
                                        check = false;
                                        tmpGroup.Immune.Add(attackType.cold);
                                        break;
                                    case "cold,":
                                        tmpGroup.Immune.Add(attackType.cold);
                                        break;
                                    case "fire;":
                                    case "fire)":
                                        check = false;
                                        tmpGroup.Immune.Add(attackType.fire);
                                        break;
                                    case "fire,":
                                        tmpGroup.Immune.Add(attackType.fire);
                                        break;
                                    case "radiation;":
                                    case "radiation)":
                                        check = false;
                                        tmpGroup.Immune.Add(attackType.radiation);
                                        break;
                                    case "radiation,":
                                        tmpGroup.Immune.Add(attackType.radiation);
                                        break;
                                    case "slashing;":
                                    case "slashing)":
                                        check = false;
                                        tmpGroup.Immune.Add(attackType.slashing);
                                        break;
                                    case "slashing,":
                                        tmpGroup.Immune.Add(attackType.slashing);
                                        break;
                                    case "bludgeoning;":
                                    case "bludgeoning)":
                                        check = false;
                                        tmpGroup.Immune.Add(attackType.bludgeoning);
                                        break;
                                    case "bludgeoning,":
                                        tmpGroup.Immune.Add(attackType.bludgeoning);
                                        break;
                                }
                            }
                        }
                        if (split[i] == "(weak" || split[i] == "weak")
                        {
                            i++;//skipping to
                            bool check = true;
                            while (check)
                            {
                                i++;
                                switch (split[i])
                                {
                                    case "cold;":
                                    case "cold)":
                                        check = false;
                                        tmpGroup.Weak.Add(attackType.cold);
                                        break;
                                    case "cold,":
                                        tmpGroup.Weak.Add(attackType.cold);
                                        break;
                                    case "fire;":
                                    case "fire)":
                                        check = false;
                                        tmpGroup.Weak.Add(attackType.fire);
                                        break;
                                    case "fire,":
                                        tmpGroup.Weak.Add(attackType.fire);
                                        break;
                                    case "radiation;":
                                    case "radiation)":
                                        check = false;
                                        tmpGroup.Weak.Add(attackType.radiation);
                                        break;
                                    case "radiation,":
                                        tmpGroup.Weak.Add(attackType.radiation);
                                        break;
                                    case "slashing;":
                                    case "slashing)":
                                        check = false;
                                        tmpGroup.Weak.Add(attackType.slashing);
                                        break;
                                    case "slashing,":
                                        tmpGroup.Weak.Add(attackType.slashing);
                                        break;
                                    case "bludgeoning;":
                                    case "bludgeoning)":
                                        check = false;
                                        tmpGroup.Weak.Add(attackType.bludgeoning);
                                        break;
                                    case "bludgeoning,":
                                        tmpGroup.Weak.Add(attackType.bludgeoning);
                                        break;
                                }
                            }
                        }
                        if (split[i] == "does")
                        {
                            switch (split[i + 2])
                            {
                                case "fire":
                                    tmpGroup.DamageType = attackType.fire;
                                    break;
                                case "bludgeoning":
                                    tmpGroup.DamageType = attackType.bludgeoning;
                                    break;
                                case "cold":
                                    tmpGroup.DamageType = attackType.cold;
                                    break;
                                case "radiation":
                                    tmpGroup.DamageType = attackType.radiation;
                                    break;
                                case "slashing":
                                    tmpGroup.DamageType = attackType.slashing;
                                    break;
                            }
                        }
                    }
                    immuneGroups.Add(tmpGroup);
                }
                if (s == "Immune System:")
                {
                    immuneSystem = true;
                }


                if (infection)
                {
                    var numbers = Regex.Matches(s, @"-?\d+");

                    var tmpGroup = new Group(int.Parse(numbers[0].Value), int.Parse(numbers[1].Value), int.Parse(numbers[2].Value), int.Parse(numbers[3].Value), groupType.Infection);
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (split[i] == "(immune" || split[i] == "immune")
                        {
                            i++;//skipping to
                            bool check = true;
                            while (check)
                            {
                                i++;
                                switch (split[i])
                                {
                                    case "cold;":
                                    case "cold)":
                                        check = false;
                                        tmpGroup.Immune.Add(attackType.cold);
                                        break;
                                    case "cold,":
                                        tmpGroup.Immune.Add(attackType.cold);
                                        break;
                                    case "fire;":
                                    case "fire)":
                                        check = false;
                                        tmpGroup.Immune.Add(attackType.fire);
                                        break;
                                    case "fire,":
                                        tmpGroup.Immune.Add(attackType.fire);
                                        break;
                                    case "radiation;":
                                    case "radiation)":
                                        check = false;
                                        tmpGroup.Immune.Add(attackType.radiation);
                                        break;
                                    case "radiation,":
                                        tmpGroup.Immune.Add(attackType.radiation);
                                        break;
                                    case "slashing;":
                                    case "slashing)":
                                        check = false;
                                        tmpGroup.Immune.Add(attackType.slashing);
                                        break;
                                    case "slashing,":
                                        tmpGroup.Immune.Add(attackType.slashing);
                                        break;
                                    case "bludgeoning;":
                                    case "bludgeoning)":
                                        check = false;
                                        tmpGroup.Immune.Add(attackType.bludgeoning);
                                        break;
                                    case "bludgeoning,":
                                        tmpGroup.Immune.Add(attackType.bludgeoning);
                                        break;
                                }
                            }
                        }
                        if (split[i] == "(weak" || split[i] == "weak")
                        {
                            i++;//skipping to
                            bool check = true;
                            while (check)
                            {
                                i++;
                                switch (split[i])
                                {
                                    case "cold;":
                                    case "cold)":
                                        check = false;
                                        tmpGroup.Weak.Add(attackType.cold);
                                        break;
                                    case "cold,":
                                        tmpGroup.Weak.Add(attackType.cold);
                                        break;
                                    case "fire;":
                                    case "fire)":
                                        check = false;
                                        tmpGroup.Weak.Add(attackType.fire);
                                        break;
                                    case "fire,":
                                        tmpGroup.Weak.Add(attackType.fire);
                                        break;
                                    case "radiation;":
                                    case "radiation)":
                                        check = false;
                                        tmpGroup.Weak.Add(attackType.radiation);
                                        break;
                                    case "radiation,":
                                        tmpGroup.Weak.Add(attackType.radiation);
                                        break;
                                    case "slashing;":
                                    case "slashing)":
                                        check = false;
                                        tmpGroup.Weak.Add(attackType.slashing);
                                        break;
                                    case "slashing,":
                                        tmpGroup.Weak.Add(attackType.slashing);
                                        break;
                                    case "bludgeoning;":
                                    case "bludgeoning)":
                                        check = false;
                                        tmpGroup.Weak.Add(attackType.bludgeoning);
                                        break;
                                    case "bludgeoning,":
                                        tmpGroup.Weak.Add(attackType.bludgeoning);
                                        break;
                                }
                            }
                        }
                        if (split[i] == "does")
                        {
                            switch (split[i + 2])
                            {
                                case "fire":
                                    tmpGroup.DamageType = attackType.fire;
                                    break;
                                case "bludgeoning":
                                    tmpGroup.DamageType = attackType.bludgeoning;
                                    break;
                                case "cold":
                                    tmpGroup.DamageType = attackType.cold;
                                    break;
                                case "radiation":
                                    tmpGroup.DamageType = attackType.radiation;
                                    break;
                                case "slashing":
                                    tmpGroup.DamageType = attackType.slashing;
                                    break;
                            }
                        }
                    }
                    infectionGroups.Add(tmpGroup);
                }

                if (s == "Infection:")
                {
                    infection = true;
                }
            }
            var tmpImmune = immuneGroups.ToArray();
            var tmpImmuneUnits = tmpImmune.Select(@group => @group.Units).ToList();
            var tmpInfection = infectionGroups.ToArray();
            var tmpInfectionUnits = tmpInfection.Select(@group => @group.Units).ToList();
            int boost = 0;
            while (infectionGroups.Count > 0)
            {
                immuneGroups = tmpImmune.ToList();
                for (int i = 0; i < immuneGroups.Count; i++)
                {
                    immuneGroups[i].Boost = boost;
                    immuneGroups[i].Units = tmpImmuneUnits[i];
                }
                infectionGroups = tmpInfection.ToList();
                for (int i = 0; i < infectionGroups.Count; i++)
                {
                    infectionGroups[i].Units = tmpInfectionUnits[i];
                }

                while (infectionGroups.Count > 0 && immuneGroups.Count > 0)
                {
                    //Console.WriteLine("Immune System:");
                    //for (int i = 0; i < immuneGroups.Count; i++)
                    //{
                    //    Console.WriteLine("Group {0} contains {1} units", i + 1, immuneGroups[i].Units);
                    //}
                    //Console.WriteLine();
                    //Console.WriteLine("Infection:");
                    //for (int i = 0; i < infectionGroups.Count; i++)
                    //{
                    //    Console.WriteLine("Group {0} contains {1} units", i + 1, infectionGroups[i].Units);
                    //}
                    //Console.WriteLine("-------------------------------");
                    var groupUnion = infectionGroups.Union(immuneGroups).ToList();
                    groupUnion = groupUnion.OrderByDescending(c => (c.Units * (c.Damage + c.Boost))).ToList();
                    groupUnion = groupUnion.OrderByDescending(c => (c.Units * (c.Damage + c.Boost))).ThenByDescending(c => c.Initiative)
                        .ToList();
                    foreach (var @group in groupUnion)
                    {
                        if (@group.Gt == groupType.Immune)
                        {
                            var tmp = infectionGroups.Where(c => c.target == false)
                                .OrderByDescending(c => @group.TheoreticalDamage(c))
                                .ThenByDescending(c => c.Units * (c.Damage + c.Boost))
                                .ThenByDescending(c => c.Initiative);
                            if (tmp.FirstOrDefault() != null && @group.TheoreticalDamage(tmp.FirstOrDefault()) > 0)
                            {
                                @group.ChosenEnemy = tmp.FirstOrDefault();

                            }
                            else
                            {
                                @group.ChosenEnemy = null;
                            }

                            if (@group.ChosenEnemy != null) @group.ChosenEnemy.target = true;
                        }
                        else
                        {
                            var tmp = immuneGroups.Where(c => c.target == false)
                                .OrderByDescending(c => @group.TheoreticalDamage(c))
                                .ThenByDescending(c => c.Units * (c.Damage + c.Boost))
                                .ThenByDescending(c => c.Initiative);
                            if (tmp.FirstOrDefault() != null && @group.TheoreticalDamage(tmp.FirstOrDefault()) > 0)
                            {
                                @group.ChosenEnemy = tmp.FirstOrDefault();

                            }
                            else
                            {
                                @group.ChosenEnemy = null;
                            }

                            if (@group.ChosenEnemy != null) @group.ChosenEnemy.target = true;
                        }
                    }

                    var tmpCopy = groupUnion.OrderByDescending(c => c.Initiative).ToList();
                    for (int i = 0; i < tmpCopy.Count; i++)
                    {
                        var @group = tmpCopy[i];
                        if (groupUnion.Contains(@group))
                        {
                            @group.target = false;
                            if (@group.Attack())
                            {
                                groupUnion.Remove(@group.ChosenEnemy);
                                immuneGroups.Remove(@group.ChosenEnemy);
                                infectionGroups.Remove(@group.ChosenEnemy);
                                @group.ChosenEnemy = null;
                            }
                            else
                            {
                                @group.ChosenEnemy = null;
                            }

                        }

                    }

                    if (boost == 34)
                    {
                        int a = 12;
                    }
                    bool canKillImmune = false;
                    bool canKillInfection = false;
                    foreach (var immuneGroup in immuneGroups)
                    {
                        foreach (var infectionGroup in infectionGroups)
                        {
                            if (immuneGroup.TheoreticalDamage(infectionGroup) / infectionGroup.Hp > 0)
                            {
                                canKillInfection = true;
                                goto next;
                            }
                        }
                    }
                next:;
                    foreach (var infectionGroup in infectionGroups)
                    {
                        foreach (var immuneGroup in immuneGroups)
                        {
                            if (immuneGroup.TheoreticalDamage(infectionGroup) / infectionGroup.Hp > 0)
                            {
                                canKillImmune = true;
                                goto next2;
                            }
                        }
                    }
                next2:;
                    if (!canKillImmune || !canKillInfection)
                    {
                        break;
                    }
                }
                boost++;
            }
            var groupUnion2 = infectionGroups.Union(immuneGroups).ToList();
            int cnt = 0;
            foreach (var @group in groupUnion2)
            {
                cnt += @group.Units;
            }
            Console.WriteLine(cnt);

            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:000.000000} ms", ts.TotalMilliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            Console.ReadLine();
        }
    }
}