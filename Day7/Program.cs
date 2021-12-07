using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace AdventOfCode2021
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            var positions = input[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s)).ToArray();

            // Create a dictionary of each position and how many crabs are at that position
            Dictionary<int, int> numCrabsAtPositions = new();
            foreach (var value in positions)
            {
                if (numCrabsAtPositions.ContainsKey(value))
                    numCrabsAtPositions[value]++;
                else
                    numCrabsAtPositions[value] = 1;
            }
            
            var maxPosition = numCrabsAtPositions.Keys.Max();
            var accumulators = new Func<int, int, int, ulong>[]
            {
                (key, count, origin) => (ulong) Math.Abs((key - origin) * count),                            // Part 1
                (key, count, origin) => (ulong) MathUtils.Factorial(Math.Abs(key - origin)) * (ulong) count  // Part 2
            };

            for (var part = 1; part <= 2; part++)
            {
                var minFuel = ulong.MaxValue;

                // Go through all of the possible positions, from 0 until the maximum position.
                // Accumulate the total fuel used from the origin until the crab position.
                for (var origin = 0; origin <= maxPosition; origin++)
                {
                    ulong totalFuel = 0L;
                    foreach (var (key, count) in numCrabsAtPositions)
                        totalFuel += accumulators[part - 1](key, count, origin);

                    if (totalFuel < minFuel)
                        minFuel = totalFuel;
                }

                Console.WriteLine($"Part {part}: The minimum fuel is {minFuel}");
            }

            // Part 1: The minimum fuel is 347509
            // Part 2: The minimum fuel is 98257206
        }
    }
}
