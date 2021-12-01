using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2021
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // This is part 1
            
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            var numIncreasing = 0;
            var lastValue = int.MinValue;
            
            foreach (var line in input)
            {
                var n = Convert.ToInt32(line);
                if (lastValue != int.MinValue && n > lastValue)
                    numIncreasing++;
                lastValue = n;
            }

            Console.WriteLine($"Part 1: The number of increasing values is {numIncreasing}");
            
            // This is part 2
            // I used a circular queue with a callback that computes the sum before a new element is enqueued.
            // In order to "flush" the last entry, I just added a "0" entry to the list of lines.

            numIncreasing = 0;
            lastValue = int.MinValue;
            CircularQueue<int> circularQueue = new(3);
            
            foreach (var line in input.Append("0"))
            {
                var n = Convert.ToInt32(line);
                circularQueue.Enqueue(n, (queue) =>
                {
                    var sum = queue.Sum();
                    if (lastValue != int.MinValue && sum > lastValue)
                        numIncreasing++;
                    lastValue = sum;
                });
            }
            
            Console.WriteLine($"Part 2a: The number of increasing values is {numIncreasing}");
            
            // This is part 2 solved with a more traditional C# Queue.
            numIncreasing = 0;
            lastValue = int.MinValue;
            Queue<int> queue = new(4);
            
            foreach (var line in input)
            {
                var n = Convert.ToInt32(line);

                queue.Enqueue(n);
                if (queue.Count > 3)
                    queue.Dequeue();
                if (queue.Count == 3)
                {
                    var sum = queue.Sum();
                    if (lastValue != int.MinValue && sum > lastValue)
                        numIncreasing++;
                    lastValue = sum;
                }
            }
            
            Console.WriteLine($"Part 2b: The number of increasing values is {numIncreasing}");
        }
    }
}