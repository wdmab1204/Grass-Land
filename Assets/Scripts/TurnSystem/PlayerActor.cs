using UnityEngine;
using TurnSystem;
using System.Collections;
using CardNameSpace;
using System.Collections.Generic;
using SimpleSpriteAnimator;
using GameEntity;
using System;

public static class AnimationConverter
{

    public static Dictionary<string,string> GetDics()
    {
        Dictionary<string, string> animationDics = new Dictionary<string, string>();
        animationDics.Add("검기 발사", "Swing Sword");
        animationDics.Add("파이어 볼", "Throw Fireball");
        animationDics.Add("자가 치유", "Drink Potion");
        animationDics.Add("화살 발사", "Shoot an Arrow");
        animationDics.Add("아이스 스피어", "Throw Icespear");

        return animationDics;
    }
}


[DisallowMultipleComponent]
public class PlayerActor : MonoBehaviour, ITurnActor
{
    public GameObject ActorObject { get; set; }
    public ActorState ActorState { get; set; }
    [SerializeField] private TilemapReader tilemapReader;
    private Dice<int>[] dices = new Dice<int>[2];

    private Navigation navigation;
    [SerializeField] private DestinationTile highlightTilePrefab0;
    [SerializeField] private DestinationTile highlightTilePrefab1;
    [SerializeField] private DestinationTile highlightTilePrefab2;
    [SerializeField] private DestinationTile highlightTilePrefab3;
    private DestinationTile[] highlightTiles;

    [SerializeField] private DeckHandler deckHandler;
    [SerializeField] private PlayerEntity playerEntity;
    [SerializeField] private SpriteAnimator SpriteAnimator;
    [SerializeField] private EntityManager EntityManager;

    public Action OnExitTurn;

    public IEnumerator ActionCoroutine()
    {
        ActorState = ActorState.Start;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                yield return CardAction();
                break;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                yield return MoveAction();
                break;
            }
            yield return null;
        }

        OnExitTurn?.Invoke();

        ActorState = ActorState.End;
    }

    private IEnumerator CardAction()
    {
        // hand card show
        deckHandler.Show();

        /// Wait for player to select and use a card
        yield return deckHandler.WaitForClickCard();

        // card effect and animation
        Debug.Log("Card using....!");
        // end
        deckHandler.Hide();
        yield return null;
    }

    private IEnumerator MoveAction()
    {
        var centerPoint = tilemapReader.ChangeWorldToLocalPosition(transform.position);
        Vector3Int[] directions = new Vector3Int[4] { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };
        //rightup, leftdown, leftup, rightdown
        for (int i = 0; i < highlightTiles.Length; i++)
        {
            var nearbyCoordinate = centerPoint + directions[i];
            if (tilemapReader.HasTile(nearbyCoordinate))
            {
                highlightTiles[i].transform.position = tilemapReader.ChangeLocalToWorldPosition(nearbyCoordinate);
                highlightTiles[i].Show();
            }
        }

        yield return navigation.WaitForClickDestination();

        var destination = navigation.Destination;

        for (int i = 0; i < highlightTiles.Length; i++)
        {
            highlightTiles[i].Hide();
        }

        yield return navigation.GoDestination(end: destination, actor: transform);

        if(EntityManager.TryGetEntityOnTile<Portal>(playerEntity.LocalPosition, out Entity portal))
        {
            ((Portal)portal).LoadScene();
        }
        
    }

    private void Awake()
    {
        ActorObject = this.gameObject;
        navigation = new Navigation(tilemapReader);
        this.transform.position = tilemapReader.RepositioningTheWorld(this.transform.position);

        dices[0] = new Dice<int>(new int[6] { 1, 2, 3, 4, 5, 6 });
        dices[1] = new Dice<int>(new int[6] { 1, 2, 3, 4, 5, 6 });
        var movePoint = dices[0].GetRandomValue() + dices[1].GetRandomValue();

        highlightTiles = new DestinationTile[4];

        highlightTiles[0] = Instantiate(highlightTilePrefab0.gameObject).GetComponent<DestinationTile>();
        highlightTiles[0].clickEvent = navigation.SetDestination;
        highlightTiles[1] = Instantiate(highlightTilePrefab1.gameObject).GetComponent<DestinationTile>();
        highlightTiles[1].clickEvent = navigation.SetDestination;
        highlightTiles[2] = Instantiate(highlightTilePrefab2.gameObject).GetComponent<DestinationTile>();
        highlightTiles[2].clickEvent = navigation.SetDestination;
        highlightTiles[3] = Instantiate(highlightTilePrefab3.gameObject).GetComponent<DestinationTile>();
        highlightTiles[3].clickEvent = navigation.SetDestination;
        playerEntity.OnDeath = () =>
        {
            SpriteAnimator.Play("Player-Death");
        };

    }
}

