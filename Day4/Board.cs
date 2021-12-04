using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace AdventOfCode2021
{
    public class Board
    {
        private class BoardCell
        {
            public readonly int Row;
            public readonly int Col;
            public bool Marked;

            public BoardCell(int row, int col)
            {
                Row = row;
                Col = col;
            }
        }
        
        private const int DIM = 5;
        private static int BOARD_ID;

        public int Id { get; }

        // This is used to test membership of a number within the matrix. It holds the position of the number.
        private readonly Dictionary<int, BoardCell> BoardCells = new();

        private List<BitArray> RowMatches { get; } = new(DIM);
        private List<BitArray> ColumnMatches { get; } = new(DIM);
        private BitArray LeftDiagonalMatches { get; } = new(DIM);  // 0,0 1,1 2,2 3,3 4,4
        private BitArray RightDiagonalMatches { get; } = new(DIM); // 0,4 1,3 2,2 3,1 4,0
        
        public Board()
        {
            this.Id = BOARD_ID++;
            foreach (var _ in Enumerable.Range(0, DIM))
            {
                this.RowMatches.Add(new BitArray(DIM));
                this.ColumnMatches.Add(new BitArray(DIM));
            }
        }
        
        public void AddRow(int idxRow, int[] nums)
        {
            for (var i = 0; i < nums.Length; i++)
            {
                this.BoardCells.Add(nums[i], new BoardCell(idxRow, i));
            }
        }

        public bool CallNumber(int number)
        {
            if (!this.BoardCells.TryGetValue(number, out var cell))
                return false;

            var row = cell.Row;
            var col = cell.Col;

            cell.Marked = true;
            this.RowMatches[row][col] = true;
            this.ColumnMatches[col][row] = true;
            
            if (row == col)
            {
                this.LeftDiagonalMatches[row] = true;
            }

            if (row + col == DIM - 1)
            {
                this.RightDiagonalMatches[col] = true;
            }

            return true;
        }

        public bool IsBingo
        {
            get
            {
                for (var row = 0; row < DIM; row++)
                {
                    if (RowMatches[row].Sum() == DIM)
                        return true;
                }

                for (var col = 0; col < DIM; col++)
                {
                    if (ColumnMatches[col].Sum() == DIM)
                        return true;
                }

                if (this.LeftDiagonalMatches.Sum() == DIM)
                    return true;
                return this.RightDiagonalMatches.Sum() == DIM;
            }
        }

        public int SumOfUnmarkedSquares
        {
            get
            {
                return this.BoardCells.Where(kvp => !kvp.Value.Marked).Sum(kvp => kvp.Key);
            }
        }

        public bool Dump()
        {
            Console.WriteLine($"Board {Id}");
            for (var row = 0; row < DIM; row++)
            {
                Console.WriteLine($"RowMatches: {string.Join("", RowMatches[row].Dump())}");
            }

            for (var col = 0; col < DIM; col++)
            {
                Console.WriteLine($"ColMatches: {string.Join("", ColumnMatches[col].Dump())}");
            }
            
            Console.WriteLine($"LeftDiagMatches: {string.Join("", LeftDiagonalMatches.Dump())}");
            Console.WriteLine($"RightDiagMatches: {string.Join("", RightDiagonalMatches.Dump())}");
            Console.WriteLine();

            return true;
        }
    }
}