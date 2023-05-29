using System.Collections.Generic;
using KMolenda.Aisd.Graph;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Navigation
{
    private Graph<TileNode> graph;
    private Tilemap tilemap;

    public Navigation(Graph<TileNode> graph)
    {
        this.graph = graph;
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();

        if (tilemap == null)
        {
            throw new System.NullReferenceException();
        }
    }

    public Path GetShortestPath(Vector3 start, Vector3 end)
    {
        TileNode localStart = (TileNode)tilemap.ChangeWorldToLocalPosition(start);
        TileNode localEnd = (TileNode)tilemap.ChangeWorldToLocalPosition(end);

        List<Vector3> pathList = new List<Vector3>();
        foreach(var node in graph.ShortestPath(localStart, localEnd))
        {
            Vector3 pathNode = tilemap.ChangeLocalToWorldPosition(node.position);
            pathList.Add(pathNode);
        }

        Path path = new Path(pathList.ToArray());

        return path;
    }

    public class Path
    {
        Vector3[] nodes;

        public Path(Vector3[] nodes)
        {
            this.nodes = nodes;
        }

        public Vector3 this[int index]
        {
            get => nodes[index];
            set => nodes[index] = value;
        }
    }
}