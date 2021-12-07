using System.Collections.Generic;

namespace AdventOfCode2021
{
    public static class MathUtils
    {
        private static readonly Dictionary<int, int> Factorials = new();

        public static int Factorial(int n)
        {
            if (Factorials.TryGetValue(n, out var factorial))
                return factorial;

            var sum = 0;
            var m = n;
            while (m > 0)
                sum += m--;
            Factorials[n] = sum;
            return sum;
        }
    }
}