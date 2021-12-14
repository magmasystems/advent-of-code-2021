namespace AdventOfCode2021
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            Dictionary<string, string> rules = new();
            
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            var polymerTemplate = ParseInput(input, rules);
            var polymerTemplateCopy = polymerTemplate;
            int[] steps = { 10, 40 };

            for (var part = 0; part < 2; part++)
            {
                for (var step = 1; step <= steps[part]; step++)
                {
                    var insertionMap = new SortedDictionary<int, string>();
                    for (var idx = 0; idx < polymerTemplate.Length - 1; idx++)
                    {
                        FindInsertionPoints(polymerTemplate, polymerTemplate.Substring(idx, 2), rules, insertionMap);
                    }

                    // Now perform the substitutions
                    var numInserted = 0;
                    foreach (var (index, stringToInsert) in insertionMap)
                    {
                        polymerTemplate = polymerTemplate.Insert(index + numInserted, stringToInsert);
                        numInserted++;
                    }
                }

                var countPerLetter = GetCountPerLetter(polymerTemplate);

                // Part 1 = 3406
                Console.WriteLine($"Part {part+1}: The difference between Max and Min is {countPerLetter.Values.Max() - countPerLetter.Values.Min()}");
                polymerTemplate = polymerTemplateCopy;
            }
        }

        private static void FindInsertionPoints(string polymerTemplate, string pair, IDictionary<string, string> rules, IDictionary<int, string> insertionMap)
        {
            if (!rules.TryGetValue(pair, out var stringToInsert))
                return;
            
            for (var idx = 1; idx < polymerTemplate.Length; idx++)
            {
                if (polymerTemplate[idx-1] != pair[0] || polymerTemplate[idx] != pair[1])
                    continue;
                if (!insertionMap.ContainsKey(idx))
                    insertionMap.Add(idx, "");
                insertionMap[idx] = stringToInsert;
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
                rules.Add(parts[0], parts[1]);
            }

            return polymerTemplate;
        }

        private static Dictionary<char, int> GetCountPerLetter(string polymerTemplate)
        {
            var countPerLetter = new Dictionary<char, int>();
            foreach (var ch in polymerTemplate)
            {
                if (!countPerLetter.ContainsKey(ch))
                    countPerLetter.Add(ch, 0);
                countPerLetter[ch]++;
            }
            return countPerLetter;
        }
    }
}
