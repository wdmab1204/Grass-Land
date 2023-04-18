using System;
using System.Collections.Generic;
using System.Drawing;
using KMolenda.Aisd.Graph;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class Map : Graph<Room>
{
    Dictionary<Room, List<GameObject>> ObjectData = new Dictionary<Room, List<GameObject>>();
    System.Random rand = new System.Random();
    Tilemap tilemap;

    public Map(int initialSize) : base(initialSize)
    {
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
    }

    public void SetObjects(Room room, List<GameObject> mobs)
    {
        ObjectData[room] = mobs;
    }

    public List<GameObject> GetObjects(Room room)
    {
        return ObjectData[room];
    }

    public void InstantiateObjects()
    {
        foreach(var room in Vertices)
        {
            var objects = ObjectData[room];

            foreach(var obj in objects)
            {
                GameObject mob = GameObject.Instantiate(obj);

                int x = rand.Next(0, room.FloorSize.width);
                int y = rand.Next(0, room.FloorSize.height);
                Vector3Int localPosition = new Vector3Int(room.Position.x + x, room.Position.y + y);

                mob.transform.position = tilemap.ChangeLocalToWorldPosition(localPosition);
            }
        }
    }
}


