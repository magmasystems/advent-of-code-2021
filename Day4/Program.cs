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
            const int DIM = 5;
            
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            var numbers = input[0].Split(',').Select(s => Convert.ToInt32(s)).ToArray();
            
            // Build the boards from the list of input data
            var boards = new List<Board>();
            for (var i = 0; i < input.Length; i++)
            {
                if (i == 0 || input[i].Length == 0)
                    continue;

                var board = new Board();
                for (var j = i; j < i + DIM; j++)
                {
                    board.AddRow(j - i, input[j].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s)).ToArray());
                }

                boards.Add(board);
                i += DIM;
            }

            var sumForPart1 = 0;
            var lastSum = 0;
            var lastBingoNumber = 0;
            BitArray bingoBoards = new(boards.Count);

            // Call each number in the roller cage
            foreach (var number in numbers)
            {
                // Go through each board that doesn't have a bingo yet
                foreach (var board in boards.Where(b => !bingoBoards[b.Id]))
                {
                    // See if the number results in a bingo for the board
                    if (!board.CallNumber(number) || !board.IsBingo)
                        continue;
                    
                    // Capture the first bingo
                    if (sumForPart1 == 0)
                    {
                        sumForPart1 = board.SumOfUnmarkedSquares;
                        Console.WriteLine($"The last number is {number} and the sum of unmarked squares is {sumForPart1} and the product is {number * sumForPart1}");
                    }

                    // For Part 2, record the last bingo that was called
                    lastSum = board.SumOfUnmarkedSquares;
                    lastBingoNumber = number;
                    bingoBoards.Set(board.Id, true);
                }
            }
            
            Console.WriteLine($"The last number is {lastBingoNumber} and the sum of unmarked squares is {lastSum} and the product is {lastBingoNumber * lastSum}");
        }
    }
}
