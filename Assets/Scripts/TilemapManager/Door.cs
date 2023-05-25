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
            player.position = new Vector3(nextRoom.Position.x, nextRoom.Position.y);
            GameRuleSystem gameRuleSystem = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameRuleSystem>();
            //gameRuleSystem.UpdateTurnOrderQueue();
        }
    }
}

