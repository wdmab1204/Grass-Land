using System;
using UnityEngine.Tilemaps;



public class Room
{
	public enum ThemeType { Grassland, Deadmine, Ancientremains };
	public enum RoomType { Normal, BossRoom };

	(int width, int height) size;
	(int x, int y) position;
	(int x, int y) door;

	TileBase[,] tileBases;

	public (int x, int y) Door { get => door; }
	public (int x, int y) Position { get => position; }
	public (int width, int height) Size { get => size; }

	public Room((int, int) size, TileBase[,] tileBases, (int, int) door, (int, int) position)
	{
		this.size = size;
		this.tileBases = tileBases;
		this.door = door;
		this.position = position;
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


