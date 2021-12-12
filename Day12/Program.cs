namespace AdventOfCode2021
{
    internal static class Program
    {
        private const int START = 0;
        private const int END = 1;

        private class NodeInfo
        {
            public string Name { get; }
            public int Index { get; }
            private static int Counter;
            public bool IsSmall => Name[0] >= 'a' && Name[0] <= 'z';
            public readonly List<NodeInfo> Adjacent = new();

            public NodeInfo(string name)
            {
                this.Name = name;
                this.Index = Counter++;
            }
        }
        
        // Map node name to a value that indiciates whether it is a small cave
        private static readonly Dictionary<string, NodeInfo> Nodes = new()
        {
            { "start", new NodeInfo("start") },
            { "end",   new NodeInfo("end") },
        };
        
        private static readonly HashSet<string> FoundPaths = new();
        private static int Part;

        private static void Main(string[] args)
        {
            BuildAdjacencyList(args);

            for (Part = 1; Part <= 2; Part++)
            {
                foreach (var node2 in Nodes["start"].Adjacent)
                {
                    var visited = new int[Nodes.Count];
                    FindPaths(node2, visited, "start");
                }

                // Part 1 should be 5457
                // Part 2 should be 128506
                Console.WriteLine($"Part {Part}: The number of paths is {FoundPaths.Count}");
                FoundPaths.Clear();
            }
        }

        private static void BuildAdjacencyList(string[] args)
        {
            var input = File.ReadAllLines(args.Length > 0 ? args[0] : "Input.txt");
            foreach (var line in input)
            {
                var parts = line.Split('-');
                if (!Nodes.ContainsKey(parts[0]))
                {
                    Nodes[parts[0]] = new NodeInfo(parts[0]);
                }

                if (!Nodes.ContainsKey(parts[1]))
                {
                    Nodes[parts[1]] = new NodeInfo(parts[1]);
                }

                Nodes[parts[0]].Adjacent.Add(Nodes[parts[1]]);
                Nodes[parts[1]].Adjacent.Add(Nodes[parts[0]]);
            }
        }

        private static void FindPaths(NodeInfo node, int[] visited, string path)
        {
            switch (node.Index)
            {
                case END:
                    path += $",{node.Name}";
                    FoundPaths.Add(path);
                    return;
                case START:
                    return;
            }

            switch (Part)
            {
                case 1 when visited[node.Index] == 1:
                    
                // If the node has been visited and there is already a cave that has been visited twice, return
                case 2 when visited[node.Index] >= 1 && visited.Contains(2):
                    return;
            }

            path += $",{node.Name}";
            if (node.IsSmall)
                visited[node.Index]++;

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var node2 in node.Adjacent)
            {
                var oldVisited = new int[visited.Length];
                Array.Copy(visited, 0, oldVisited, 0, visited.Length);

                FindPaths(node2, visited, path);
                
                Array.Copy(oldVisited, 0, visited, 0, visited.Length);    
            }
        }
    }
}
