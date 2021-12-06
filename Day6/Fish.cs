namespace AdventOfCode2021
{
    public class Fish
    {
        private int DaysLeft { get; set; }

        public Fish(int days)
        {
            this.DaysLeft = days;
        }

        public bool Subtract()
        {
            // Returns true if the fish should spawn, false if not
            if (--this.DaysLeft < 0)
            {
                this.DaysLeft = 6;
                return true;
            }
            return false;
        }
    }
}