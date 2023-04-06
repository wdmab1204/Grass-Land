using System;
using UnityEngine;

public class Door : Interactive
{
    public Vector3 nextPosition;
    public Room nextRoom;
    public override void Do(Transform player)
    {
        if(nextRoom != null)
        {
            TilemapManager tilemapManager = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<TilemapManager>();
            tilemapManager.currentRoom = nextRoom;
            player.position = new Vector3(nextRoom.Position.x, nextRoom.Position.y);
        }
    }
}

