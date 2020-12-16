using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AoC2020_16
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Field> fieldList = new List<Field>();

            string line = Console.ReadLine();
            // Read all fields
            while (!string.IsNullOrEmpty(line))
            {
                // Example match: "departure location: 1-10 or 64-128"
                Match collection = Regex.Match(line,
                    "^(?<name>.*?): (?<min1>\\d*?)-(?<max1>\\d*?) or (?<min2>\\d*?)-(?<max2>\\d*?)$");
                // Create and add field
                fieldList.Add(new Field(
                    collection.Groups["name"].Value,
                    int.Parse(collection.Groups["min1"].Value),
                    int.Parse(collection.Groups["max1"].Value),
                    int.Parse(collection.Groups["min2"].Value),
                    int.Parse(collection.Groups["max2"].Value)
                ));
                
                line = Console.ReadLine();
            }

            // Arrays are easier to index. Do we even index anymore? Who cares.
            Field[] fields = fieldList.ToArray();

            Console.ReadLine(); // "your ticket"
            Ticket myTicket = Ticket.ReadTicket(Console.ReadLine()); // Our ticket

            Console.ReadLine(); // ""

            Console.ReadLine(); // "nearby tickets"
            List<Ticket> validTickets = new List<Ticket>(); // Keep a list of only the valid tickets
            int invalidSum = 0; // Keep the sum of all invalid tickets
            line = Console.ReadLine(); // Read the first ticket
            while (!string.IsNullOrEmpty(line))
            {
                Ticket ticket     = Ticket.ReadTicket(line); // Read ticket from line
                int    invalidity = ticket.InvalidSum(fields); // Check how invalid it is
                invalidSum += invalidity; // Add
                if (invalidity == 0) validTickets.Add(ticket); // Not invalid? Then it's valid.
                
                line = Console.ReadLine();
            }
            // Answer to part 1
            Console.WriteLine($"Ticket scanning error rate = {invalidSum}");

            Dictionary<int, Field> indexToField = new Dictionary<int, Field>();
            // Find all the fields
            // This is probably overkill because AoC is often kind, but I'm doing it anyway
            Queue<Field> fieldsToCheck  = new Queue<Field>(fields);
            while (fieldsToCheck.Count > 0)
            {
                // Try finding this field next
                Field findNext = fieldsToCheck.Dequeue();
                // Keep a list of all possible indices this field might be on
                List<int> possibleIndices = new List<int>();
                for (int i = 0; i < fields.Length; i++)
                {
                    if (indexToField.ContainsKey(i)) continue; // This index is already taken, we can skip it
                    if (validTickets.Any(t => !t.FieldValidOn(i, findNext))) continue; // Field doesn't fit here
                    possibleIndices.Add(i); // This is a possible index for this number
                }

                if (possibleIndices.Count > 1)
                {
                    // This field may be in multiple places
                    // Finish some other fields first, then try again
                    fieldsToCheck.Enqueue(findNext);
                }
                else if (possibleIndices.Count == 0)
                {
                    // Uh oh.
                    throw new Exception($"No possible index for field {findNext.name}");
                }
                else
                {
                    // We found it!
                    indexToField.Add(possibleIndices.First(), findNext);
                }
            }

            long multiply = 1; // The departure parts on our ticket multiplied
            foreach (var (index, field) in indexToField)
            {
                Console.WriteLine($"Field {field.name} is on index {index}");
                if (field.name.StartsWith("departure "))
                {
                    multiply *= myTicket.fields[index];
                }
            }

            Console.WriteLine($"Our ticket multiplied gives {multiply}");


        }
    }

    internal class Field
    {
        public string name;
        private readonly int min1;
        private readonly int max1;
        private readonly int min2;
        private readonly int max2;

        public Field(string name, int min1, int max1, int min2, int max2)
        {
            this.name = name;
            this.min1 = min1;
            this.max1 = max1;
            this.min2 = min2;
            this.max2 = max2;
        }

        // Check whether the value is in either the first or second range
        public bool CheckValue(int value)
        {
            return (value >= min1 && value <= max1) || (value >= min2 && value <= max2);
        }
    }

    internal class Ticket
    {
        // Values of the fields on this ticket
        public readonly int[] fields;

        public Ticket(int[] fields)
        {
            this.fields = fields;
        }

        // Read a ticket from string
        public static Ticket ReadTicket(string ticket)
        {
            string[] parts  = ticket.Split(',');
            int[]    fields = new int[parts.Length];

            for (int i = 0; i < parts.Length; i++) fields[i] = int.Parse(parts[i]);

            return new Ticket(fields);
        }

        // Calculate the invalidity sum of this ticket
        public int InvalidSum(Field[] check)
        {
            return fields.Where((t, _) => StrictlyInvalid(t, check)).Sum();
        }

        // Is this value invalid independent of the field?
        private static bool StrictlyInvalid(int value, Field[] check)
        {
            return check.AsParallel().All(field => !field.CheckValue(value));
        }

        // Is this field valid on the specified index?
        public bool FieldValidOn(int index, Field field)
        {
            return field.CheckValue(fields[index]);
        }
    }
}