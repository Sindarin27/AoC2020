using System;
using System.Linq;

namespace AoC2020_2
{
    class Program
    {
        static void Main(string[] args)
        {
            String line = Console.ReadLine();
            int count = 0;
            
            while (line != null && line != "finish")
            {
                String[] parts = line.Split(':', ' ', '-');
                int min = int.Parse(parts[0]);
                int max = int.Parse(parts[1]);
                char letter = char.Parse(parts[2]);
                string pass = parts[4];

                if (Check(min, max, letter, pass)) count++;

                line = Console.ReadLine();
            }
            
            Console.WriteLine("Count = " + count);
        }

        private static bool CheckOld(int min, int max, char letter, string pass)
        {
            int occ = pass.Count(c => c == letter);
            return (occ >= min && occ <= max);
        }

        private static bool Check(int a, int b, char ch, string str)
        {
            try
            {
                bool pos1 = str[a - 1] == ch;
                bool pos2 = str[b - 1] == ch;
                
                if (pos1 && pos2) return false;
                else if (pos1 || pos2) return true;
                else return false;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }
    }
}