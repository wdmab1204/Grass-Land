using UnityEngine;
using TurnSystem;
using System.Collections;
using CardNameSpace;
using System.Collections.Generic;
using SimpleSpriteAnimator;
using GameEntity;
using System;
using UnityEngine.Tilemaps;

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
public class PlayerActor : TurnActor
{
    public GameObject ActorObject { get; set; }
    public ActorState ActorState { get; set; }
    private Tilemap tilemap;
    private Dice<int>[] dices = new Dice<int>[2];

    [SerializeField] private ArrowTileGroup arrowTileGroup;
    private Vector3 destination;

    [SerializeField] private DeckHandler deckHandler;
    [SerializeField] private PlayerEntity playerEntity;
    [SerializeField] private SpriteAnimator SpriteAnimator;
    [SerializeField] private EntityManager EntityManager;

    public Action OnExitTurn;

    public override IEnumerator ActionCoroutine()
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
        var centerLocalPoint = tilemap.ChangeWorldToLocalPosition(transform.position);
        Vector3Int[] directions = new Vector3Int[4] { Vector3Int.right, Vector3Int.left, Vector3Int.up, Vector3Int.down };
        //rightup, leftdown, leftup, rightdown

        //arrowTileGroup.transform.position = this.transform.position;
        for (int i = 0; i < arrowTileGroup.childs.Length; i++)
        {
            var nearbyCoordinate = centerLocalPoint + directions[i];
            if (tilemap.HasTile(nearbyCoordinate))
            {
                arrowTileGroup.childs[i].transform.position = tilemap.ChangeLocalToWorldPosition(nearbyCoordinate);
                arrowTileGroup.childs[i].Show();
            }
        }

        yield return WaitForClickArrowTile();

        arrowTileGroup.Hide();

        var colls = Physics2D.OverlapPointAll(destination);

        foreach(var coll in colls)
        {
            if (coll.TryGetComponent<Door>(out Door door))
            {
                door.Do(this.transform);
                yield break;
            }
        }

        yield return GoDestination(destination);

        if (EntityManager.TryGetEntityOnTile<Portal>(playerEntity.LocalPosition, out Entity portal))
        {
            ((Portal)portal).LoadScene();
        }
        
    }
    bool canMove = false;
    IEnumerator WaitForClickArrowTile()
    {
        yield return new WaitUntil(() => canMove);
        canMove = false;
    }

    private void Awake()
    {
        ActorObject = this.gameObject;

        dices[0] = new Dice<int>(new int[6] { 1, 2, 3, 4, 5, 6 });
        dices[1] = new Dice<int>(new int[6] { 1, 2, 3, 4, 5, 6 });
        var movePoint = dices[0].GetRandomValue() + dices[1].GetRandomValue();
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
    }

    private void Start()
    {
        this.transform.position = tilemap.RepositioningTheWorld(this.transform.position);

        arrowTileGroup.ClickEvent += position =>
        {
            canMove = true;
            this.destination = position;
        };

        playerEntity.OnDeath = () =>
        {
            SpriteAnimator.Play("Player-Death");
        };
    }
}

