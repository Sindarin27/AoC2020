using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020_9
{
    class Program
    {
        private static readonly int preambleLength = 25;

        static void Main(string[] args)
        {
            Queue<long> preamble = new Queue<long>();
            Queue<long> fullCode = new Queue<long>();
            long        invalid  = 0;

            string line = Console.ReadLine();

            // Read all numbers and find the incorrect one. Don't check for invalid numbers in the preamble.
            while (!string.IsNullOrEmpty(line))
            {
                long number = long.Parse(line);

                if (preamble.Count < preambleLength) preamble.Enqueue(number);
                else
                {
                    if (!CheckNumber(number, preamble))
                    {
                        Console.WriteLine("The number " + number + " is not valid!");
                        invalid = number;
                        break; // A number after this one can never form a sum to this number. Stop reading to save time.
                    }

                    // Remove the oldest item in the preamble and add this one.
                    preamble.Dequeue();
                    preamble.Enqueue(number);
                }

                // Always add this number to the full code.
                fullCode.Enqueue(number);

                line = Console.ReadLine();
            }

            // Clear the console.
            while (!string.IsNullOrEmpty(line)) line = Console.ReadLine();

            // Find the numbers summing up to the invalid one.
            KeyValuePair<long, long> summers = FindSum(invalid, fullCode);
            Console.WriteLine("It's the sum of the numbers between " + summers);
            FindSumBetween(summers.Key, summers.Value);
        }

        private static void FindSumBetween(long start, long end)
        {
            // I'm lazy.
            Console.WriteLine("Would you mind re-inputting those numbers?");
            long   min  = Math.Min(start, end);
            long   max  = Math.Max(start, end);
            string line = Console.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                // Find new minmax
                long number = long.Parse(line);
                min = Math.Min(number, min);
                max = Math.Max(number, max);
                line = Console.ReadLine();
            }

            Console.WriteLine($"Your min is {min} and max is {max} summing to {min + max}");
        }

        // Check whether a number is valid considering the current preamble
        private static bool CheckNumber(long number, Queue<long> preamble)
        {
            // Keep a hashset of numbers that would sum up to 'number'
            HashSet<long> possibleSums = new HashSet<long>();

            foreach (long n in preamble)
            {
                // If this number is a possible sum, return
                if (possibleSums.Contains(n)) return true;
                else possibleSums.Add(number - n);
            }

            return false;
        }

        private class LongLongPair
        {
            public long startN;
            public long value;

            public LongLongPair(long startN, long value)
            {
                this.startN = startN;
                this.value = value;
            }
        }

        private static KeyValuePair<long, long> FindSum(long sumToFind, Queue<long> code)
        {
            // Keep a list with every start number and their current sum
            List<LongLongPair> numbers = new List<LongLongPair>();

            // Check all numbers in the queue.
            // First, check if any number currently in the list combined with this reaches the goal.
            // If not, add it to the list of numbers.
            while (true)
            {
                long nextNum = code.Dequeue();

                // Check all numbers currently considered
                foreach (LongLongPair n in numbers)
                {
                    if (n.value + nextNum == sumToFind) return new KeyValuePair<long, long>(n.startN, nextNum);
                    else n.value += nextNum;
                }

                // Add this number
                numbers.Add(new LongLongPair(nextNum, nextNum));
            }
        }
    }
}