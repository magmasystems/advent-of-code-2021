using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021
{
    public class Board
    {
        private const int DIM = 5;
        private static int BOARD_ID;

        public int Id { get; }
        private int[,] Matrix { get; } = new int[DIM, DIM];

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
                this.Matrix[idxRow, i] = nums[i];
        }

        public bool CallNumber(int number)
        {
            for (var row = 0; row < DIM; row++)
            {
                for (var col = 0; col < DIM; col++)
                {
                    if (number == this.Matrix[row, col])
                    {
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
                }
            }

            return false;
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
                var sum = 0;
                for (var row = 0; row < DIM; row++)
                {
                    for (var col = 0; col < DIM; col++)
                    {
                        if (this.RowMatches[row][col] == false)
                            sum += this.Matrix[row, col];
                    }
                }

                return sum;
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