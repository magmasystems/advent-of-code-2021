using System.Collections;

namespace AdventOfCode2021
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            // This is part 1

            Dictionary<char, int> CharToPoints = new()
            {
                { ')',     3 },
                { ']',    57 },
                { '}',  1197 },
                { '>', 25137 }
            };
            
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");

            var points = 0;
            var incompleteStacks = new List<Stack<char>>();

            for (var index = 0; index < input.Length; index++)
            {
                var line = input[index];
                var stack = new Stack<char>();
                var firstCount = 0;

                foreach (var c in line)
                {
                    switch (c)
                    {
                        case '(' or '<' or '{' or '[':
                            stack.Push(c);
                            break;
                        case ')' or '>' or '}' or ']':
                        {
                            var c2 = stack.Pop();
                            if (c == ')' && c2 != '(' || c == '}' && c2 != '{' || c == '>' && c2 != '<' ||
                                c == ']' && c2 != '[')
                            {
                                if (firstCount == 0)
                                {
                                    firstCount = CharToPoints[c];
                                }
                            }

                            break;
                        }
                    }
                }

                if (firstCount != 0)
                {
                    points += firstCount;
                }
                else
                {
                    incompleteStacks.Add(stack);
                }
            }

            // The number of points is 413733
            Console.WriteLine($"The number of points is {points}");
            
            // PART 2

            var allPoints = incompleteStacks.Select(stack => stack.Aggregate<char, ulong>(0L, (current, c) => c switch
                {
                    '(' => current * 5 + 1,
                    '[' => current * 5 + 2,
                    '{' => current * 5 + 3,
                    '<' => current * 5 + 4,
                    _ => current
                }))
                .ToList();

            allPoints.Sort();
            
            // The median points is 3354640192
            Console.WriteLine($"The median points is {allPoints[allPoints.Count/2]}");
        }
    }
}
