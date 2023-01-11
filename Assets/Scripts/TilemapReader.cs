using System;
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

public class TileNode
{
    public TileNode(Vector3Int localPosition)
    {
        this.position = localPosition;
    }

    public TileNode(int x, int y, int z)
    {
        this.position = new Vector3Int(x, y, z);
    }

    public string ClassName { get => this.GetType().Name; }

    public Vector3Int position { get; }

    // 명시적 캐스트 
    public static explicit operator TileNode(Vector3Int position)
    {
        return new TileNode(position);
    }

    // 암시적 캐스트
    public static implicit operator Vector3Int(TileNode tileNode)
    {
        return tileNode.position;
    }

    public override string ToString()
    {
        var wynik = new System.Text.StringBuilder("[");
        wynik.Append($"{position.x}, {position.y}, {position.z}");
        return wynik.Append(']').ToString();
    }

    public override int GetHashCode()
    {
        if (position == null) return 0;
        return position.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        TileNode tile = obj as TileNode;
        return tile != null && tile.position == this.position;
    }

}

public class TilemapReader : MonoBehaviour
{
    public Tilemap tilemap = null;
    private bool isPlaying = false;
    public Graph<TileNode> Graph { get; private set; }

    private Graph<TileNode> CreateGraphWithTilemap(Tilemap tilemap)
    {
        Graph<TileNode> graph = new Graph<TileNode>();

        for (int n = tilemap.cellBounds.xMin, i = 0; n < tilemap.cellBounds.xMax; n++, i++)
        {
            for (int p = tilemap.cellBounds.yMin, j = 0; p < tilemap.cellBounds.yMax; p++, j++)
            {
                if (tilemap.HasTile(new Vector3Int(n, p, 0)))
                {
                    graph.AddVertex(new TileNode(n, p, 0));

                    //check surrounding , and add vertices;
                    if (tilemap.HasTile(new Vector3Int(n - 1, p, 0)))    graph.AddVertex(new TileNode(n - 1, p, 0)); graph.AddEdge(new TileNode(n, p, 0), new TileNode(n - 1, p, 0));
                    if (tilemap.HasTile(new Vector3Int(n + 1, p, 0)))    graph.AddVertex(new TileNode(n + 1, p, 0)); graph.AddEdge(new TileNode(n, p, 0), new TileNode(n + 1, p, 0));
                    if (tilemap.HasTile(new Vector3Int(n, p - 1, 0)))    graph.AddVertex(new TileNode(n, p - 1, 0)); graph.AddEdge(new TileNode(n, p, 0), new TileNode(n, p - 1, 0));
                    if (tilemap.HasTile(new Vector3Int(n, p + 1, 0)))    graph.AddVertex(new TileNode(n, p + 1, 0)); graph.AddEdge(new TileNode(n, p, 0), new TileNode(n, p + 1, 0));
                }
                else
                {

                }
            }
        }

        return graph;
    }

    public Vector3 ChangeLocalToWorldPosition(Vector3Int position) => tilemap.GetCellCenterWorld(position);

    public Vector3Int ChangeWorldToLocalPosition(Vector3 worldPosition) => tilemap.WorldToCell(worldPosition);

    public bool HasTile(Vector3Int position) => tilemap.HasTile(position);

    public bool HasTile(Vector3 worldPosition) => tilemap.HasTile(tilemap.WorldToCell(worldPosition));

    void Start()
    {
        isPlaying = true;

        tilemap = transform.GetComponentInParent<Tilemap>();
        Graph = CreateGraphWithTilemap(this.tilemap);
        Debug.Log(Graph.ToString<TileNode>());
    }
}
