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

public class TilemapGraph : Graph<TileNode>
{
    public Tilemap tilemap;

    private void InitializeGraphWithTilemap(Tilemap tilemap)
    {
        for (int n = tilemap.cellBounds.xMin, i = 0; n < tilemap.cellBounds.xMax; n++, i++)
        {
            for (int p = tilemap.cellBounds.yMin, j = 0; p < tilemap.cellBounds.yMax; p++, j++)
            {
                if (tilemap.HasTile(new Vector3Int(n, p, 0)))
                {
                    AddVertex(new TileNode(n, p, 0));

                    //check surrounding , and add vertices;
                    if (tilemap.HasTile(new Vector3Int(n - 1, p, 0))) AddVertex(new TileNode(n - 1, p, 0)); AddTwoEdge(new TileNode(n, p, 0), new TileNode(n - 1, p, 0));
                    if (tilemap.HasTile(new Vector3Int(n + 1, p, 0))) AddVertex(new TileNode(n + 1, p, 0)); AddTwoEdge(new TileNode(n, p, 0), new TileNode(n + 1, p, 0));
                    if (tilemap.HasTile(new Vector3Int(n, p - 1, 0))) AddVertex(new TileNode(n, p - 1, 0)); AddTwoEdge(new TileNode(n, p, 0), new TileNode(n, p - 1, 0));
                    if (tilemap.HasTile(new Vector3Int(n, p + 1, 0))) AddVertex(new TileNode(n, p + 1, 0)); AddTwoEdge(new TileNode(n, p, 0), new TileNode(n, p + 1, 0));
                }
                else
                {

                }
            }
        }
    }

    public TilemapGraph(Tilemap tilemap)
    {
        this.tilemap = tilemap;
        InitializeGraphWithTilemap(tilemap);
    }

    public TilemapGraph(Tilemap tilemap, int initialSize) : base(initialSize)
    {
        this.tilemap = tilemap;
        InitializeGraphWithTilemap(tilemap);
    }
}