using System;
using System.Collections;
using System.Collections.Generic;
using CardNameSpace.Base;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGroup : MonoBehaviour, IGraphicsDisplay
{
    private Tilemap tilemap;
    private Dictionary<Vector3Int, TileBase> tileDictionary = new Dictionary<Vector3Int, TileBase>();
    private Dictionary<string, TileBase> tilenameDic = new Dictionary<string, TileBase>();
    public TileBase[] tileArray;
    public Tilemap dynamicTilemap;

    public void CreateClones(string name, Range range, Vector3 center = default)
    {
        if (center == default) center = Vector3.zero;
        var centerLocalPosition = tilemap.ChangeWorldToLocalPosition(center);
        var tile = tilenameDic[name];

        foreach(var localCoord in range.localCoords)
        {
            Debug.Log(localCoord);
            var tileLocalPosition = centerLocalPosition + (Vector3Int)localCoord;
            if (!tilemap.HasTile(tileLocalPosition)) continue;
            dynamicTilemap.SetTile(tileLocalPosition, tile);
            var visibilityTile = Instantiate(tile);
            if (tileDictionary.ContainsKey(tileLocalPosition))
            {
                tileDictionary[tileLocalPosition] = tile;
            }
            else tileDictionary.Add(tileLocalPosition, visibilityTile);
        }
    }

    private void Awake()
    {
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        foreach (var tile in tileArray) tilenameDic.Add(tile.name, tile);
    }

    public void Hide()
    {
        foreach (var pair in tileDictionary)
        {
            dynamicTilemap.SetTile(pair.Key, null);
        }
    }

    public void Show()
    {
        foreach(var pair in tileDictionary)
        {
            dynamicTilemap.SetTile(pair.Key, pair.Value);
        }
    }
}

