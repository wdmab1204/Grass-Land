using KMolenda.Aisd.Graph;
using UnityEngine;
using UnityEngine.Tilemaps;

public struct TileNode
{
    public TileNode(Vector3Int localPosition)
    {
        this.localPosition = localPosition;
    }

    public Vector3Int localPosition { get; }

    public override string ToString()
    {
        var wynik = new System.Text.StringBuilder("[");
        wynik.Append($"{localPosition.x}, {localPosition.y}, {localPosition.z}");
        wynik[wynik.Length - 1] = ' ';
        return wynik.Append(']').ToString();
    }
}

public class TilemapReader : MonoBehaviour
{
    public Tilemap tilemap = null;
    private bool isPlaying = false;
    Graph<TileNode> Graph;

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
        var arr = new Vector3Int[tilemap.cellBounds.size.x + 2, tilemap.cellBounds.size.y + 2];

        for (int n = tilemap.cellBounds.xMin, i = 1; n < tilemap.cellBounds.xMax; n++, i++)
        {
            for (int p = tilemap.cellBounds.yMin, j = 1; p < tilemap.cellBounds.yMax; p++, j++)
            {
                if (tilemap.HasTile(new Vector3Int(n, p, 0))) 
                {
                    arr[i, j] = new Vector3Int(n, p, 0); //local position
                    Graph.AddVertex(new TileNode(arr[i,j]));

                    //check surrounding , and add vertices;
                    if (tilemap.HasTile(new Vector3Int(n - 1, p, 0))) Graph.AddVertex(new TileNode(arr[i - 1, j])); Graph.AddEdge(new TileNode(arr[i, j]), new TileNode(arr[i - 1, j]));
                    if (tilemap.HasTile(new Vector3Int(n + 1, p, 0))) Graph.AddVertex(new TileNode(arr[i + 1, j])); Graph.AddEdge(new TileNode(arr[i, j]), new TileNode(arr[i + 1, j]));
                    if (tilemap.HasTile(new Vector3Int(n, p - 1, 0))) Graph.AddVertex(new TileNode(arr[i, j - 1])); Graph.AddEdge(new TileNode(arr[i, j]), new TileNode(arr[i, j - 1]));
                    if (tilemap.HasTile(new Vector3Int(n, p + 1, 0))) Graph.AddVertex(new TileNode(arr[i, j + 1])); Graph.AddEdge(new TileNode(arr[i, j]), new TileNode(arr[i, j + 1]));
                }
                else
                {
                    arr[i, j] = new Vector3Int(n, p, 1); //local position
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        if (!isPlaying) return;

        foreach(var v in Graph.Vertices)
        {
            Gizmos.color = Color.red;
            var worldPosition = tilemap.GetCellCenterWorld(v.localPosition);
            Gizmos.DrawCube(worldPosition, Vector3.one * 0.5f);
        }
    }
}
