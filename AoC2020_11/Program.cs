using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2020_11
{
    class Program
    {
        public static bool PT2 = true;
        static void Main(string[] args)
        {
            // Read everything
            List<string> lines = new List<string>();
            string       line  = Console.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                lines.Add(line);
                line = Console.ReadLine();
            }

            // Parse everything
            int width  = lines[0].Length,
                height = lines.Count;
            Spot[,] seats = new Spot[width, height];
            for (int y = 0; y < height; y++)
            {
                string current = lines[y];
                for (int x = 0; x < width; x++)
                {
                    seats[x, y] = new Spot(current[x]);
                }
            }

            // Run the problem
            Spot[,] newSpots;
            if (PT2)
            {
                // Keep going until things stop changing
                while (Update(seats, out newSpots, CountOccupiedSeenAround, 5))
                {
                    seats = newSpots;
                }
            }
            else
            {
                // Keep going until things stop changing
                while (Update(seats, out newSpots, CountOccupiedAround, 4))
                {
                    seats = newSpots;
                }
            }

            // Newspots now has an unchanged value.
            Console.WriteLine($"There are {CountOccupied(newSpots)} occupied seats.");
        }

        // Writes an array of spots to the console, in the same fashion as the original example
        private static void WriteArrayToConsole(Spot[,] array)
        {
            int width  = array.GetLength(0),
                height = array.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                    switch (array[x, y].seatType)
                    {
                        case Spot.SeatType.FLOOR:
                            Console.Write(".");
                            break;
                        case Spot.SeatType.FREE:
                            Console.Write("L");
                            break;
                        case Spot.SeatType.OCCUPIED:
                            Console.Write("#");
                            break;
                    }

                Console.WriteLine("");
            }

            Console.WriteLine("---------------");
        }

        // Count the total number of occupied seats in an array
        private static int CountOccupied(Spot[,] seats)
        {
            return seats.Cast<Spot>().Count(spot => spot.seatType == Spot.SeatType.OCCUPIED);
        }
        
        // Update the array with the given function and spot count.
        // Updates to the array in out, returns a boolean to determine if anything changed.
        private static bool Update(Spot[,] old, out Spot[,] newSpots, Func<Spot[,], int, int, int> countFunction, int spotsBeforeLeave)
        {
            int width  = old.GetLength(0),
                height = old.GetLength(1);
            newSpots = new Spot[width, height];
            bool changed = false;
            
            // Update all seats
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                switch (old[x, y].seatType)
                {
                    // Floor always remains the same. Copy to new array.
                    case Spot.SeatType.FLOOR:
                        newSpots[x, y] = old[x, y];
                        break;
                    // Free seats become occupied if the count around them is 0.
                    case Spot.SeatType.FREE:
                        if (countFunction(old, x, y) == 0)
                        {
                            newSpots[x, y] = Spot.Occupied;
                            changed = true;
                        }
                        else newSpots[x, y] = old[x, y];

                        break;
                    // Occupied seats become free if the count around them is >= spotsBeforeLeave
                    case Spot.SeatType.OCCUPIED:
                        if (countFunction(old, x, y) >= spotsBeforeLeave)
                        {
                            newSpots[x, y] = Spot.Free;
                            changed = true;
                        }
                        else newSpots[x, y] = old[x, y];

                        break;
                }

            return changed;
        }

        // Count occupied chairs directly adjacent to a given chair
        private static int CountOccupiedAround(Spot[,] array, int x, int y)
        {
            int around = 0;
            for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue; // Seat itself
                if (GetSeatSafely(array, x + i, y + j).seatType == Spot.SeatType.OCCUPIED) around++;
            }

            return around;
        }
        
        // Count occupied chairs visible from a given chair
        private static int CountOccupiedSeenAround(Spot[,] array, int x, int y)
        {
            int around = 0;
            for (int i = -1; i <= 1; i++)
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue; // Seat itself
                if (OccupiedInDirection(array, x, y, i, j)) around++;
            }

            return around;
        }

        // Take a chair and a direction, and check if an occupied chair is visible in that direction
        private static bool OccupiedInDirection(Spot[,] array, int x, int y, int dx, int dy)
        {
            int width  = array.GetLength(0),
                height = array.GetLength(1);
            x += dx;
            y += dy;
            if (x < 0 || y < 0 || x >= width || y >= height) return false;
            
            while (array[x, y].seatType != Spot.SeatType.OCCUPIED)
            {
                if (array[x, y].seatType == Spot.SeatType.FREE) return false;
                x += dx;
                y += dy;
                if (x < 0 || y < 0 || x >= width || y >= height) return false;
            }

            return true;
        }

        // Dumb way of getting a seat that returns floor if it's outside the array
        private static Spot GetSeatSafely(Spot[,] array, int x, int y)
        {
            try
            {
                return array[x, y];
            }
            catch (IndexOutOfRangeException)
            {
                return Spot.Floor;
            }
        }
    }
    
    // Since there's no default class with three possible values, might as well make it myself
    internal readonly struct Spot
    {
        public enum SeatType
        {
            FLOOR,
            FREE,
            OCCUPIED
        }

        public readonly SeatType seatType;

        // Make a spot from the text identifier
        public Spot(char identifier)
        {
            switch (identifier)
            {
                case 'L':
                    seatType = SeatType.FREE;
                    break;
                case '#':
                    seatType = SeatType.OCCUPIED;
                    break;
                default:
                    seatType = SeatType.FLOOR;
                    break;
            }
        }

        public Spot(SeatType seatType)
        {
            this.seatType = seatType;
        }

        public static Spot Occupied => new Spot(SeatType.OCCUPIED);
        public static Spot Free => new Spot(SeatType.FREE);
        public static Spot Floor => new Spot(SeatType.FLOOR);
    }
}