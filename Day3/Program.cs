using System;
using System.IO;

// ReSharper disable once CheckNamespace
namespace AdventOfCode2021
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            // Assume all lines have the same number of columns
            var numColumns = input[0].Length;
            var numLines = input.Length;
            
            var sGamma = "";
            for (var col = 0; col < numColumns; col++)
            {
                var ones = 0;
                for (var row = 0; row < numLines; row++)
                {
                    ones += input[row][col] == '1' ? 1 : 0;
                }

                sGamma += ones >= numLines / 2 ? "1" : "0";
            }

            var gamma = 0;
            var multiplier = (int) Math.Pow(2, numColumns-1);
            foreach (var c in sGamma)
            {
                if (c == '1')
                    gamma += multiplier;
                multiplier >>= 1;
            }
            
            // numColumns^2 - 1 is the number you get if all columns were 1. If you take that and subtract the gamma, you get the epsilon.
            var epsilon = (int) Math.Pow(2, numColumns) - 1 - gamma;

            Console.WriteLine($"Gamma is {gamma}, Epsilon is {epsilon}, Multiplied is {gamma * epsilon}");
        }
    }
}