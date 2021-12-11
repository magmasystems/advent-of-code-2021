namespace AdventOfCode2021
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            var octopi = new int[input.Length, input[0].Length];

            for (var row = 0; row < input.Length; row++)
            {
                var line = input[row];
                for (var col = 0; col < line.Length; col++)
                {
                    octopi[row, col] = line[col] - '0';
                }
            }

            var sum = 0;
            var firstStepWithAllFlashes = 0;

            for (var time = 1; time <= 1000; time++)
            {
                var flashed = new bool[octopi.GetLength(0), octopi.GetLength(1)];
                Pulse(octopi, flashed);

                var numFlashes = CountFlashes(octopi, flashed);
                if (numFlashes == octopi.GetLength(0) * octopi.GetLength(1) && firstStepWithAllFlashes == 0)
                    firstStepWithAllFlashes = time;

                sum += numFlashes;
                if (time == 100)
                    Console.WriteLine($"Part 1: The number of flashes is {sum}");
            }

            Console.WriteLine($"Part 2: The first step where everything flashed is {firstStepWithAllFlashes}");
        }

        private static void Pulse(int[,] octopi, bool[,] flashed)
        {
            for (var row = 0; row < octopi.GetLength(0); row++)
            {
                for (var col = 0; col < octopi.GetLength(1); col++)
                {
                    if (!flashed[row, col] && ++octopi[row, col] > 9)
                    {
                        FlashOne(octopi, row, col, flashed);
                    }
                }
            }
        }
        
        private static int CountFlashes(int[,] octopi, bool[,] flashed)
        {
            var flashes = 0;
            
            for (var row = 0; row < octopi.GetLength(0); row++)
            {
                for (var col = 0; col < octopi.GetLength(1); col++)
                {
                    if (flashed[row, col])
                    {
                        flashes++;
                        octopi[row, col] = 0;
                    }
                }
            }

            return flashes;
        }

        private static void FlashOne(int[,] octopi, int row, int col, bool[,] flashed)
        {
            if (row < 0 || col < 0 || row >= octopi.GetLength(0) || col >= octopi.GetLength(1))
                return;
            if (flashed[row, col])
                return;
            if (++octopi[row, col] <= 9)
                return;
            
            flashed[row, col] = true;

            FlashOne(octopi, row - 1, col + 0, flashed);  // up
            FlashOne(octopi, row + 1, col + 0, flashed);  // down
            FlashOne(octopi, row + 0, col - 1, flashed);  // left
            FlashOne(octopi, row + 0, col + 1, flashed);  // right
            FlashOne(octopi, row - 1, col - 1, flashed);  // upper left
            FlashOne(octopi, row - 1, col + 1, flashed);  // upper right
            FlashOne(octopi, row + 1, col - 1, flashed);  // lower left
            FlashOne(octopi, row + 1, col + 1, flashed);  // lower right
        }
    }
}
