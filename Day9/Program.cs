// ReSharper disable once CheckNamespace

using System.Collections;
using System.Drawing;

namespace AdventOfCode2021
{
    internal static class Program
    {
        class RiskLevel
        {
            public Point Coordinate { get; }
            public int Level { get; }

            public RiskLevel(Point coordinate, int level)
            {
                Coordinate = coordinate;
                Level = level;
            }
        }
        
        private static void Main(string[] args)
        {
            // PART 1
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            var numRows = input.Length;
            var numCols = input[0].Length;
            var heightmap = new int[numRows, numCols];
            
            for (var row = 0; row < numRows;  row++)
            {
                for (var col = 0; col < numCols; col++)
                {
                    heightmap[row, col] = input[row][col] - '0';
                }
            }

            var riskLevels = new List<RiskLevel>();
            
            for (var row = 0; row < numRows;  row++)
            {
                for (var col = 0; col < numCols; col++)
                {
                    var origin = heightmap[row, col];
                    var x1 = col > 0         ? heightmap[row, col - 1] : int.MaxValue;
                    var x2 = col < numCols-1 ? heightmap[row, col + 1] : int.MaxValue;
                    var y1 = row > 0         ? heightmap[row-1, col]   : int.MaxValue;
                    var y2 = row < numRows-1 ? heightmap[row+1, col]   : int.MaxValue;
                    
                    if (origin < x1 && origin < x2 && origin < y1 && origin < y2)
                        riskLevels.Add(new RiskLevel(new Point(col, row), origin));
                }
            }

            // Final answer - 548
            Console.WriteLine($"Part 1: The sum of the low points is {riskLevels.Sum(r => r.Level + 1)}");
            
            // Part 2
            var basinSizes = (from r in riskLevels 
                let visited = new bool[numRows, numCols] 
                select GetBasinSize(heightmap, r.Level, r.Coordinate.X, r.Coordinate.Y, r.Coordinate.X, r.Coordinate.Y, visited)).ToList();

            basinSizes.Sort();
            var last3BasinSizes = basinSizes.Skip(basinSizes.Count - 3).ToArray();
           
            // Final answer - 786048
            Console.WriteLine($"Part 2: The product of the largest 3 basin sizes is {last3BasinSizes[0] * last3BasinSizes[1] * last3BasinSizes[2]}");
            
        }

        private static int GetBasinSize(int[,] heightmap, int level, int x, int y, int origX, int origY, bool[,] visited)
        {
            // Check for out-of-bounds
            if (x < 0 || x >= heightmap.GetLength(1) || y < 0 || y >= heightmap.GetLength(0))
                return 0;
            
            // If we visited this cell already, return
            if (visited[y,x])
                return 0;

            // Mark the cell as visited
            visited[y,x] = true;
            var value = heightmap[y, x];
            
            // Console.WriteLine($"level {level}, value {value}, y/x {y}/{x}, origin {origY}/{origX}");
            
            // According to the rules, ignore cells with the value 9
            if (value == 9)
                return 0;
            
            // If we are not at the origin, and the value of the cell is lower or equal to the level, then it's not part of the basin
            var isOrigin = x == origX && y == origY;
            if (!isOrigin && value <= level)
                return 0;

            // Check all of the surrounding cells
            var total = 1;
            total += GetBasinSize(heightmap, level, x-1, y, origX, origY, visited);
            total += GetBasinSize(heightmap, level, x+1, y, origX, origY, visited);
            total += GetBasinSize(heightmap, level, x, y-1, origX, origY, visited);
            total += GetBasinSize(heightmap, level, x, y+1, origX, origY, visited);

            return total;
        }
    }
}
