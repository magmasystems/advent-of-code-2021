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
            int[] NUMDAYS = { 80, 256 };
            
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            // The input for this puzzle is on a single line. It's a list of numbers, separated by commas.
            var values = input[0].Split(',', StringSplitOptions.RemoveEmptyEntries);

            for (var part = 0; part < 2; part++)
            {
                ulong[] counts = new ulong[9];
                foreach (var t in values)
                {
                    counts[Convert.ToInt32(t)]++;
                }

                for (var day = 1; day <= NUMDAYS[part]; day++)
                {
                    ulong[] newCounts = new ulong[9];
                    for (var i = 8; i >= 0; i--)
                    {
                        newCounts[i] = counts[i == 8 ? 0 : i+1];
                    }
                    newCounts[6] += counts[0];
                    Array.Copy(newCounts, counts, 9);
                }

                ulong sum = 0L;
                foreach (var value in counts)
                {
                    sum += value;
                }

                // Part 2: After 80 days, there are 394994 fish
                // Part 2: After 256 days, there are 1765974267455 fish
                Console.WriteLine($"Part {part+1}: After {NUMDAYS[part]} days, there are {sum} fish");
            }
            
            #if PART1_IS_OK_BUT_NOT_OPTIMAL
            var fishes = Enumerable.Range(0, values.Length).Select(i => new Fish(Convert.ToInt32(values[i]))).ToList();
            for (var day = 1; day <= NUMDAYS[0]; day++)
            {
                List<Fish> newFishes = new();
                foreach (var fish in fishes)
                {
                    if (fish.Subtract())
                    {
                        newFishes.Add(new Fish(8));
                    }
                }

                fishes.AddRange(newFishes);
                //Console.WriteLine($"Part {1}: After {day} days, there are {fishes.Count} fish and there are {newFishes.Count} new fish");
            }
            Console.WriteLine($"Part {1}: After {NUMDAYS[0]} days, there are {fishes.Count} fish");
            #endif

            #if PART_2_TAKES_TOO_LONG  // This works up until around 130 days, then really slows down
            // A 64-bit uint holds 16 groups of 4-bit integers, where each 4-bit int can be at most 8
            var numFishes = values.Length;
            PackedArray bitFish = new();
            for (var i = 0; i < numFishes; i++)
            {
                var n = (byte) Convert.ToInt32(values[i]);
                bitFish.SetValue(i, n);
            }
            
            for (var day = 1; day <= 130; day++)
            {
                PackedArray newbitFishes = new();
                var numNew = 0;
                var numFish = bitFish.Length;
                for (var i = 0; i < numFish; i++)
                {
                    var n = bitFish.GetValue(i);
                    if (--n < 0)
                    {
                        newbitFishes.AppendValue(8);
                        numNew++;
                        bitFish.SetValue(i, 6);
                    }
                    else
                    {
                        bitFish.SetValue(i, n);
                    }
                }
                
                bitFish.AppendValues(newbitFishes, numNew);
                
                Console.WriteLine($"Part {2}: After {day} days, there are {bitFish.Length} fish");
            }

            Console.WriteLine($"Part {2}: After {NUMDAYS[1]} days, there are {bitFish.Length} fish");
            #endif
        }
    }
}
