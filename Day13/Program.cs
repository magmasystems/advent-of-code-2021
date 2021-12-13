namespace AdventOfCode2021
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            var yMax = 0;
            var xMax = 0;

            foreach (var line in input)
            {
                if (line.Length == 0)
                    break;

                var parts = line.Split(',');
                yMax = Math.Max(yMax, Convert.ToInt32(parts[1]));
                xMax = Math.Max(xMax, Convert.ToInt32(parts[0]));
            }
            
            var matrix = new int[yMax+1, xMax+1];
            
            foreach (var line in input)
            {
                if (line.Length == 0)
                    continue;

                if (line.StartsWith("fold along "))
                {
                    var parts = line["fold along ".Length..].Split('=');
                    var axis = parts[0];
                    var number = Convert.ToInt32(parts[1]);
                    matrix.FoldedOr(yMax, xMax, axis == "y", number);
                    if (axis == "y")
                        yMax = number-1;
                    else
                        xMax = number-1;

                    // Should be 693
                    Console.WriteLine($"The number of hashes is {matrix.Sum(yMax, xMax)}");  // For Part 1, only consider the 1st fold
                    // matrix.Dump(yMax, xMax);
                }
                else
                {
                    var parts = line.Split(',');
                    var x = Convert.ToInt32(parts[0]);
                    var y = Convert.ToInt32(parts[1]);
                    matrix[y, x] = 1;
                }
            }

            // Should be UCLZRAZU
            //
            // X  X    XX    X      XXXX   XXX     XX    XXXX   X  X 
            // X  X   X  X   X         X   X  X   X  X      X   X  X 
            // X  X   X      X        X    X  X   X  X     X    X  X 
            // X  X   X      X       X     XXX    XXXX    X     X  X 
            // X  X   X  X   X      X      X X    X  X   X      X  X 
            //  XX     XX    XXXX   XXXX   X  X   X  X   XXXX    XX  

            matrix.Dump(yMax, xMax);
        }
    }
}
