using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020_4
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(UnitTest()); // Run tests
            int count = 0;
            while (true)
            {
                if (ReadPassport()) count++;
                Console.WriteLine(count);
            }
        }

        private static bool UnitTest()
        {
            bool success = true;
            Dictionary<String, String> parts = new Dictionary<string, string>()
            {
                {"byrV", "2002"},
                {"byrI", "2003"},
                {"hgtV1", "60in"},
                {"hgtV2", "190cm"},
                {"hgtI1", "190in"},
                {"hgtI2", "190"},
                {"hclV", "#123abc"},
                {"hclI1", "#123abz"},
                {"hclI2", "123abc"},
                {"eclV", "brn"},
                {"eclI", "wat"},
                {"pidV", "000000001"},
                {"pidI", "0123456789"}
            };
            if (!CheckByr(parts, "byrV"))
            {
                success = false;
                Console.WriteLine("byrV failed");
            }
            if (CheckByr(parts, "byrI"))
            {
                success = false;
                Console.WriteLine("byrI failed");
            }
            if (!CheckHeight(parts, "hgtV1"))
            {
                success = false;
                Console.WriteLine("hgtV1 failed");
            }
            if (!CheckHeight(parts, "hgtV2"))
            {
                success = false;
                Console.WriteLine("hgtV2 failed");
            }
            if (CheckHeight(parts, "hgtI1"))
            {
                success = false;
                Console.WriteLine("hgtI1 failed");
            }
            if (CheckHeight(parts, "hgtI2"))
            {
                success = false;
                Console.WriteLine("hgtI2 failed");
            }
            if (!CheckHair(parts, "hclV"))
            {
                success = false;
                Console.WriteLine("hclV failed");
            }
            if (CheckHair(parts, "hclI1"))
            {
                success = false;
                Console.WriteLine("hclI1 failed");
            }
            if (CheckHair(parts, "hclI2"))
            {
                success = false;
                Console.WriteLine("hclI2 failed");
            }
            if (!CheckEcl(parts, "eclV"))
            {
                success = false;
                Console.WriteLine("eclV failed");
            }
            if (CheckEcl(parts, "eclI"))
            {
                success = false;
                Console.WriteLine("eclI failed");
            }
            if (!CheckPid(parts, "pidV"))
            {
                success = false;
                Console.WriteLine("pidV failed");
            }
            if (CheckPid(parts, "pidI"))
            {
                success = false;
                Console.WriteLine("pidI failed");
            }
            return success;
        }

        private static bool ReadPassport()
        {
            Dictionary<String, String> passportparts = new Dictionary<string, string>();
            String line = Console.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                String[] parts = line.Split(':', ' ');

                for (int i = 0; i < parts.Length; i += 2)
                {
                    passportparts.Add(parts[i], parts[i+1]);
                }
                
                line = Console.ReadLine();
            }

            return Complete(passportparts);
        }

        private static bool Complete(Dictionary<String, String> parts)
        {
            return (
                CheckByr(parts, "byr") &&
                CheckIyr(parts, "iyr") &&
                CheckEyr(parts, "eyr") &&
                CheckHeight(parts, "hgt") &&
                CheckHair(parts, "hcl") &&
                CheckEcl(parts, "ecl") &&
                CheckPid(parts, "pid")
            );
        }

        private static bool CheckByr(Dictionary<String, String> parts, String name) { return CheckNum(parts, name, 1920, 2002); }
        private static bool CheckIyr(Dictionary<String, String> parts, String name) { return CheckNum(parts, name, 2010, 2020); }
        private static bool CheckEyr(Dictionary<String, String> parts, String name) { return CheckNum(parts, name, 2020, 2030); }

        private static bool CheckNum(Dictionary<String, String> parts, String name, int min, int max)
        {
            if (!parts.ContainsKey(name)) return false;
            int number = int.Parse(parts[name]);
            return number >= min && number <= max;
        }

        private static bool CheckHeight(Dictionary<String, String> parts, String name)
        {
            if (!parts.ContainsKey(name)) return false;
            String value = parts[name];
            if (value.EndsWith("cm"))
            {
                value = value.TrimEnd('c', 'm');
                int height = int.Parse(value);
                return height >= 150 && height <= 193;
            }
            else if (value.EndsWith("in"))
            {
                value = value.TrimEnd('i', 'n');
                int height = int.Parse(value);
                return height >= 59 && height <= 76;
            }
            else return false;
        }

        private static bool CheckHair(Dictionary<String, String> parts, String name)
        {
            if (!parts.ContainsKey(name)) return false;
            String value = parts[name];
            return Regex.IsMatch(value, "#[0-9a-f]{6}");
        }

        private static String[] validEyeCols = {"amb", "blu", "brn", "gry", "grn", "hzl", "oth"};
        private static bool CheckEcl(Dictionary<String, String> parts, String name)
        {
            if (!parts.ContainsKey(name)) return false;
            String value = parts[name];
            return validEyeCols.Contains(value);
        }

        private static bool CheckPid(Dictionary<String, String> parts, String name)
        {
            if (!parts.ContainsKey(name)) return false;
            String value = parts[name];
            try { long.Parse(value); } catch (FormatException) { return false; } // Not a number

            return value.Length == 9;
        }
    }
}