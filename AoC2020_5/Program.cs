using System;
using System.Linq.Expressions;

namespace AoC2020_5
{
    class Program
    {
        static void Main(string[] args)
        {
            // RunTests();
            
            bool[] seatsTaken = new bool[1024]; // Create a boolean array for every seat. Not elegant, but it works very well
            
            String line = Console.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                seatsTaken[GetSeatNum(line)] = true; // The seat on the read line is taken. Next.
                
                line = Console.ReadLine();
            }

            bool firstSeatFound = false;
            for (int i = 0; i < 1024; i++)
            {
                if (!seatsTaken[i] && !firstSeatFound) continue; // An empty chair, because it does not exist on this plane.
                if (!seatsTaken[i] && firstSeatFound) // An empty chair? And we already found the first chair? This is our spot!
                {
                    Console.WriteLine($"Solution!!!: {i}");
                    return;
                }
                if (seatsTaken[i]) firstSeatFound = true; // Any chair found means the first chair has been found.
            }

        }

        private static void RunTests()
        {
            // Check the tests in the example
            Test("FBFBBFFRLR", 357);
            Test("BFFFBBFRRR", 567);
            Test("FFFBBBFRRR", 119);
            Test("BBFFBBFRLL", 820);
            Console.WriteLine("Tests finished");
        }
        
        // Test whether a certain chair returns the expected value
        private static bool Test(String text, int expected)
        {
            int returned = GetSeatNum(text);
            if (returned == expected) return true;
            else
            {
                Console.WriteLine($"Test {text} failed, returned {returned} but expected {expected}");
                return false;
            }
        }

        // Get the seat number according to the rules set.
        private static int GetSeatNum(String text)
        {
            int row = Decode(text, 0, 7, 'B'); // Row is in the first 7 characters, with F being low and B high
            int col = Decode(text, 7, 3, 'R'); // Col is in the last 3 characters, with L being low and R high
            return row * 8 + col;
        }

        // Decode a piece of text, starting from startIndex and reading numsToRead numbers.
        // highIndicator is the character that tells us we need to take the upper half.
        private static int Decode(String text, int startIndex, int numsToRead, char highIndicator)
        {
            int lower  = 0;
            int higher = (int) Math.Pow(2, numsToRead) - 1;
            int endIndex = startIndex + numsToRead;

            for (int i = startIndex; i < endIndex; i++)
            {
                // Console.WriteLine($"Lower is {lower} and higher is {higher}");
                char c = text[i];
                // Console.Write($"{c}: ");
                if (c == highIndicator) lower = (higher + lower) / 2 + 1; // Remove the lower half
                else higher = (higher + lower) / 2; // Remove the upper half
                
            }
            // Console.WriteLine($"Lower is {lower} and higher is {higher}");
            return lower;
        }
    }
}