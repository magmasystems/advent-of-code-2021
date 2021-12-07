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
            var values = input[0].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s)).ToArray();

            Dictionary<int, int> valueToCount = new();
            foreach (var value in values)
            {
                if (valueToCount.ContainsKey(value))
                    valueToCount[value]++;
                else
                    valueToCount[value] = 1;
            }
            var maxPos = valueToCount.Keys.Max();

            // Part 1
            var minFuel = ulong.MaxValue;
            
            for (var origin = 0; origin <= maxPos; origin++)
            {
                ulong totalFuel = 0L;
                foreach (var (key, count) in valueToCount)
                {
                    totalFuel += (ulong) Math.Abs((key - origin) * count);
                }

                if (totalFuel < minFuel)
                    minFuel = totalFuel;
            }
            
            Console.WriteLine($"Part 1: The minimum fuel is {minFuel}");
            
            // Part 2
            minFuel = ulong.MaxValue;

            for (var origin = 0; origin <= maxPos; origin++)
            {
                ulong totalFuel = 0L;
                foreach (var (key, count) in valueToCount)
                {
                    totalFuel += (ulong) MathUtils.Factorial(Math.Abs(key - origin)) * (ulong) count;
                }

                if (totalFuel < minFuel)
                {
                    minFuel = totalFuel;
                }
            }
            
            Console.WriteLine($"Part 2: The minimum fuel is {minFuel}");
        }
    }
}
