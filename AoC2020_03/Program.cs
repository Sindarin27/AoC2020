using System;
using System.Drawing;

namespace AoC2020_3
{
    class Program
    {
        static void Main(string[] args)
        {
            // Read dimensions (provided by user; better get counting!)
            Console.WriteLine("width = ?");
            int width = int.Parse(Console.ReadLine());
            Console.WriteLine("height = ?");
            int height = int.Parse(Console.ReadLine());
            
            bool[,] world = new bool[width, height];
            
            // Read the map
            for (int y = 0; y < height; y++)
            {
                string line = Console.ReadLine();
                for (int x = 0; x < width; x++)
                {
                    world[x, y] = FromChar(line[x]);
                }
            }

            int total = 1;
            
            // Do each of the required walking strategies.
            total *= Walk(new Point(1, 1), world, width, height);
            total *= Walk(new Point(3, 1), world, width, height);
            total *= Walk(new Point(5, 1), world, width, height);
            total *= Walk(new Point(7, 1), world, width, height);
            total *= Walk(new Point(1, 2), world, width, height);

            Console.WriteLine("Trees: " + total);
        }

        // Walk an entire map with the provided step size, return the amount of trees bumped into.
        private static int Walk(Point step, bool[,] map, int width, int height)
        {
            bool  goal   = false;
            Point pos    = new Point(0,0);
            int   points = 0;
            
            while (goal == false)
            {
                if (pos.Y >= height) // Reached the end
                {
                    goal = true;
                    continue;
                }
                points += Points(pos, map);
                pos = Step(pos, step, width);
            }

            return points;
        }

        // Determine a map boolean from a character on the map.
        private static bool FromChar(char c)
        {
            return c == '#';
        }

        // Return the amount of "points" a square gets: tree gives 1, empty gives 0.
        private static int Points(Point pos, bool[,] map)
        {
            return map[pos.X, pos.Y] ? 1 : 0;
        }

        // Move the point to the next position. Loops back around the x-axis.
        private static Point Step(Point pos, Point step, int width)
        {
            pos.X = (pos.X + step.X) % width;
            pos.Y += step.Y;
            return pos;
        }
    }
}