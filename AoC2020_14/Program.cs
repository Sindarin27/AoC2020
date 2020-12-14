using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020_14
{
    public static class Extensions
    {
        public static void AddOrReplace<TA,TB>(this Dictionary<TA, TB> dict, TA key, TB value)
        {
            if (dict.ContainsKey(key)) dict[key] = value;
            else dict.Add(key, value);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string op = Console.ReadLine();
            string mask = "0";
            List<Func<ulong, ulong>> actions = new List<Func<ulong, ulong>>();
            
            Dictionary<int, ulong> posToValuePt1 = new Dictionary<int, ulong>();
            Dictionary<ulong, ulong> posToValuePt2 = new Dictionary<ulong, ulong>();
            
            while (!string.IsNullOrEmpty(op))
            {
                if (op.StartsWith("mask"))
                {
                    actions = GetMask(op);
                    mask = op;
                }
                else
                {
                    Match collection = Regex.Match(op, "^mem\\[(?<pos>\\d*?)\\] = (?<val>\\d*?)$");
                    int   pos        = int.Parse(collection.Groups["pos"].Value);
                    ulong val        = ulong.Parse(collection.Groups["val"].Value);
                    ulong modifiedVal = actions.Aggregate(val, (current, action) => action(current));
                    posToValuePt1.AddOrReplace(pos, modifiedVal);

                    List<ulong> writePositions = GetAllWritePositions(mask, (ulong) pos);
                    foreach (ulong position in writePositions)
                    {
                        posToValuePt2.AddOrReplace(position, val);
                    }
                }
                op = Console.ReadLine();
            }

            ulong sum = posToValuePt1.Values.Aggregate<ulong, ulong>(0, (current, value) => current + value);
            ulong sumTwo = posToValuePt2.Values.Aggregate<ulong, ulong>(0, (current, value) => current + value);
            Console.WriteLine($"Sum is {sum}");
            Console.WriteLine($"Second sum is {sumTwo}");
        }

        private static List<Func<ulong, ulong>> GetMask(string maskString)
        {
            string mask = Regex.Match(maskString, "^mask = (?<mask>.*?)$").Groups["mask"].Value;
            return MakeMask(mask);
        }

        private static List<ulong> GetAllWritePositions(string maskString, ulong position)
        {
            List<ulong> options = new List<ulong>(){0};
            for (int i = 0; i < 36; i++)
            {
                switch (maskString[^(i+1)])
                {
                    case '0':
                        options = options.Select(option => option | ((position >> i) % (ulong) 2 << i)).ToList();
                        break;
                    case '1':
                        options = options.Select(option => option | (ulong) 1 << i).ToList();
                        break;
                    default:
                        List<ulong> newOptions = new List<ulong>();
                        foreach (ulong option in options)
                        {
                            newOptions.Add(option | (ulong) 1 << i);
                            newOptions.Add(option);
                        }
                        options = newOptions;
                        break;
                }
            }

            return options;
        }

        private static List<Func<ulong, ulong>> MakeMask(string mask)
        {
            List<Func<ulong, ulong>> actions = new List<Func<ulong, ulong>>();
            
            for (int i = 0; i < mask.Length; i++)
            {
                switch (mask[^(i+1)])
                {
                    case '1':
                    {
                        int i1 = i;
                        actions.Add(n => SetBitToOne(i1, n));
                        break;
                    }
                    case '0':
                    {
                        int i1 = i;
                        actions.Add(n => SetBitToZero(i1, n));
                        break;
                    }
                }
            }

            return actions;
        }

        private static ulong SetBitToOne(int position, ulong value)
        {
            return value | ((ulong) 1 << position);
        }
        
        private static ulong SetBitToZero(int position, ulong value)
        {
            return value & ~((ulong) 1 << position);
        }
    }
}