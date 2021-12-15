// ReSharper disable UnusedMember.Local
namespace AdventOfCode2021
{
    internal static class Program
    {
        private static SortedDictionary<string, ulong> Bigrams;
        private static SortedDictionary<char, ulong> MapOfUniqueLetters;

        private static void Main(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            Dictionary<string, string> rules = new();
            int[] steps = { 10, 40 };
            
            for (var part = 1; part <= 2; part++)
            {
                Bigrams = new();
                MapOfUniqueLetters = new();
                ParseInput(input, rules);
                
                for (var step = 1; step <= steps[part-1]; step++)
                {
                    var bigramsCopy = new SortedDictionary<string, ulong>(Bigrams);

                    foreach (var kvp in Bigrams.Where(_ => _.Value != 0))
                    {
                        if (!rules.TryGetValue(kvp.Key, out var stringToInsert))
                            continue;
                        var pair1 = new string(new[] { kvp.Key[0], stringToInsert[0] });
                        var pair2 = new string(new[] { stringToInsert[0], kvp.Key[1] });
                        bigramsCopy[pair1] += Bigrams[kvp.Key];
                        bigramsCopy[pair2] += Bigrams[kvp.Key];
                        bigramsCopy[kvp.Key] -= Bigrams[kvp.Key];

                        MapOfUniqueLetters[stringToInsert[0]] += Bigrams[kvp.Key];
                    }

                    Bigrams = bigramsCopy;
                    //Console.WriteLine($"Step {step}");
                    //DumpBigrams(Bigrams);
                    //DumpCharacterCounts(MapOfUniqueLetters);
                }

                // Part 1 = 3406
                // Part 2 = 3941782230241
                Console.WriteLine($"Part {part}: The difference between Max and Min is {MapOfUniqueLetters.Values.Max() - MapOfUniqueLetters.Values.Min()}");
            }
        }

        private static string ParseInput(IEnumerable<string> input, IDictionary<string, string> rules)
        {
            var polymerTemplate = "";

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(polymerTemplate))
                {
                    polymerTemplate = line;
                    continue;
                }

                if (line.Length == 0)
                    continue;

                var parts = line.Split(new[] { ' ', '-', '>' }, StringSplitOptions.RemoveEmptyEntries);
                if (!rules.ContainsKey(parts[0]))
                    rules.Add(parts[0], parts[1]);

                MapOfUniqueLetters[parts[0][0]] = 0L;
                MapOfUniqueLetters[parts[0][1]] = 0L;
                MapOfUniqueLetters[parts[1][0]] = 0L;
            }

            foreach (var c in polymerTemplate)
            {
                if (!MapOfUniqueLetters.ContainsKey(c))
                    MapOfUniqueLetters.Add(c, 0L);
                else
                    MapOfUniqueLetters[c]++;
            }

            foreach (var pair in from ch1 in MapOfUniqueLetters.Keys from ch2 in MapOfUniqueLetters.Keys select $"{ch1}{ch2}")
            {
                Bigrams.Add(pair, 0L);
            }

            for (var index = 1; index < polymerTemplate.Length; index++)
            {
                var pair = new string(new[] { polymerTemplate[index - 1], polymerTemplate[index] });
                Bigrams[pair]++;
            }

            return polymerTemplate;
        }

        private static SortedDictionary<char, ulong> GetCountPerLetter(string polymerTemplate)
        {
            var countPerLetter = new SortedDictionary<char, ulong>();
            foreach (var ch in polymerTemplate)
            {
                if (!countPerLetter.ContainsKey(ch))
                    countPerLetter.Add(ch, 0);
                countPerLetter[ch]++;
            }
            return countPerLetter;
        }

        private static void DumpBigrams(SortedDictionary<string, ulong> dict)
        {
            foreach (var (key, value) in dict.Where(_ => _.Value != 0))
            {
                Console.Write($"{key}={value} ");
            }
            Console.WriteLine();
        }
        
        private static void DumpCharacterCounts(SortedDictionary<char, ulong> dict)
        {
            ulong total = 0;
            foreach (var (key, value) in dict)
            {
                Console.Write($"{key}={value} ");
                total += value;
            }
            Console.WriteLine($" - length is {total}");
        }
    }
}
