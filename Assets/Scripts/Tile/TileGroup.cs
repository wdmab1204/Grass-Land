using System;
using System.Collections;
using System.Collections.Generic;
using CardNameSpace.Base;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileGroup : MonoBehaviour, IEnumerable, IGraphicsDisplay
{
    Tilemap tilemap;
    private Dictionary<Vector2Int, VisibilityTile> tileDictionary = new Dictionary<Vector2Int, VisibilityTile>();

    public void CreateClones(VisibilityTile tilePrefab, Vector2Int[] coords, Vector3Int center = default)
    {
        if (center == default) center = Vector3Int.zero;
        foreach(var coord in coords)
        {
            var tileWorldPosition = center + (Vector3Int)coord;
            if (!tilemap.HasTile(tileWorldPosition)) continue;
            var visibilityTile = Instantiate(tilePrefab, this.transform).GetComponent<VisibilityTile>();
            visibilityTile.transform.position = tilemap.ChangeLocalToWorldPosition(tileWorldPosition);
            if (tileDictionary.ContainsKey(coord))
            {
                Destroy(tileDictionary[coord].gameObject);
                tileDictionary.Remove(coord);
            }
            tileDictionary.Add(coord, visibilityTile);
            visibilityTile.Show();
        }
    }

    private void Awake()
    {
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
    }

    public void Hide()
    {
        foreach(var value in tileDictionary.Values)
        {
            value.Hide();
        }
    }

    public void Show()
    {
        foreach (var value in tileDictionary.Values)
        {
            value.Show();
        }
    }

    private class MyEnumerator : IEnumerator
    {
        public VisibilityTile[] tileArray;
        int position = -1;

        //constructor
        public MyEnumerator(VisibilityTile[] arr)
        {
            tileArray = arr;
        }
        private IEnumerator getEnumerator()
        {
            return (IEnumerator)this;
        }
        //IEnumerator
        public bool MoveNext()
        {
            position++;
            return (position < tileArray.Length);
        }
        //IEnumerator
        public void Reset()
        {
            position = -1;
        }
        //IEnumerator
        public object Current
        {
            get
            {
                try
                {
                    return tileArray[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }

    public IEnumerator GetEnumerator()
    {
        VisibilityTile[] arr = new VisibilityTile[tileDictionary.Count];
        tileDictionary.Values.CopyTo(arr, 0);
        return new MyEnumerator(arr);
    }
}

