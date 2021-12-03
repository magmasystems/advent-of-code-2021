using System;
using System.Collections;
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

            var gamma = FromBinaryString(sGamma);
            
            // numColumns^2 - 1 is the number you get if all columns were 1. If you take that and subtract the gamma, you get the epsilon.
            var epsilon = (int) Math.Pow(2, numColumns) - 1 - gamma;

            Console.WriteLine($"Gamma is {gamma}, Epsilon is {epsilon}, Multiplied is {gamma * epsilon}");
            
            // PART 2

            List<string> oxy = new(input);
            List<string> co2 = new(input);
            var oxyRating = 0;
            var co2Rating = 0;
            
            for (var col = 0; col < numColumns; col++)
            {
                GetRowsWithZerosAndOnes(oxy, col, out IList<int> rowsWithOnes, out IList<int> rowsWithZeros);
                RemoveRowsUsingIndexes(oxy, rowsWithOnes.Count >= rowsWithZeros.Count ? rowsWithZeros : rowsWithOnes);

                if (oxy.Count == 1)
                {
                    oxyRating = FromBinaryString(oxy[0]);
                    break;
                }
            }
            
            for (var col = 0; col < numColumns; col++)
            {
                GetRowsWithZerosAndOnes(co2, col, out IList<int> rowsWithOnes, out IList<int> rowsWithZeros);
                // Determine the least common value (0 or 1) in the current bit position, and keep only numbers with that bit in that position. 
                RemoveRowsUsingIndexes(co2, rowsWithZeros.Count > rowsWithOnes.Count ? rowsWithZeros : rowsWithOnes);

                if (co2.Count == 1)
                {
                    co2Rating = FromBinaryString(co2[0]);
                    break;
                }
            }
            
            // Oxy Scrubbing Rating is 1599, CO2 Scrubbing Rating is 3716, Multiplied is 5941884
            Console.WriteLine($"Oxy Scrubbing Rating is {oxyRating}, CO2 Scrubbing Rating is {co2Rating}, Multiplied is {oxyRating * co2Rating}");
        }

        private static void GetRowsWithZerosAndOnes(IReadOnlyList<string> s, int col, out IList<int> rowsWithOnes, out IList<int> rowsWithZeros)
        {
            rowsWithOnes  = new List<int>();
            rowsWithZeros = new List<int>();
            for (var row = 0; row < s.Count; row++)
            {
                if (s[row][col] == '1')
                {
                    rowsWithOnes = rowsWithOnes.Append(row).ToList();
                }
                else
                {
                    rowsWithZeros = rowsWithZeros.Append(row).ToList();
                }
            }
        }

        private static void RemoveRowsUsingIndexes(IList s, IList<int> indexes)
        {
            for (var i = indexes.Count - 1;  i >= 0;  i--)
            {
                s.RemoveAt(indexes[i]);
            }
        }

        private static int FromBinaryString(string s)
        {
            var n = 0;
            var multiplier = (int) Math.Pow(2, s.Length-1);
            
            foreach (var c in s)
            {
                if (c == '1')
                    n += multiplier;
                multiplier >>= 1;
            }

            return n;
        }
    }
}