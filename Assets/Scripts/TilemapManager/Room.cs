using System;
using UnityEngine.Tilemaps;
using UnityEngine;
using Random = System.Random;
using System.Collections.Generic;
using TurnSystem;

public class Room
{
	public enum ThemeType { Grassland, Deadmine, Ancientremains };
	public enum RoomType { Normal, BossRoom };

	(int width, int height) size;
	(int x, int y) position;
	(int x, int y) door;

	TileBase[,] tileBases;
	List<GameObject> mobs = new List<GameObject>();
	Tilemap tilemap;

	public (int x, int y) Door { get => door; }
	public (int x, int y) Position { get => position; }
	public Vector3 WorldPosition { get => tilemap.ChangeLocalToWorldPosition(new Vector3Int(position.x, position.y)); }
	public (int width, int height) Size { get => size; }
	public (int width, int height) FloorSize { get => (size.width - 1, size.height - 1); }
		
	public Room((int, int) size, TileBase[,] tileBases, (int, int) door, (int, int) position)
	{
		this.size = size;
		this.tileBases = tileBases;
		this.door = door;
		this.position = position;

		tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();

		GameObject mob1 = Resources.Load<GameObject>("Mob/Green Slime");
		GameObject mob2 = Resources.Load<GameObject>("Mob/Yellow Slime");
		GameObject mob3 = Resources.Load<GameObject>("Mob/Golem");

		mobs.Add(mob1);
		mobs.Add(mob2);
		mobs.Add(mob3);
	}

	public static Room CreateRandomRoom((int width, int height) size, TileBase[] groundTiles, TileBase[] wallTiles, TileBase door, (int x, int y) roomPosition)
	{
		TileBase[,] tileBases = new TileBase[size.width, size.height];
		Random rand = new Random();
		for(int w = 0; w<size.width; w++)
		{
			for(int h=0; h<size.height; h++)
			{
				if(IsWall(w,size.width,h,size.height))
				{
					//벽 타일을 배열에 삽입
                    //tileBases[w, h] = wallTiles[rand.Next(wallTiles.Length)];
                }
				else
				{
					//바닥 타일을 배열에 삽입
					tileBases[w, h] = groundTiles[rand.Next(groundTiles.Length)];
				}
			}
		}

        bool IsWall(int w, int weight, int h, int height) => ((w == weight - 1) || (h == height - 1));

		(int x, int y) doorPos = GetRandomTopRightEdgeCoordinate(size);

		tileBases[doorPos.x,doorPos.y] = door;

		Room room = new Room(size, tileBases, doorPos, roomPosition);

		return room;
    }

	public GameObject[] GetMobs()
	{
		return mobs.ToArray();
	}

	public TurnActor[] GetActors()
	{
		List<TurnActor> list = new List<TurnActor>();
		foreach(var obj in mobs)
		{
			if(obj.TryGetComponent<TurnActor>(out TurnActor turnActor))
			{
				list.Add(turnActor);
			}
		}

		return list.ToArray();
	}

	public T[] Gets<T>() => Gets<T>(-1);

	public T[] Gets<T>(LayerMask layerMask)
	{
		List<T> list = new List<T>();
		bool useLayerMask = true;
		if (layerMask == -1) useLayerMask = false;
		for(int i=0; i < size.height; i++)
		{
			for(int j=0; j<size.width; j++)
			{
				Vector3Int localPosition = new Vector3Int(position.x + i, position.y + j);
				var worldCell = tilemap.ChangeLocalToWorldPosition(localPosition);
				Debug.Log(worldCell);

				Collider2D col = null;
				if (useLayerMask)
                {
					col = Physics2D.OverlapCircle(worldCell, 0.3f, layerMask);
                }
                else
				{
					col = Physics2D.OverlapCircle(worldCell, 0.3f);
                }

				if (col != null && col.transform.parent.TryGetComponent<T>(out T result)) list.Add(result);
            }
        }

		return list.ToArray();
	}

	public void InstantiateMobs()
	{
		Random rand = new Random();

		foreach(var mobPrefab in mobs)
		{
			GameObject mob = GameObject.Instantiate(mobPrefab);

			int x = rand.Next(0, size.width);
			int y = rand.Next(0, size.height);

			Vector3Int localPosition = new Vector3Int(position.x + x, position.y + y);

			mob.transform.position = tilemap.ChangeLocalToWorldPosition(localPosition);
		}
	}


	public TileBase GetTile(int x, int y) => tileBases[x, y];

    static (int, int) GetRandomTopRightEdgeCoordinate((int width, int height) size)
    {
		(int, int)[] edges = new (int, int)[size.width + size.height - 1];
        int index = 0;
        for (int i = 0; i < size.width; i++)
        {
			edges[index++] = (i, size.height - 1);
        }
        for (int j = 0; j < size.height-1; j++)
        {
			edges[index++] = (size.width - 1, j);
        }
        Random random = new Random();
		
        return edges[random.Next(edges.Length)];
    }
}


