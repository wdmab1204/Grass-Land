using UnityEngine;
using TurnSystem;
using System.Collections;

[DisallowMultipleComponent]
public class PlayerActor : MonoBehaviour, ITurnActor
{
    public GameObject Actor { get; set; }
    public ActorState ActorState { get; set; }
    private Navigation navigation;
    [SerializeField] private TilemapReader tilemapReader;
    private Dice<int>[] dices = new Dice<int>[2];

    [SerializeField] private HighlightTile highlightTilePrefab;
    private HighlightTile[] highlightTiles;

    public IEnumerator ActionCoroutine()
    {
        ActorState = ActorState.Start;

        var tilePos = tilemapReader.ChangeWorldToLocalPosition(transform.position);
        highlightTiles[0].transform.position = tilemapReader.ChangeLocalToWorldPosition(new Vector3Int(tilePos.x + 1, tilePos.y, tilePos.z));
        highlightTiles[1].transform.position = tilemapReader.ChangeLocalToWorldPosition(new Vector3Int(tilePos.x - 1, tilePos.y, tilePos.z));
        highlightTiles[2].transform.position = tilemapReader.ChangeLocalToWorldPosition(new Vector3Int(tilePos.x, tilePos.y + 1, tilePos.z));
        highlightTiles[3].transform.position = tilemapReader.ChangeLocalToWorldPosition(new Vector3Int(tilePos.x, tilePos.y - 1, tilePos.z));
        highlightTiles[0].Show();
        highlightTiles[1].Show();
        highlightTiles[2].Show();
        highlightTiles[3].Show();

        yield return navigation.WaitForClickDestination();
        
        var destination = navigation.Destination;

        yield return navigation.GoDestination(end: destination, target: transform);

        highlightTiles[0].Hide();
        highlightTiles[1].Hide();
        highlightTiles[2].Hide();
        highlightTiles[3].Hide();

        ActorState = ActorState.End;
    }

    private void Awake()
    {
        Actor = this.gameObject;
        navigation = new Navigation(tilemapReader);

        dices[0] = new Dice<int>(new int[6] { 1, 2, 3, 4, 5, 6 });
        dices[1] = new Dice<int>(new int[6] { 1, 2, 3, 4, 5, 6 });
        var movePoint = dices[0].GetRandomValue() + dices[1].GetRandomValue();

        highlightTiles = new HighlightTile[4];
        highlightTiles[0] = Instantiate(highlightTilePrefab.gameObject).GetComponent<HighlightTile>();
        highlightTiles[1] = Instantiate(highlightTilePrefab.gameObject).GetComponent<HighlightTile>();
        highlightTiles[2] = Instantiate(highlightTilePrefab.gameObject).GetComponent<HighlightTile>();
        highlightTiles[3] = Instantiate(highlightTilePrefab.gameObject).GetComponent<HighlightTile>();

        highlightTiles[0].clickEvent = navigation.CreateDestination;
        highlightTiles[1].clickEvent = navigation.CreateDestination;
        highlightTiles[2].clickEvent = navigation.CreateDestination;
        highlightTiles[3].clickEvent = navigation.CreateDestination;
        
    }
}

