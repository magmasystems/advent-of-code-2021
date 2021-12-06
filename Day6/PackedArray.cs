using System.Collections.Generic;

namespace AdventOfCode2021
{
    public class PackedArray
    {
        private int MaxIndex = -1;
        public int Length => this.MaxIndex + 1;

        private readonly List<ulong> InternalList = new(400000/16);
        
        private void EnsureSize(int idx)
        {
            while (idx / 16 >= this.InternalList.Count)
                this.InternalList.Add(0);
        }

        public int GetValue(int idx)
        {
            this.EnsureSize(idx);
            
            var shiftAmt = idx % 16 * 4;
            var mask = (ulong) 0x0FL << shiftAmt;
            var value = (this.InternalList[idx / 16] & mask) >> shiftAmt;
            return (int) value;
        }
        
        public void SetValue(int idx, int n)
        {
            if (idx > this.MaxIndex)
                this.MaxIndex = idx;
            
            this.EnsureSize(idx);
            
            var shiftAmt = idx % 16 * 4;
            var mask = 0xFFFFFFFFFFFFFFFFL ^ (ulong) (0xFL << shiftAmt);
            this.InternalList[idx / 16] = (this.InternalList[idx / 16] & mask) | ((ulong) n << shiftAmt);
            
            // Console.WriteLine($"index {idx:00}, value {n}, shiftAmt {shiftAmt:00}, mask {mask:X}, this[idx/16] {this[idx/16]:X}");
        }
        
        public void AppendValues(PackedArray newbitFishes, int numNewFishes)
        {
            for (var i = 0; i < numNewFishes; i++)
            {
                var val = newbitFishes.GetValue(i);
                this.SetValue(this.MaxIndex + 1, val);
            }
        }
        
        public void AppendValue(int n)
        {
            this.SetValue(this.Length, n);
        }
    }
}