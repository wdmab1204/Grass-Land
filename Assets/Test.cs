using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    public Tilemap tilemap;

    void Start()
    {
        transform.position = tilemap.ChangeLocalToWorldPosition(Vector3Int.zero);
        
    }
}