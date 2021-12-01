using System;
using System.IO;

namespace AdventOfCode2021
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            var numIncreasing = 0;
            var lastValue = Int32.MinValue;
            
            foreach (var line in input)
            {
                int n = Convert.ToInt32(line);
                if (lastValue != Int32.MinValue && n > lastValue)
                    numIncreasing++;
                lastValue = n;
            }

            Console.WriteLine($"The number of increasing values is {numIncreasing}");
        }
    }
}