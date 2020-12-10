using System;
using System.Collections.Generic;

namespace AoC2020_10
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> adapters = new List<int>(); // Adding to a list is faster than adding to an array
            string    line     = Console.ReadLine();
            while (!string.IsNullOrEmpty(line))
            {
                adapters.Add(int.Parse(line));
                line = Console.ReadLine();
            }

            adapters.Sort(); // That's easy.
            int[] adaptersArray = adapters.ToArray(); // Indexing on an array is faster than a list
            int[] differences = CountDifferences(adaptersArray);

            Console.WriteLine($"{differences[1]} * {differences[3]} = {differences[1] * differences[3]}");
            Console.WriteLine($"We can make {FindPossibleConnections(adaptersArray)} possible connections.");
        }

        private static int[] CountDifferences(int[] adaptersArray)
        {
            int[] differences = new int[4]; // Difference can be 0, 1, 2 or 3
            for (int i = 0; i < adaptersArray.Length; i++)
            {
                int current  = adaptersArray[i];
                int previous = i == 0 ? 0 : adaptersArray[i - 1];
                differences[current - previous]++;
            }

            differences[3]++; // Step from last adapter to device

            return differences;
        }

        private static long FindPossibleConnections(int[] adaptersArray)
        {
            long[] connectionsPerAdapter = new long[adaptersArray.Length];
            
            // Backtrack. The last adapter can only connect to the laptop.
            // Every other adapter can make a connection to all adapters within 3 steps of them.
            // If an adapter can make a connection, it can connect via that adapter in as many ways as that adapter by itself can.
            connectionsPerAdapter[^1] = 1;
            for (int i = connectionsPerAdapter.Length - 2; i >= 0; i--)
            {
                connectionsPerAdapter[i] = FindConnectionsFor(i, adaptersArray, connectionsPerAdapter);
            }

            return FindConnectionsFor(-1, adaptersArray, connectionsPerAdapter); // The final possibility is in the socket itself
        }

        private static long FindConnectionsFor(int index, int[] adaptersArray, long[] connectionsPerAdapter)
        {
            int adapter     = index >= 0 ? adaptersArray[index] : 0;
            long connections = 0;
            for (int j = 1; j <= 3; j++)
            {
                if (index + j >= adaptersArray.Length) break; // End of array reached
                if (adaptersArray[index + j] - adapter <= 3) // Check if in range
                {
                    connections += connectionsPerAdapter[index + j];
                }
            }
            return connections;
        }
    }
}