using System.Collections.Generic;
using KMolenda.Aisd.Graph;
using UnityEngine;
using UnityEngine.Tilemaps;

/*========================================================

    Variable naming Rules

    Tile Local Position
    => tile position, position... no Local

    Tile World Position
    => tile world Position... must have "world" word

=========================================================*/


public struct TileNode
{
    public TileNode(Vector3Int localPosition)
    {
        this.localPosition = localPosition;
    }

    public TileNode(int x, int y, int z)
    {
        this.localPosition = new Vector3Int(x, y, z);
    }

    public Vector3Int localPosition { get; }

    public override string ToString()
    {
        var wynik = new System.Text.StringBuilder("[");
        wynik.Append($"{localPosition.x}, {localPosition.y}, {localPosition.z}");
        wynik[wynik.Length - 1] = ' ';
        return wynik.Append(']').ToString();
    }

    public string ClassName { get => this.GetType().Name; }
}

public class TilemapReader : MonoBehaviour
{
    public Tilemap tilemap = null;
    private bool isPlaying = false;
    public Graph<TileNode> Graph { get; private set; }

    void Start()
    {
        isPlaying = true;

        tilemap = transform.GetComponentInParent<Tilemap>();
        InitGraph(this.tilemap);
        Debug.Log(Graph.ToString<TileNode>());


    }


    //[x,x,x,x,x]
    //[x,0,0,0,x]
    //[x,0,0,0,x]
    //[x,0,0,0,x]
    //[x,x,x,x,x]
    private void InitGraph(Tilemap tilemap)
    {
        Graph = new Graph<TileNode>();

        for (int n = tilemap.cellBounds.xMin, i = 0; n < tilemap.cellBounds.xMax; n++, i++)
        {
            for (int p = tilemap.cellBounds.yMin, j = 0; p < tilemap.cellBounds.yMax; p++, j++)
            {
                if (tilemap.HasTile(new Vector3Int(n, p, 0)))
                {
                    Graph.AddVertex(new TileNode(n, p, 0));

                    //check surrounding , and add vertices;
                    if (tilemap.HasTile(new Vector3Int(n - 1, p, 0)))    Graph.AddVertex(new TileNode(n - 1, p, 0)); Graph.AddEdge(new TileNode(n, p, 0), new TileNode(n - 1, p, 0));
                    if (tilemap.HasTile(new Vector3Int(n + 1, p, 0)))    Graph.AddVertex(new TileNode(n + 1, p, 0)); Graph.AddEdge(new TileNode(n, p, 0), new TileNode(n + 1, p, 0));
                    if (tilemap.HasTile(new Vector3Int(n, p - 1, 0)))    Graph.AddVertex(new TileNode(n, p - 1, 0)); Graph.AddEdge(new TileNode(n, p, 0), new TileNode(n, p - 1, 0));
                    if (tilemap.HasTile(new Vector3Int(n, p + 1, 0)))    Graph.AddVertex(new TileNode(n, p + 1, 0)); Graph.AddEdge(new TileNode(n, p, 0), new TileNode(n, p + 1, 0));



                }
                else
                {

                }
            }
        }

    }

    //private void OnDrawGizmos()
    //{
    //    if (!isPlaying) return;

    //    foreach(var v in Graph.Vertices)
    //    {
    //        Gizmos.color = Color.red;
    //        var worldPosition = tilemap.GetCellCenterWorld(v.localPosition);
    //        Gizmos.DrawCube(worldPosition, Vector3.one * 0.5f);
    //    }
    //}
}
