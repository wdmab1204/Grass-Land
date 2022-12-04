// GraphNode 클래스
using System.Collections.Generic;

public class GraphNode<T>
{
    private List<GraphNode<T>> _neighbors;
    private List<int> _weights;

    public T Data { get; set; }

    public GraphNode()
    {
    }

    public GraphNode(T value)
    {
        this.Data = value;
    }

    public List<GraphNode<T>> Neighbors
    {
        get
        {
            _neighbors = _neighbors ?? new List<GraphNode<T>>();
            return _neighbors;
        }
    }

    public List<int> Weights
    {
        get
        {
            _weights = _weights ?? new List<int>();
            return _weights;
        }
    }
}

// Graph 클래스
public class Graph<T>
{
    private List<GraphNode<T>> _nodeList;

    public Graph()
    {
        _nodeList = new List<GraphNode<T>>();
    }

    public GraphNode<T> AddNode(T data)
    {
        GraphNode<T> n = new GraphNode<T>(data);
        _nodeList.Add(n);
        return n;
    }

    public GraphNode<T> AddNode(GraphNode<T> node)
    {
        _nodeList.Add(node);
        return node;
    }

    public void AddEdge(GraphNode<T> from, GraphNode<T> to, bool oneway = true, int weight = 0)
    {
        from.Neighbors.Add(to);
        from.Weights.Add(weight);

        if (!oneway)
        {
            to.Neighbors.Add(from);
            to.Weights.Add(weight);
        }
    }

    internal void DebugPrintLinks()
    {
        foreach (GraphNode<T> graphNode in _nodeList)
        {
            foreach (var n in graphNode.Neighbors)
            {
                string s = graphNode.Data + " - " + n.Data;
                //Console.WriteLine(s);
            }
        }
    }
}