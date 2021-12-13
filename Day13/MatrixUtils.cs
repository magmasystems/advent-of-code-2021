namespace AdventOfCode2021;

public static class MatrixUtils
{
    public static void FoldedOr(this int[,] matrix, int yMax, int xMax, bool foldUp, int midPoint)
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