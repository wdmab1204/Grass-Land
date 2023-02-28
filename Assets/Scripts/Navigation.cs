using System.Collections.Generic;
using KMolenda.Aisd.Graph;

public class Navigation<T>
{
    private Graph<T> graph;

    public Navigation(Graph<T> graph)
    {
        this.graph = graph;
    }

    public Path GetShortestPath(T start, T end)
    {
        List<T> nodeList = new List<T>();
        foreach(var node in graph.ShortestPath(start, end))
        {
            nodeList.Add(node);
        }

        Path path = new Path(nodeList.ToArray());

        return path;
    }

    public class Path
    {
        T[] nodes;

        public Path(T[] nodes)
        {
            this.nodes = nodes;
        }

        public T this[int index]
        {
            get => nodes[index];
            set => nodes[index] = value;
        }
    }
}