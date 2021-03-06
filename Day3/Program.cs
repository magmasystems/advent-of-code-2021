using System;
using System.Collections;
using System.Collections.Generic;
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

            var gamma = sGamma.FromBinaryString();
            
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
                var mask = GetRowsWithZerosAndOnes(oxy, col, out var numOnes, out var numZeros);
                RemoveRowsUsingIndexes(oxy, mask, numOnes < numZeros);

                if (oxy.Count == 1)
                {
                    oxyRating = oxy[0].FromBinaryString();
                    break;
                }
            }
            
            for (var col = 0; col < numColumns; col++)
            {
                var mask = GetRowsWithZerosAndOnes(co2, col, out var numOnes, out var numZeros);
                
                // Determine the least common value (0 or 1) in the current bit position, and keep only numbers with that bit in that position. 
                RemoveRowsUsingIndexes(co2, mask, numZeros <= numOnes);

                if (co2.Count == 1)
                {
                    co2Rating = co2[0].FromBinaryString();
                    break;
                }
            }
            
            // Oxy Scrubbing Rating is 1599, CO2 Scrubbing Rating is 3716, Multiplied is 5941884
            Console.WriteLine($"Oxy Scrubbing Rating is {oxyRating}, CO2 Scrubbing Rating is {co2Rating}, Multiplied is {oxyRating * co2Rating}");
        }

        private static BitArray GetRowsWithZerosAndOnes(IReadOnlyList<string> s, int col, out int numOnes, out int numZeros)
        {
            var mask  = new BitArray(s.Count);
            numOnes = 0;
            numZeros = 0;
            
            for (var row = 0; row < s.Count; row++)
            {
                if (s[row][col] == '1')
                {
                    mask[row] = true;
                    numOnes++;
                }
                else
                {
                    mask[row] = false;
                    numZeros++; 
                }
            }

            return mask;
        }

        private static void RemoveRowsUsingIndexes(IList s, BitArray mask, bool valuesToRemove)
        {
            for (var i = mask.Count - 1;  i >= 0;  i--)
            {
                if (mask[i] == valuesToRemove)
                    s.RemoveAt(i);
            }
        }
    }
}