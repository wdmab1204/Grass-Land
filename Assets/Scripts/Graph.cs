using System;
using System.Collections.Generic;
using System.Text;

namespace KMolenda.Aisd.Graph
{

    /// <summary>
    /// Implementacja grafu nieskierowanego, nieważonego w formie listy sąsiadów (adjacency list)
    /// </summary>
    /// <remarks>
    /// Założenia:
    /// 1. graf nieskierowany, nieważony
    /// 2. węzły są typu <code>T</code> - są to etykiety
    /// 3. węzły muszą być unikalne
    /// 4. krawędzie są typu <code>Tuple(T,T)</code>
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class Graph<T> : IGraph<T>
    {
        // Dictionary: { 1 -> {2, 3}, 2 -> {1}, 3 -> {1} }
        public Dictionary<T, HashSet<T>> AdjacencyList { get; } = new Dictionary<T, HashSet<T>>();

        public Graph() { }

        public Graph(int initialSize)
        {
            AdjacencyList = new Dictionary<T, HashSet<T>>(initialSize);
        }

        public Graph(IEnumerable<T> vertices, IEnumerable<Tuple<T, T>> edges)
        {
            foreach (var vertex in vertices) AddVertex(vertex);
            foreach (var edge in edges) AddTwoEdge(edge);
        }

        public bool AddVertex(T vertex)
        {
            if (ContainsVertex(vertex))
                return false;

            AdjacencyList[vertex] = new HashSet<T>();
            return true;
        }

        public bool ContainsVertex(T vertex) => AdjacencyList.ContainsKey(vertex);

        public IEnumerable<T> Neighbours(T vertex) => AdjacencyList[vertex];

        public IEnumerable<T> Vertices => AdjacencyList.Keys;

        public bool AddEdge(T from, T to) => AddEdge((from, to));

        public bool AddEdge((T from, T to) edge)
        {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
            {
                AdjacencyList[edge.Item1].Add(edge.Item2);
                return true;
            }

            return false;
        }

        public bool AddTwoEdge(T from, T to) => AddTwoEdge(Tuple.Create(from, to));

        public bool AddTwoEdge(Tuple<T, T> edge)
        {
            if (AdjacencyList.ContainsKey(edge.Item1) && AdjacencyList.ContainsKey(edge.Item2))
            {
                AdjacencyList[edge.Item1].Add(edge.Item2);
                AdjacencyList[edge.Item2].Add(edge.Item1);
                return true;
            }

            return false;
        }

        // public IEnumerable< Tuple<T,T> > Edges {get;} // ToDo

    }
}