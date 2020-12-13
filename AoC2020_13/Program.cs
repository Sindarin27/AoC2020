using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020_13
{
    class Program
    {
        static void Main(string[] args)
        {
            int      target      = int.Parse(Console.ReadLine());
            string[] timesString = Console.ReadLine().Split(',');
            int[]    times       = new int[timesString.Length];
            for (int i = 0; i < times.Length; i++)
            {
                if (timesString[i] == "x") times[i] = -1;
                else times[i] = int.Parse(timesString[i]);
            }

            KeyValuePair<int, int> firstLeave    = FindFirstLeaveTime(times, target);
            int                    minutesToWait = firstLeave.Value - target;
            Console.WriteLine(
                $"Earliest time we can leave is {firstLeave}. We'd need to wait {minutesToWait} minutes. Answer {firstLeave.Key * minutesToWait}");
            Console.WriteLine($"Buses will first converge on {DoChineseRemainder(times)}");
        }

        // Find the first time after target that a bus leaves
        private static KeyValuePair<int, int> FindFirstLeaveTime(int[] times, int target)
        {
            int[] goalTimes = new int[times.Length];
            for (int i = 0; i < times.Length; i++)
            {
                int time = times[i];
                if (time == -1)
                {
                    goalTimes[i] = Int32.MaxValue; // This bus won't ever leave
                    continue;
                }

                goalTimes[i] = (target / time) * time + time; // First time this bus will leave after or at target
            }

            int min = goalTimes.Min(); // First bus that will leave
            return new KeyValuePair<int, int>(times[Array.IndexOf(goalTimes, min)], min); // Find the matching bus too
        }

        private static bool CheckStep(int[] times, long step)
        {
            // Check if there's any bus that DOESN'T match, and not that.
            return !times.Where((time, i) => time != -1 && step % time != i).Any();
        }

        // This is the super slow variant
        private static long FindAnswerToPart2(int[] numbers, long startOffset)
        {
            int minimumStepSize = numbers.Max();
            int offset          = Array.IndexOf(numbers, minimumStepSize);
            // If we know we can start that far in, why not
            long startCountingAt = (startOffset / minimumStepSize) * minimumStepSize - offset;
            for (long i = startCountingAt;; i += minimumStepSize)
            {
                if (CheckStep(numbers, i)) return i;
            }
        }

        // This one is faster
        private static long DoChineseRemainder(int[] numbers)
        {
            // Multiply all numbers together to one big product
            // Ignore -1 (x)
            long product = numbers
                .Where(number => number != -1)
                .Aggregate<int, long>(1, (current, number) => current * number);

            // Now make a list of all the desired modulos
            // multiplied by the (product / number)
            // and the Modular Multiplicative Inverse
            List<long> numberProducts = new List<long>();
            for (int i = 0; i < numbers.Length; i++)
            {
                int number = numbers[i];
                if (number == -1) continue;

                int mod = number - i; // We don't want remainders 0, 1, 2. We want remainders 7-1, 13-2, etc. Found out by experimenting with the example.
                long pp  = product / number;
                long inv = FindInv(pp, number);
                numberProducts.Add(mod * pp * inv);
            }

            // Now sum em all together
            long sum = numberProducts.Sum();
            // Modulo them, and we've got our answer.
            return sum % product;
        }

        // There's probably an instant way of doing it, but this is nice enough.
        private static long FindInv(long pp, long n)
        {
            long inv = 0;

            while ((pp * inv) % n != 1)
            {
                inv++;
            }

            return inv;
        }
    }
}