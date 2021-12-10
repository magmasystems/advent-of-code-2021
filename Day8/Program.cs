using System.Text.RegularExpressions;

// ReSharper disable once CheckNamespace
namespace AdventOfCode2021
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // cbgefad agc fdega cgdf ecdgfa efgca gaefbd edagbc cg ecafb | cdgafbe cfdg cg gac
            Regex regex = new(@"^((?<pulse>\w+)\s){10}\|(\s(?<digit>\w+)){4}$", RegexOptions.Compiled);

            // PART 1

            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            
            var sumOfDigitsWithUniqueNumberOfSegments = 0;
            foreach (var line in input)
            {
                var match = regex.Match(line);
                foreach (Capture capture in match.Groups["digit"].Captures)
                {
                    if (capture.Value.Length is 2 or 3 or 4 or 7)
                        sumOfDigitsWithUniqueNumberOfSegments++;
                }
            }

            // The sum of digits should be 514
            Console.WriteLine($"Sum is {sumOfDigitsWithUniqueNumberOfSegments}");
            
            // PART 2 

            var sumOfDisplayNumbers = 0;
            foreach (var line in input)
            {
                var match = regex.Match(line);
                if (!match.Success)
                    continue;

                var DigitMask = new ushort[10];
                var _23or5 = new List<ushort>(3);
                var _06or9 = new List<ushort>(3);

                for (var i = 0; i < 10; i++)
                {
                    var pulse = match.Groups["pulse"].Captures[i].Value;
                    // Turn the string into a 7-bit bitmask, from 'a' to 'g'
                    var mask = SegmentStringToMask(pulse);
                    switch (pulse.Length)
                    {
                        case 2:
                            DigitMask[1] = mask;
                            break;
                        case 3:
                            DigitMask[7] = mask;
                            break;
                        case 4:
                            DigitMask[4] = mask;
                            break;
                        case 7:
                            DigitMask[8] = mask;
                            break;
                        case 5:
                            _23or5.Add(mask);
                            break;
                        case 6:
                            _06or9.Add(mask);
                            break;
                    }
                }

                // Compare the 3 digits that are of length 5 with '7'. The one that has a difference of 2 segments is the digit '3'.
                for (var i = 0; i < _23or5.Count; i++)
                {
                    if (CountOnesInBitMask((ushort)(_23or5[i] ^ DigitMask[7])) == 2)
                    {
                        DigitMask[3] = _23or5[i];
                        _23or5.RemoveAt(i);
                        // Now we have digits 2 and 5 left.
                        break;
                    }
                }
                
                // Compare the 3 digits that are of length 6 with '7'. The one that has a difference of 5 segments is the digit '6'.
                // That's because digits 0 and 9 share the two vertical segments that 7 has.
                for (var i = 0; i < _06or9.Count; i++)
                {
                    if (CountOnesInBitMask((ushort)(_06or9[i] ^ DigitMask[7])) == 5)
                    {
                        DigitMask[6] = _06or9[i];
                        _06or9.RemoveAt(i);
                        // Now we have digits 0 and 9 left.
                        break;
                    }
                }
                
                // Now that we know what '6' is, we can compare the candidates for '2' and '5'.
                // The segments of '6' differ from the segments of '5' by the vertical segment in the lower right.
                for (var i = 0; i < _23or5.Count; i++)
                {
                    if (CountOnesInBitMask((ushort)(_23or5[i] ^ DigitMask[6])) == 1)
                    {
                        DigitMask[5] = _23or5[i];
                        _23or5.RemoveAt(i);
                        DigitMask[2] = _23or5[0];
                        break;
                    }
                }
                
                // The only ones that are left are 0 and 9.
                // The only difference between 9 and 3 is the vertical segment in the upper left.
                for (var i = 0; i < _06or9.Count; i++)
                {
                    if (CountOnesInBitMask((ushort)(_06or9[i] ^ DigitMask[3])) == 1)
                    {
                        DigitMask[9] = _06or9[i];
                        _06or9.RemoveAt(i);
                        DigitMask[0] = _06or9[0];
                        break;
                    }
                }

                var displayNumber = 0;
                for (var i = 0; i < 4; i++)
                {
                    string pulse = match.Groups["digit"].Captures[i].Value;
                    // Turn the string into a 7-bit bitmask, from 'a' to 'g'
                    var mask = SegmentStringToMask(pulse);
                    
                    for (var j = 0; j < DigitMask.Length; j++)
                    {
                        if (mask == DigitMask[j])
                        {
                            // We found the digit
                            displayNumber = displayNumber * 10 + j;
                            break;
                        }
                    }
                }
                // Console.WriteLine($"The displayed number is {displayNumber}");
                sumOfDisplayNumbers += displayNumber;
            }
            
            // The sum of the displayed numbers is 1012272
            Console.WriteLine($"The sum of the displayed numbers is {sumOfDisplayNumbers}");
        }

        private static ushort SegmentStringToMask(string segment)
        {
            // Turn the string into a 7-bit bitmask, from 'a' to 'g'
            return segment.Aggregate<char, ushort>(0, (current, t) => (ushort)(current | (ushort)(1 << (t - 'a'))));
        }

        private static int CountOnesInBitMask(ushort bitMask)
        {
            var sum = 0;
            for (var i = 0;  i < 7;  i++)
            {
                sum += (bitMask & 0x01) == 0x01 ? 1 : 0;
                bitMask >>= 1;
            }
            return sum;
        }
    }
}
