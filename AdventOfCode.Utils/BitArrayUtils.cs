using System.Collections;
using System.Text;

namespace AdventOfCode2021
{
    public static class BitArrayUtils
    {
        public static string Dump(this BitArray array)
        {
            StringBuilder sb = new();
            foreach (bool b in array)
                sb.Append(b ? "1" : "0");

            return sb.ToString();
        }
        
        public static int Sum(this BitArray array)
        {
            var sum = 0;
            foreach (bool b in array)
                sum += b ? 1 : 0;

            return sum;
        }
    }
}