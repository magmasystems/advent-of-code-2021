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
                {
                    continue;
                }

                if (line.StartsWith("fold along "))
                {
                    var direction = line["fold along ".Length..];
                    var parts = direction.Split('=');
                    var axis = parts[0];
                    var number = Convert.ToInt32(parts[1]);
                    Fold(matrix, yMax, xMax, axis, number);
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

        private static void Fold(int[,] matrix, int yMax, int xMax, string axis, int number)
        {
            matrix.Or(yMax, xMax, axis == "y", number);
        }
    }
    
    public static class MatrixUtils
    {
        public static void Or(this int[,] matrix, int yMax, int xMax, bool foldUp, int midPoint)
        {
            var pointer1 = midPoint-1;
            var pointer2 = midPoint+1;

            while (true)
            {
                if (foldUp)
                {
                    // if we have 15 rows, and we fold at 7, then 14->0, 13->1, 12->2, 11->3, 10->4, 9->5, 8->6
                    for (var col = 0; col <= xMax; col++)
                        matrix[pointer1, col] |= matrix[pointer2, col];
                    if (--pointer1 < 0)
                        break;
                    if (++pointer2 > yMax)
                        break;
                }
                else
                {
                    for (var row = 0; row <= yMax; row++)
                        matrix[row, pointer1] |= matrix[row, pointer2];
                    if (--pointer1 < 0)
                        break;
                    if (++pointer2 > xMax)
                        break;
                }
            }
        }

        public static int Sum(this int[,] matrix, int yMax, int xMax)
        {
            var sum = 0;
            
            for (var row = 0;  row <= yMax;  row++)
                for (var col = 0; col <= xMax; col++) 
                    sum += matrix[row, col];
            
            return sum;
        }
        
        public static void Dump(this int[,] matrix, int yMax, int xMax)
        {
            var letterLength = (xMax+1) / 8;
            
            for (var row = 0; row <= yMax; row++)
            {
                for (var col = 0; col <= xMax; col++)
                {
                    if (letterLength > 0 && col % letterLength == 0)
                        Console.Write("  ");
                    Console.Write(matrix[row, col] == 0 ? " " : "X");
                }

                Console.WriteLine();
            }
        }
    }
}
