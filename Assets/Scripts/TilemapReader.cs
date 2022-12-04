using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TileData = System.Collections.Generic.Dictionary<UnityEngine.Vector3, int>;

public enum TileDirection
{
    topLeft,
    topRight,
    bottomLeft,
    bottomRight
};

public class TilemapReader : MonoBehaviour
{
    public Tilemap tilemap = null;

    public TileData availablePlaces;

    private bool isPlaying = false;


    Vector3Int[,] localPlaces;

    void Start()
    {
        isPlaying = true;

        tilemap = transform.GetComponentInParent<Tilemap>();
        localPlaces = new Vector3Int[tilemap.cellBounds.size.x, tilemap.cellBounds.size.y];

        for (int n = tilemap.cellBounds.xMin, i = 0; n < tilemap.cellBounds.xMax; n++, i++)
        {
            for (int p = tilemap.cellBounds.yMin, j = 0; p < tilemap.cellBounds.yMax; p++, j++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tilemap.transform.position.y));
                Vector3 place = tilemap.GetCellCenterWorld(localPlace);
                if (tilemap.HasTile(localPlace))
                {
                    //Tile at "place"
                    localPlaces[i, j] = localPlace;

                }
                else
                {

                    //Debug.Log(place);
                }
            }
        }

    }

    public Vector3 GetNextTilePosition(Vector3 currentTile, TileDirection direction)
    {
        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        if (!isPlaying) return;

        for(int i=0; i<tilemap.size.x; i++)
        {
            for(int j=0; j<tilemap.size.y; j++)
            {
                var place = tilemap.GetCellCenterWorld(localPlaces[i, j]);
                //var place = spots[i, j];

                Gizmos.color = Color.red;
                Gizmos.DrawCube(place, Vector3.one * 0.5f) ;

            }
        }
    }
}
