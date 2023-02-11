using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomTilemap : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] private TileBase[] tiles;
    private readonly int width = 15;
    private readonly int height = 15;


    void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        for (int i=0; i<width; i++)
        {
            for(int j=0; j<height; j++)
            {
                var localPosition = new Vector3Int(i, j, 0);
                tilemap.SetTile(localPosition, GetRandomTile());
            }
        }
    }

    private TileBase GetRandomTile()
    {
        int index = Random.Range(0, tiles.Length);
        return tiles[index];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
