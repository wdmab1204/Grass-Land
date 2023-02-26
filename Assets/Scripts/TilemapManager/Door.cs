using System;
using UnityEngine;

public class Door : Interactive
{
    public Room nextRoom;
    public override void Do(Transform player)
    {
        if(nextRoom!= null)
        {
            player.position = new Vector3(nextRoom.Position.x, nextRoom.Position.y, 0);
        }
    }
}

