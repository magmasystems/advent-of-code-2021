using System;
using System.IO;

namespace AdventOfCode2021
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            int x = 0, y = 0;

            foreach (var line in input)
            {
                var parts = line.Split(' ');
                var n = Convert.ToInt32(parts[1]);
                
                switch (parts[0].ToLower())
                {
                    case "forward":
                        x += n;
                        break;
                    case "up":
                        y -= n;
                        break;
                    case "down":
                        y += n;
                        break;
                }
            }
            
            Console.WriteLine($"Depth is {y} and Horizontal position is {x} and multiplied is {x * y}.");
        }
    }
}
