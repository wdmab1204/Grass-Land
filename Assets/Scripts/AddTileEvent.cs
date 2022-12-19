using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using KMolenda.Aisd.Graph;


public class AddTileEvent : MonoBehaviour
{
    private Dice<int>[] dices = new Dice<int>[2];
    [SerializeField] private Tilemap tilemap;
    private MouseInput mouseInput;
    private HashSet<Vector3Int> highlightedTiles = new HashSet<Vector3Int>();
    [SerializeField] private Transform player;
    [SerializeField] private TilemapReader TilemapReader;

    //Button Click Event
    public void ThrowDice()
    {
        var movePoint = dices[0].GetRandomValue() + dices[1].GetRandomValue();
        var playerPos = tilemap.WorldToCell(player.position);
        //주사위만큼 갈수있는 거리를 화면에 타일로 표시

        //debug
        movePoint = 3;
        for (int x = 0; x <= movePoint; x++)
        {
            int y = movePoint - x;
            HighlightTile(new Vector3Int(playerPos.x + x, playerPos.y + y, 0));
            HighlightTile(new Vector3Int(playerPos.x + x, playerPos.y - y, 0));
            HighlightTile(new Vector3Int(playerPos.x - x, playerPos.y + y, 0));
            HighlightTile(new Vector3Int(playerPos.x - x, playerPos.y - y, 0));
        }
        isPlaying = true;
    }

    private void HighlightTile(Vector3Int tileLocalPosition)
    {

        if (!tilemap.HasTile(tileLocalPosition))
        {
            Debug.LogError("TileNode DOES NOT EXIST" + tilemap.GetCellCenterWorld(tileLocalPosition));
            return;
        }

        //if (highlightedTiles.Contains(tileLocalPosition)) return;


        //get tile from tileLocalPosition
        Tile switchableTile = tilemap.GetTile<Tile>(tileLocalPosition);



        //if tile does exist and it have SwitchableTile Class
        if (switchableTile != null && switchableTile is SwitchableTile)
        {
            //change sprite
            Sprite sprite = ((SwitchableTile)switchableTile).GetNextSprite();
            ((SwitchableTile)switchableTile).sprite = sprite;


            tilemap.RefreshTile(tileLocalPosition);

            highlightedTiles.Add(tileLocalPosition);

            Debug.Log(tileLocalPosition);
        }
    }

    private void MouseClickEvent()
    {
        Vector2 mousePosition = mouseInput.Mouse.MousePosition.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // make sure we are clicking the cell
        Vector3Int selectedTileLocalPos = tilemap.WorldToCell(mousePosition);

        Debug.Log("Selected Pos : " + selectedTileLocalPos.ToString());
        //has tile and it is highlightedTile
        if (tilemap.HasTile(selectedTileLocalPos) && highlightedTiles.Contains(selectedTileLocalPos))
        {
            var start = tilemap.WorldToCell(player.position);
            var end = selectedTileLocalPos;

            StartCoroutine(GoDestination(new TileNode(start), new TileNode(end)));
        }
    }

    private IEnumerator GoDestination(TileNode start, TileNode end)
    {

        //Find shortest path 
        foreach (var next in TilemapReader.Graph.ShortestPath(start, end))
        {
            var nextPos = tilemap.GetCellCenterWorld(next.position);
            while (Vector3.Distance(player.position, nextPos) > 0.0f)
            {
                player.position = Vector3.MoveTowards(player.position, nextPos, Time.deltaTime);
                yield return null;
            }
        }
    }

    private void Awake()
    {
        mouseInput = new MouseInput();
        dices = new Dice<int>[2];
        dices[0] = new Dice<int>(new int[6] { 1, 2, 3, 4, 5, 6 });
        dices[1] = new Dice<int>(new int[6] { 1, 2, 3, 4, 5, 6 });
        TilemapReader = GetComponent<TilemapReader>();
    }

    bool isPlaying = false;
    private void Start()
    {
        mouseInput.Mouse.MouseClick.performed += _ => MouseClickEvent();
        //HighlightTile(Vector3Int.zero);
    }

    private void OnEnable()
    {
        mouseInput.Enable();
    }

    private void OnDisable()
    {
        mouseInput.Disable();
    }

    private void OnDrawGizmos()
    {
        if (!isPlaying) return;

        foreach (var v in highlightedTiles)
        {
            Gizmos.color = Color.red;
            var worldPosition = tilemap.GetCellCenterWorld(v);
            Gizmos.DrawCube(worldPosition, Vector3.one * 0.5f);
        }
    }
}
