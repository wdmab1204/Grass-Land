using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(ThemeTileGroup))]
public class MapGenerator : MonoBehaviour
{
    public Door doorPrefab;

    private Tilemap tilemap;
    private TilemapReader TilemapReader;
    private ThemeTileGroup themeTileGroup;

    private void Awake()
    {
        themeTileGroup = GetComponent<ThemeTileGroup>();
        tilemap = GetComponent<Tilemap>();
        TilemapReader = GetComponent<TilemapReader>();
    }

    public void CreateMapAndApply(Room.ThemeType themeType, Room.RoomType roomType, (int width, int height) size)
    {
        int padding = 2;
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                Vector3Int roomPos = new Vector3Int(i * (size.width + padding), j * (size.height + padding), 0);
                ThemeTileGroup.ThemeTile theme;
                switch (themeType)
                {
                    case Room.ThemeType.Grassland:
                        theme = themeTileGroup.grassland;
                        break;

                    case Room.ThemeType.Deadmine:
                        theme = themeTileGroup.deadmine;
                        break;

                    case Room.ThemeType.Ancientremains:
                        theme = themeTileGroup.ancientremains;
                        break;

                    default:
                        return;
                }

                Room room = Room.CreateRandomRoom(size, themeType, roomType, theme.grounds, theme.walls, theme.doors[0], (roomPos.x, roomPos.y));
                var doorObject = Instantiate(doorPrefab);
                doorObject.transform.position = TilemapReader.ChangeLocalToWorldPosition(new Vector3Int(room.Position.x + room.Door.x, room.Position.y + room.Door.y, 0));

                for (int x = 0; x < size.width; x++)
                {
                    for (int y = 0; y < size.height; y++)
                    {
                        tilemap.SetTile(new Vector3Int(x + roomPos.x, y + roomPos.y, 0), room.GetTile(x, y));
                    }
                }
            }
        }
        
    }
}
