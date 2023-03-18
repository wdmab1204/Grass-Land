using System.Collections;
using System.Collections.Generic;
using KMolenda.Aisd.Graph;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace UnityEngine.Tilemaps
{
    public static class TilemapExtensions
    {
        public static Vector3 ChangeLocalToWorldPosition(this Tilemap tilemap, Vector3Int position) => tilemap.GetCellCenterWorld(position);

        public static Vector3Int ChangeWorldToLocalPosition(this Tilemap tilemap, Vector3 worldPosition) => tilemap.WorldToCell(worldPosition);

        public static Vector3 RepositioningTheWorld(this Tilemap tilemap, Vector3 worldPosition) => tilemap.GetCellCenterWorld(tilemap.WorldToCell(worldPosition));

        public static bool HasTile(this Tilemap tilemap, Vector3Int position) => tilemap.HasTile(position);

        public static bool HasTile(this Tilemap tilemap, Vector3 worldPosition) => tilemap.HasTile(ChangeWorldToLocalPosition(tilemap, worldPosition));

        public static Navigation CreateNavigation(this Tilemap tilemap)
        {
            TilemapGraph tilemapGraph = new TilemapGraph(tilemap);

            Navigation navigation = new Navigation(tilemapGraph);

            return navigation;
        }
    }
}

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
        List<Room> roomList = new List<Room>();
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
                roomList.Add(room);
                map.AddVertex(room);
            }
        }

        map.AddEdge(roomList[0], roomList[1]);
        map.AddEdge(roomList[0], roomList[3]);
        map.AddEdge(roomList[1], roomList[2]);
        map.AddEdge(roomList[1], roomList[4]);
        map.AddEdge(roomList[2], roomList[5]); 
        map.AddEdge(roomList[2], roomList[5]); 
        map.AddEdge(roomList[3], roomList[4]); 
        map.AddEdge(roomList[3], roomList[6]);
        map.AddEdge(roomList[4], roomList[5]);
        map.AddEdge(roomList[4], roomList[7]);
        map.AddEdge(roomList[5], roomList[8]);
        map.AddEdge(roomList[6], roomList[7]);
        map.AddEdge(roomList[7], roomList[8]);

        CreateDoor();
    }

    private void CreateDoor()
    {
        foreach(var room in map.AdjacencyList)
        {
            foreach(var nextRoom in room.Value)
            {
                var door = Instantiate(doorPrefab);
                door.transform.position = tilemap.ChangeLocalToWorldPosition(new Vector3Int(room.Key.Position.x + room.Key.Door.x, room.Key.Position.y + room.Key.Door.y, 0));
                door.nextPosition = tilemap.ChangeLocalToWorldPosition(new Vector3Int(nextRoom.Position.x, nextRoom.Position.y, 0));
            }
        }
    }

    //public void InitializeNavigation()
    //{
    //    tilemapGraph = new TilemapGraph(tilemap);

    //    foreach(var node in tilemapGraph.Vertices)
    //    {
    //        Debug.Log(node.position);
    //    }

    //    navigation = new Navigation<TileNode>(this.tilemapGraph);
    //}

}
