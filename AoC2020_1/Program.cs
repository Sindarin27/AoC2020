using System;
using System.Collections.Generic;

namespace AoC2020_1
{
    class Program
    {
        static List<int> numbersRead = new List<int>();
        
        static void Main(string[] args)
        {
            string line = Console.ReadLine();
            while (line != null)
            {
                int number = int.Parse(line);

                foreach (int known in numbersRead)
                {
                    if (CheckSum(number, known))
                    {
                        return;
                    }
                }
                numbersRead.Add(number);
                line = Console.ReadLine();
            }
        }

        private static bool CheckSum(int a, int b)
        {
            if (a + b > 2020) return false;
            foreach (int c in numbersRead)
            {
                if (CheckBigSum(a, b, c)) return true;
            }

            return false;
        }

        private static bool CheckBigSum(int a, int b, int c)
        {
            if (a + b + c == 2020)
            {
                Console.WriteLine("Answer: " + a * b * c);
                return true;
            }

            return false;
        }
    }
}