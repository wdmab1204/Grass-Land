using System.Collections;
using System.Collections.Generic;
using KMolenda.Aisd.Graph;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(ThemeTileGroup))]
public class TilemapManager : MonoBehaviour
{
    public Door doorPrefab;

    private Tilemap tilemap;
    private TilemapGraph tilemapGraph;
    private ThemeTileGroup themeTileGroup;

    private Navigation navigation;
    public Navigation Navigation { get => navigation; }
    public Graph<TileNode> Graph { get => tilemapGraph; }

    private Map map;

    private void Awake()
    {
        themeTileGroup = GetComponent<ThemeTileGroup>();
        tilemap = GetComponent<Tilemap>();

        int roomCount = 9;
        map = new Map(roomCount);
    }

    public Vector3 ChangeLocalToWorldPosition(Vector3Int position) => tilemap.GetCellCenterWorld(position);

    public Vector3Int ChangeWorldToLocalPosition(Vector3 worldPosition) => tilemap.WorldToCell(worldPosition);

    public Vector3 RepositioningTheWorld(Vector3 worldPosition) => tilemap.GetCellCenterWorld(tilemap.WorldToCell(worldPosition));

    public bool HasTile(Vector3Int position) => tilemap.HasTile(position);

    public bool HasTile(Vector3 worldPosition) => tilemap.HasTile(ChangeWorldToLocalPosition(worldPosition));

    public void InitializeMapAndApply((Room.ThemeType, Room.RoomType) mapType, (int width, int height) roomSize)
    {
        InitMap(mapType, roomSize);
        ApplyMap();
    }

    public void ApplyMap()
    {
        if (map == null) throw new System.NullReferenceException();

        foreach(Room room in map.Vertices)
        {
            for (int x = 0; x < room.Size.width; x++)
            {
                for (int y = 0; y < room.Size.height; y++)
                {
                    tilemap.SetTile(new Vector3Int(x + room.Position.x, y + room.Position.y, 0), room.GetTile(x, y));
                }
            }
        }
    }

    private void InitMap((Room.ThemeType themeType, Room.RoomType roomType) mapType, (int width, int height) roomSize)
    {
        int padding = 2;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3Int roomPosition = new Vector3Int(i * (roomSize.width + padding), j * (roomSize.height + padding), 0);
                ThemeTileGroup.ThemeTile theme;
                switch (mapType.themeType)
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

                Room room = Room.CreateRandomRoom(roomSize, theme.grounds, theme.walls, theme.doors[0], (roomPosition.x, roomPosition.y));
                var doorObject = Instantiate(doorPrefab);
                doorObject.transform.position = ChangeLocalToWorldPosition(new Vector3Int(room.Position.x + room.Door.x, room.Position.y + room.Door.y, 0));

                map.AddVertex(room);
            }
        }

    }

    public void InitializeNavigation()
    {
        tilemapGraph = new TilemapGraph(tilemap);

        foreach(var node in tilemapGraph.Vertices)
        {
            Debug.Log(node.position);
        }

        navigation = new Navigation(this);
    }

}
