using System;
using System.Linq;

namespace AoC2020_17
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Run part one or two? [1/2]");
            if (Console.ReadLine() == "1")
            {
                PartOne();
            }
            else
            {
                PartTwo();
            }
        }

        #region PartOne

        private static void PartOne()
        {
            // Create grid
            int      dimensions = int.Parse(Console.ReadLine());
            bool[,,] grid       = new bool[1, dimensions, dimensions];
            // Read grid
            for (int y = 0; y < dimensions; y++)
            {
                string line = Console.ReadLine();
                for (int x = 0; x < dimensions; x++)
                {
                    grid[0, x, y] = line[x] == '#';
                }
            }

            // Update 6 times
            for (int cycle = 0; cycle < 6; cycle++)
            {
                grid = UpdateGrid(grid);
            }
            
            // Calculate answer
            int sum = grid.Cast<bool>().Count(b => b);
            Console.WriteLine($"Bootup completed: {sum} active cubes");
        }

        // Draw grid for debugging purposes
        private static void DrawGrid(bool[,,] grid)
        {
            int zRange = grid.GetLength(0);
            int xRange = grid.GetLength(1);
            int yRange = grid.GetLength(2);

            for (int z = 0; z < zRange; z++)
            {
                Console.WriteLine($"z={z}");

                for (int y = 0; y < yRange; y++)
                {
                    for (int x = 0; x < xRange; x++)
                    {
                        Console.Write(grid[z, x, y] ? '#' : '.');
                    }

                    Console.WriteLine();
                }
            }
        }

        // Create the next cycle grid
        private static bool[,,] UpdateGrid(bool[,,] oldGrid)
        {
            // Calculate ranges and create new grid (because we want to batch update, no way to do that in place)
            int      zRange  = oldGrid.GetLength(0) + 2;
            int      xRange  = oldGrid.GetLength(1) + 2;
            int      yRange  = oldGrid.GetLength(2) + 2;
            bool[,,] newGrid = new bool[zRange, xRange, yRange];

            // Update every cell
            for (int z = 0; z < zRange; z++)
            {
                for (int y = 0; y < yRange; y++)
                {
                    for (int x = 0; x < xRange; x++)
                    {
                        newGrid[z, x, y] = UpdateCell(z, x, y, oldGrid);
                    }
                }
            }

            // End
            return newGrid;
        }

        private static bool UpdateCell(int z, int x, int y, bool[,,] oldGrid)
        {
            int sum  = 0;
            // Calculate lengths now for future reference
            int maxZ = oldGrid.GetLength(0);
            int maxY = oldGrid.GetLength(2);
            int maxX = oldGrid.GetLength(1);
            
            // Check whether a certain offset would be outside the old grid
            bool OutsideZ(int dz) => (z + dz < 1 || z + dz > maxZ);
            bool OutsideY(int dy) => (y + dy < 1 || y + dy > maxY);
            bool OutsideX(int dx) => (x + dx < 1 || x + dx > maxX);
            
            // Check all neighbours
            for (int dz = -1; dz <= 1; dz++)
            {
                if (OutsideZ(dz)) continue; // Outside of occupied grid
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (OutsideY(dy)) continue; // Outside of occupied grid
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        if (OutsideX(dx)) continue;                  // Outside of occupied grid
                        if (dx == 0 && dy == 0 && dz == 0) continue; // Home cell
                        if (oldGrid[z + dz - 1, x + dx - 1, y + dy - 1]) // Is this cell occupied?
                        {
                            sum++;
                            if (sum > 3) return false; // No matter what, a sum larger than 3 means the cell dies
                        }
                    }
                }
            }

            if (sum == 3) return true; // A cell with exactly 3 neighbours is always alive
            return sum == 2 
                   && !OutsideX(0) 
                   && !OutsideY(0) 
                   && !OutsideZ(0) 
                   && oldGrid[z - 1, x - 1, y - 1]; // A cell with two neighbours is alive if it was already alive
        }

        #endregion

        #region PartTwo

        // Dirty Duplicate Code
        private static void PartTwo()
        {
            int       dimensions = int.Parse(Console.ReadLine());
            bool[,,,] grid       = new bool[1, 1, dimensions, dimensions];
            for (int y = 0; y < dimensions; y++)
            {
                string line = Console.ReadLine();
                for (int x = 0; x < dimensions; x++)
                {
                    grid[0, 0, x, y] = line[x] == '#';
                }
            }

            for (int cycle = 0; cycle < 6; cycle++)
            {
                grid = Update4DGrid(grid);
            }

            int sum = grid.Cast<bool>().Count(b => b);
            Console.WriteLine($"Bootup completed: {sum} active cubes");
        }

        // Dirty Duplicate Code
        private static bool[,,,] Update4DGrid(bool[,,,] oldGrid)
        {
            int       wRange  = oldGrid.GetLength(0) + 2;
            int       zRange  = oldGrid.GetLength(1) + 2;
            int       xRange  = oldGrid.GetLength(2) + 2;
            int       yRange  = oldGrid.GetLength(3) + 2;
            bool[,,,] newGrid = new bool[wRange, zRange, xRange, yRange];

            for (int w = 0; w < wRange; w++)
            for (int z = 0; z < zRange; z++)
            {
                for (int y = 0; y < yRange; y++)
                {
                    for (int x = 0; x < xRange; x++)
                    {
                        newGrid[w, z, x, y] = Update4DCell(w, z, x, y, oldGrid);
                    }
                }
            }

            return newGrid;
        }

        // Dirty Duplicate Code
        private static bool Update4DCell(int w, int z, int x, int y, bool[,,,] oldGrid)
        {
            int sum  = 0;
            int maxW = oldGrid.GetLength(0);
            int maxZ = oldGrid.GetLength(1);
            int maxY = oldGrid.GetLength(3);
            int maxX = oldGrid.GetLength(2);
            bool OutsideW(int dw) => (w + dw < 1 || w + dw > maxW);
            bool OutsideZ(int dz) => (z + dz < 1 || z + dz > maxZ);
            bool OutsideY(int dy) => (y + dy < 1 || y + dy > maxY);
            bool OutsideX(int dx) => (x + dx < 1 || x + dx > maxX);
            for (int dw = -1; dw <= 1; dw++)
            {
                if (OutsideW(dw)) continue; // Outside of occupied grid
                for (int dz = -1; dz <= 1; dz++)
                {
                    if (OutsideZ(dz)) continue; // Outside of occupied grid
                    for (int dy = -1; dy <= 1; dy++)
                    {
                        if (OutsideY(dy)) continue; // Outside of occupied grid
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            if (OutsideX(dx)) continue;                             // Outside of occupied grid
                            if (dx == 0 && dy == 0 && dz == 0 && dw == 0) continue; // Home cell
                            if (oldGrid[w + dw - 1, z + dz - 1, x + dx - 1, y + dy - 1])
                            {
                                sum++;
                                if (sum > 3) return false; // No matter what, a sum larger than 3 means the cell dies
                            }
                        }
                    }
                }
            }

            if (sum == 3) return true;
            return sum == 2 && !OutsideX(0) && !OutsideY(0) && !OutsideZ(0) && !OutsideW(0) &&
                   oldGrid[w - 1, z - 1, x - 1, y - 1];
        }

        #endregion
    }
}