using System;
using UnityEngine.Tilemaps;



public class Room
{
    public enum ThemeType { Grassland, Deadmine, Ancientremains };
    public enum RoomType { Normal, BossRoom };

	(int width, int height) size;
	(int x, int y) start;
	(int x, int y) door;

	ThemeType themeType;
	RoomType roomType;
	TileBase[,] tileBases;

	public Room(int width, int height, ThemeType themeType, RoomType roomType, TileBase[,] tileBases, (int, int) door)
	{
		this.size.width = width;
		this.size.height = height;
		this.themeType = themeType;
		this.roomType = roomType;
		this.tileBases = tileBases;
		this.door = door;
	}

	public static Room CreateRandomRoom(int width, int height, ThemeType themeType, RoomType roomType, TileBase[] groundTiles, TileBase[] wallTiles, TileBase door)
	{
		TileBase[,] tileBases = new TileBase[width, height];
		Random rand = new Random();
		for(int w = 0; w<width; w++)
		{
			for(int h=0; h<height; h++)
			{
				if(IsWall(w,width,h,height))
				{
                    //tileBases[w, h] = wallTiles[rand.Next(wallTiles.Length)];
                }
				else
				{
					tileBases[w, h] = groundTiles[rand.Next(groundTiles.Length)];
				}
			}
		}

        bool IsWall(int w, int weight, int h, int height) => ((w == weight - 1) || (h == height - 1));

		(int x, int y) doorPos = GetRandomTopRightEdgeCoordinate(width, height);

		tileBases[doorPos.x,doorPos.y] = door;

		Room room = new Room(width, height, themeType, roomType, tileBases, doorPos);

		return room;
    }

	public TileBase GetTile(int x, int y) => tileBases[x, y];

    static (int, int) GetRandomTopRightEdgeCoordinate(int width, int height)
    {
		(int, int)[] edges = new (int, int)[width + height - 1];
        int index = 0;
        for (int i = 0; i < width; i++)
        {
			edges[index++] = (i, height - 1);
        }
        for (int j = 0; j < height-1; j++)
        {
			edges[index++] = (width - 1, j);
        }
        Random random = new Random();
		
        return edges[random.Next(edges.Length)];
    }
}


