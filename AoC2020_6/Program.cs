using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020_6
{
    class Program
    {
        static void Main(string[] args)
        {
            int total = 0;
            
            while (true)
            {
                // Read one group and write the total
                total += ReadGroupIntersect();
                Console.WriteLine(total);
            }
        }
        
        // Part one: union of all letters
        private static int ReadGroupUnion()
        {
            string line = Console.ReadLine();
            IEnumerable<char> all = ""; // Start with no letters; grows as time goes by

            while (!string.IsNullOrEmpty(line)) // Keep reading until the next whitespace
            {
                all = all.Union(line);

                line = Console.ReadLine(); // Next line
            }

            return all.Count();
        }
        
        // Part two: intersect of all letters
        private static int ReadGroupIntersect()
        {
            string line = Console.ReadLine();
            IEnumerable<char> all = "abcdefghijklmnopqrstuvwxyz"; // Start with all letters; shrinks as time goes by

            while (!string.IsNullOrEmpty(line)) // Keep reading until the next whitespace
            {
                all = all.Intersect(line);

                line = Console.ReadLine(); // Next line
            }

            return all.Count();
        }
    }
}