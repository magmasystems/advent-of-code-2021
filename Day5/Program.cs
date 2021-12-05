using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace AdventOfCode2021
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // 781,721 -> 781,611
            var regexp = new Regex(@"^(?<x1>\d+),(?<y1>\d+)\s+->\s+(?<x2>\d+),(?<y2>\d+)$", RegexOptions.Compiled);

            for (var part = 1; part <= 2; part++)
            {
                Dictionary<Point, int> map = new();
                var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
                foreach (var line in input)
                {
                    var matches = regexp.Matches(line);
                    var x1 = Convert.ToInt32(matches[0].Groups["x1"].Value);
                    var y1 = Convert.ToInt32(matches[0].Groups["y1"].Value);
                    var x2 = Convert.ToInt32(matches[0].Groups["x2"].Value);
                    var y2 = Convert.ToInt32(matches[0].Groups["y2"].Value);

                    if (x1 == x2)
                    {
                        var yMin = Math.Min(y1, y2);
                        var yMax = Math.Max(y1, y2);
                        while (yMin <= yMax)
                        {
                            Increment(map, new Point(x1, yMin++));
                        }
                    }
                    else if (y1 == y2)
                    {
                        var xMin = Math.Min(x1, x2);
                        var xMax = Math.Max(x1, x2);
                        while (xMin <= xMax)
                        {
                            Increment(map, new Point(xMin++, y1));
                        }
                    }
                    else if (part == 2)
                    {
                        var xIncr = x1 < x2 ? 1 : x1 > x2 ? -1 : 0;
                        var yIncr = y1 < y2 ? 1 : y1 > y2 ? -1 : 0;
                        var pt = new Point(x1, y1);
                        var ptEnd = new Point(x2, y2);
                        while (!pt.Equals(ptEnd))
                        {
                            Increment(map, pt);
                            pt.Offset(xIncr, yIncr);
                        }
                        Increment(map, pt);
                    }
                }

                var sum = map.Count(kvp => kvp.Value > 1);
                Console.WriteLine($"Part {part}: The number of safe points is {sum}");
                
                // Part 1: The number of safe points is 3990
                // Part 2: The number of safe points is 21305

            }
        }

        private static void Increment(IDictionary<Point, int> map, Point pt)
        {
            if (!map.ContainsKey(pt))
                map.Add(pt, 0);
            map[pt]++;
        }
    }
}
