using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using System.Collections.Generic;
using KMolenda.Aisd.Graph;
using TMPro;


public class AddTileEvent : MonoBehaviour
{
    private Dice<int>[] dices = new Dice<int>[2];
    [SerializeField] private Tilemap tilemap;
    private MouseInput mouseInput;
    private HashSet<Vector3Int> highlightedTiles = new HashSet<Vector3Int>();
    [SerializeField] private Transform player;
    private TilemapReader TilemapReader;
    [SerializeField] private TMP_Text movePointText;
    private int movePoint;
    public int MovePoint
    {
        get
        {
            return movePoint;
        }
        set
        {
            movePoint = value;
            movePointText.text = value.ToString();
        }
    }

    //Button Click Event
    public void ThrowDice()
    {
        var movePoint = dices[0].GetRandomValue() + dices[1].GetRandomValue();
        this.MovePoint = movePoint;
    }

    private bool HighlightTile(Vector3Int tilePosition)
    {
        return true;
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
