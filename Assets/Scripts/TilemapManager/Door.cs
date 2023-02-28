using System;
using UnityEngine;

public class Door : Interactive
{
    public Vector3 nextPosition;
    public override void Do(Transform player)
    {
        if(nextPosition != null)
        {
            player.position = nextPosition;
        }
    }
}

