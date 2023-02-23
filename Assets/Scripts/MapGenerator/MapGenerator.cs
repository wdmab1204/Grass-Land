using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(ThemeTileGroup))]
public class MapGenerator : MonoBehaviour
{
    private Tilemap tilemap;
    private ThemeTileGroup themeTileGroup;

    private void Awake()
    {
        themeTileGroup = GetComponent<ThemeTileGroup>();
        tilemap = GetComponent<Tilemap>();
    }

    public void CreateMapAndApply(Room.ThemeType themeType, Room.RoomType roomType, (int width, int height) size, Vector3Int roomPos)
    {
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

        Room room = Room.CreateRandomRoom(size.width, size.height, themeType, roomType, theme.grounds, theme.walls, theme.doors[0]);

        for(int i=0; i<size.width; i++)
        {
            for(int j=0; j<size.height; j++)
            {
                tilemap.SetTile(new Vector3Int(i + roomPos.x, j + roomPos.y, 0), room.GetTile(i, j));
            }
        }
    }
}
