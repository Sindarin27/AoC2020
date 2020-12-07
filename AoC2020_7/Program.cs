using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AoC2020_7
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Dictionary<string, int>> bags = ParseAllBags();
            Console.WriteLine(HowManyBagsContainA(bags, "shiny gold") + " bags can contain a shiny gold bag.");
            Console.WriteLine("A shiny gold bag holds " + (HowManyBagsIfYouStartWithA(bags, "shiny gold") - 1) +
                              " extra bags.");
        }

        // Parse all bags in the input
        private static Dictionary<string, Dictionary<string, int>> ParseAllBags()
        {
            Dictionary<string, Dictionary<string, int>> bags = new Dictionary<string, Dictionary<string, int>>();
            string                                      line = Console.ReadLine();

            // Keep reading until encountering an empty line
            while (!string.IsNullOrEmpty(line))
            {
                KeyValuePair<string, Dictionary<string, int>> bag = ReadBag(line);
                bags.Add(bag.Key, bag.Value);

                line = Console.ReadLine();
            }

            return bags;
        }

        // Count how many bags contain a certain named bag
        private static int HowManyBagsContainA(Dictionary<string, Dictionary<string, int>> bags, string bagToContain)
        {
            int counter = 0;

            foreach (string bag in bags.Keys)
            {
                if (DoesBagContain(bags, bag, bagToContain)) counter++;
            }

            return counter;
        }

        // Count how many bags are in a certain named bag. Includes the bag itself.
        private static int HowManyBagsIfYouStartWithA(Dictionary<string, Dictionary<string, int>> bags,
            string bagToSearch)
        {
            if (!bags.TryGetValue(bagToSearch, out Dictionary<string, int> directContains)) return 0;

            int counter = 1; // The bag itself

            foreach (KeyValuePair<string, int> bag in directContains)
            {
                // Recursion! Check the contents of the bags one down.
                // We might contain this bag multiple times, so multiply it by how many we have of it
                counter += HowManyBagsIfYouStartWithA(bags, bag.Key) * bag.Value;
            }

            return counter;
        }

        // Check if a certain named bag contains another named bag, either directly or indirectly.
        private static bool DoesBagContain(Dictionary<string, Dictionary<string, int>> bags, string bagToSearch,
            string bagToContain)
        {
            if (!bags.TryGetValue(bagToSearch, out Dictionary<string, int> directContains)) return false;
            // Check if this bag contains the other bag directly
            if (directContains.ContainsKey(bagToContain)) return true;
            // Check if any sub-bag contains the bag
            return directContains.Any(subBag => DoesBagContain(bags, subBag.Key, bagToContain));
        }

        private static KeyValuePair<string, Dictionary<string, int>> ReadBag(string line)
        {
            // "{name1}" "{name2}" "bags" "contain" "{n}" "{name1}" "{name2}" "bag,"
            // e.g. "vibrant" "plum" "bags" "contain" "5" "faded" "blue" "bags," ...
            string[] parts   = line.Split(' ');
            string   bagName = parts[0] + ' ' + parts[1];
            // Console.WriteLine("Parsed bag " + bagName); // Debug bag name parsing

            Dictionary<string, int> contains = new Dictionary<string, int>();

            // One bag consists out of four parts: a count, a first name part, a second name part, and a "bags"
            for (int i = 4; i < parts.Length; i += 4)
            {
                if (parts[i] == "no") continue; // Sneaky, they're trying to trick us...

                // Read the count and name of the bag contained
                int    insideCount = int.Parse(parts[i]);
                string insideName  = parts[i + 1] + ' ' + parts[i + 2];
                // Console.WriteLine("Contains " + insideCount + " " + insideName); // Debug bag child parsing
                contains.Add(insideName, insideCount);
            }

            return new KeyValuePair<string, Dictionary<string, int>>(bagName, contains);
        }
    }
}