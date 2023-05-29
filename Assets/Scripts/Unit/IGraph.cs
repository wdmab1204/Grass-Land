using System;
using System.Collections.Generic;

namespace KMolenda.Aisd.Graph
{
    public interface IGraph<V>
    {
        // dodaje węzeł do grafu
        // jeśli węzeł już istnieje, nadpisuje
        bool AddVertex(V vertex);

        // sprawdza, czy węzeł jest w grafie
        bool ContainsVertex(V vertex);

        // zwraca kolekcję węzłów połaczonych z podanym
        IEnumerable<V> Neighbours(V vertex);

        // zwraca kolekcję wszystkich węzłów
        IEnumerable<V> Vertices { get; }

        // dodaje krawędź do grafu
        // jeśli węzłów `from` lub `to` nie ma, nie dodaje, zwraca false
        bool AddTwoEdge(V from, V to);
    }
}