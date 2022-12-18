using System;
using System.Collections.Generic;

namespace KMolenda.Aisd.Graph
{
    public static class AlgorithmsExtensions
    {

        public static IEnumerable<T> TraverseDepthFirst<T>(this IGraph<T> graph, T start)
        {
            var visited = new HashSet<T>();

            if (!graph.ContainsVertex(start))
                yield break;

            var stack = new Stack<T>();
            stack.Push(start);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                if (visited.Contains(current))
                    continue;

                yield return current;

                visited.Add(current);

                foreach (var neighbour in graph.Neighbours(current))
                    if (!visited.Contains(neighbour))
                        stack.Push(neighbour);
            }

            yield break;
        }

        public static IEnumerable<T> TraverseBreadthFirst<T>(this IGraph<T> graph, T start)
        {
            var visited = new HashSet<T>();

            if (!graph.ContainsVertex(start))
                yield break;

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();

                if (visited.Contains(current))
                    continue;

                yield return current;

                visited.Add(current);

                foreach (var neighbour in graph.Neighbours(current))
                    if (!visited.Contains(neighbour))
                        queue.Enqueue(neighbour);
            }

            yield break;
        }


        // Funkcja zwracająca funkcję, któa zwraca najkrószą ścieżkę (w sensie liczby krawędzi)
        // między wskazanymi wezłami
        // Użycie:  var path = graph.ShortestPathFunc<int>(start: 1)(4);
        // Wykorzystuje koncepcję BFS. Wychodząc od początkowego wierzchołka, zapamiętuje
        // w słowniku `previous` jak dojść do każdego węzła. aby znaleźć najkrószą ścieżkę
        // wyszukujemy poprzedni węzeł dla węzła docelowego i kontynuujemy przeglądanie
        // wszystkich poprzednich węzłów, aż dotrzemy do węzła początkowego.
        // Otrzymana ścieżka jest w kolejności odwrotnej, dlatego Reverse();
        public static Func<T, IEnumerable<T>> ShortestPathFunc<T>(this IGraph<T> graph, T start)
        {
            var previous = new Dictionary<T, T>();

            var queue = new Queue<T>();
            queue.Enqueue(start);

            while (queue.Count > 0)
            {
                var vertex = queue.Dequeue();
                foreach (var neighbour in graph.Neighbours(vertex))
                {
                    if (previous.ContainsKey(neighbour))
                        continue;

                    previous[neighbour] = vertex;
                    queue.Enqueue(neighbour);
                }
            }

            Func<T, IEnumerable<T>> shortestPath = v =>
            {
                var path = new List<T> { };

                var current = v;
                while (!current.Equals(start))
                {
                    path.Add(current);
                    current = previous[current];
                };

                path.Add(start);
                path.Reverse(); //można zakomentować, jeśli nie przeszkadza odwrotny porządek
                                //zawsze można później odwrócić przechwycony wynik

                return path;
            };

            return shortestPath;
        } // koniec ShortestPathFunc

        // Funkcja zwracająca sekwencję węzłów będacych najkrószą ścieżką (w sensie liczby krawędzi)
        // między wskazanymi węzłami
        public static IEnumerable<T> ShortestPath<T>(this Graph<T> graph, T start, T end)
           => graph.ShortestPathFunc<T>(start)(end);


        public static string ToString<T>(this Graph<T> graph)
        {
            var wynik = new System.Text.StringBuilder("{");
            foreach (var vertex in graph.AdjacencyList)
            {
                wynik.Append($" {vertex.Key}->{{{string.Join(", ", vertex.Value)}}}; {System.Environment.NewLine}"); //To output a { you use {{ and to output a } you use }}.

            }
            wynik[wynik.Length - 1] = ' ';
            return wynik.Append('}').ToString();
        }


    }

}