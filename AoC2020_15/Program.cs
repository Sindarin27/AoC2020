using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AoC2020_15
{
    class Program
    {
        static void Main(string[] args)
        {
            // Running the seven 30000000 tests can take a while, so let's ask the user first
            Console.WriteLine("Do you want to run tests? [Y/N]");
            if (Console.ReadLine() == "Y") Test();
            
            // Run the damn thing
            string line = Console.ReadLine();
            Console.WriteLine($"Number 2020 is {PlayGame(line)}");
            Console.WriteLine($"Number 30000000 is {PlayGame(line, 30000000)}");
        }

        // Run all examples and assert they are correct
        private static void Test()
        {
            Debug.Assert(PlayGame("0,3,6") == 436);
            Debug.Assert(PlayGame("1,3,2") == 1);
            Debug.Assert(PlayGame("2,1,3") == 10);
            Debug.Assert(PlayGame("1,2,3") == 27);
            Debug.Assert(PlayGame("2,3,1") == 78);
            Debug.Assert(PlayGame("3,2,1") == 438);
            Debug.Assert(PlayGame("3,1,2") == 1836);
            Debug.Assert(PlayGame("0,3,6", 30000000) == 175594);
            Debug.Assert(PlayGame("1,3,2", 30000000) == 2578);
            Debug.Assert(PlayGame("2,1,3", 30000000) == 3544142);
            Debug.Assert(PlayGame("1,2,3", 30000000) == 261214);
            Debug.Assert(PlayGame("2,3,1", 30000000) == 6895259);
            Debug.Assert(PlayGame("3,2,1", 30000000) == 18);
            Debug.Assert(PlayGame("3,1,2", 30000000) == 362);
            Console.WriteLine("Tests done");
        }

        private static int PlayGame(string line, int limit = 2020)
        {
            MemoryGame game = new MemoryGame();
            // Start with the initial few numbers
            string[] input = line.Split(',');
            for (int i = 0; i < input.Length; i++) game.CallNumberDirectly(int.Parse(input[i]));
            
            // Play the rest of the game
            while (game.turn < limit)
            {
                game.AdvanceTurn();
            }

            // Return the last number
            return game.nextNumber;
        }
    }

    // The inner workings of a game
    internal class MemoryGame
    {
        // Every number and the turn they were last called
        private readonly Dictionary<int, int> numberLastTurnCalled = new Dictionary<int, int>();
        // The current turn number
        public int turn = 1;
        // The next number to be called
        public int nextNumber = 0;

        // Call number directly
        public void CallNumberDirectly(int number)
        {
            turn++;
            // New number? Next we call 0
            if (numberLastTurnCalled.TryAdd(number, turn))
            {
                nextNumber = 0;
            }
            // Old number? Calculate the age and update the last called
            else
            {
                int age = turn - numberLastTurnCalled[number];
                numberLastTurnCalled[number] = turn;
                nextNumber = age;
            }
        }
        
        // Call the next number
        public void AdvanceTurn()
        {
            CallNumberDirectly(nextNumber);
        }
    }
}