using System;

namespace AdventOfCode2021
{
    public static class StringUtils
    {
        public static int FromBinaryString(this string s)
        {
            var n = 0;
            var multiplier = (int) Math.Pow(2, s.Length-1);
            
            foreach (var c in s)
            {
                if (c == '1')
                    n += multiplier;
                multiplier >>= 1;
            }

            return n;
        }
    }
}