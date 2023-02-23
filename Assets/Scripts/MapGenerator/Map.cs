using System;
using System.Collections.Generic;
using KMolenda.Aisd.Graph;

public class Map : IGraph<Room>
{
    public Dictionary<Room, HashSet<Room>> AdjacencyList { get; } = new Dictionary<Room, HashSet<Room>>();

    public Map() { }

    public Map(int initialSize)
    {
        AdjacencyList = new Dictionary<Room, HashSet<Room>>(initialSize);
    }

    public Map(IEnumerable<Room> vertices, IEnumerable<Tuple<Room, Room>> edges)
    {
        foreach (var vertex in vertices) AddVertex(vertex);
        foreach (var edge in edges) AddEdge(edge);
    }

    public bool AddVertex(Room vertex)
    {
        if (ContainsVertex(vertex))
            return false;

        AdjacencyList[vertex] = new HashSet<Room>();
        return true;
    }

    public bool ContainsVertex(Room vertex) => AdjacencyList.ContainsKey(vertex);

    public IEnumerable<Room> Neighbours(Room vertex) => AdjacencyList[vertex];

    public IEnumerable<Room> Vertices => AdjacencyList.Keys;

    public bool AddEdge(Room from, Room to) => AddEdge(Tuple.Create(from, to));

    public bool AddEdge(Tuple<Room, Room> edge)
    {
        if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
        {
            AdjacencyList[edge.Item1].Add(edge.Item2);
            AdjacencyList[edge.Item2].Add(edge.Item1);
            return true;
        }

        return false;
    }
}


