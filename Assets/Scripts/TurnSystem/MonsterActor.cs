﻿using UnityEngine;
using TurnSystem;
using System.Collections;
using GameEntity;
using System.Collections.Generic;
using SimpleSpriteAnimator;

[DisallowMultipleComponent]
public class MonsterActor : TurnActor
{
    enum BehaviourState { IDLE,CHASE,ATTACK };

    public GameObject ActorObject { get; set; }
    [SerializeField] private ActorState ActorState;

    private readonly string scanRangeString =
        "[2,2][2,1][2,0][2,-1][2,-2]" +
        "[1,2][1,1][1,0][1,-1][1,-2]" +
        "[0,2][0,1][0,0][0,-1][0,-2]" +
        "[-1,2][-1,1][-1,0][-1,-1][-1,-2]" +
        "[-2,2][-2,1][-2,0][-2,-1][-2,-2]";
    private Vector2Int[] scanRangeCoords;
    private readonly string attackRangeString =
        "[-1,1][0,1][1,1]" +
        "[-1,0]     [1,0]" +
        "[-1,-1][0,-1][1,-1]";
    private Vector2Int[] attackRangeCoords;
    [SerializeField] private BehaviourState currentState = BehaviourState.IDLE;
    private Vector3Int LocalPosition { get => tilemapManager.ChangeWorldToLocalPosition(this.transform.position); }
    [SerializeField] private VisibilityTile scanRagneTilePrefab;
    [SerializeField] private VisibilityTile attackRangeTilePrefab;
    [SerializeField] private TilemapManager tilemapManager;
    [SerializeField] private EntityManager EntityManager;
    [SerializeField] private TileGroup TileGroup;
    private Entity target = null;

    [SerializeField] private SpriteAnimator SpriteAnimator;
    [SerializeField] private MonsterEntity MonsterEntity;
    [SerializeField] private string AnimPreifxName = "Golem";

    [SerializeField] private bool isThrowing = false;
    [SerializeField] private GameObject throwObject;
    [SerializeField] private float moveTime;


    public override IEnumerator ActionCoroutine()
    {
        ActorState = ActorState.Start;

        if(currentState == BehaviourState.IDLE)
        {
            if (AttackReady())
            {
                currentState = BehaviourState.ATTACK;
                TileGroup.Hide();
            }
            else if (PlayerDetected())
            {
                currentState = BehaviourState.CHASE;
                TileGroup.Hide();
            }
        }

        switch (currentState)
        {
            case BehaviourState.CHASE:
                tilemapManager.Navigation.SetDestination(target.transform.position);
                yield return tilemapManager.Navigation.GoDestination((TileNode)LocalPosition, end: tilemapManager.Navigation.Destination, this.transform, 1);
                currentState = BehaviourState.IDLE;
                break;
            case BehaviourState.ATTACK:
                SpriteAnimator.Play($"{AnimPreifxName}-Attack");
                for (float frame = 0.0f; frame <= SpriteAnimator.AnimationLength; frame += Time.deltaTime)
                {
                    yield return null;
                }
                if(isThrowing)
                {
                    var obj = Instantiate(throwObject);
                    obj.transform.position = this.transform.position;
                    var nVector = (target.transform.position - obj.transform.position).normalized;
                    while (Vector3.Distance(tilemapManager.ChangeWorldToLocalPosition(obj.transform.position), tilemapManager.ChangeWorldToLocalPosition(target.transform.position)) > 0.0f)
                    {
                        Debug.Log(Vector3.Distance(obj.transform.position, target.transform.position));
                        obj.transform.position += nVector * Time.deltaTime * 1.0f;
                        yield return null;
                    }
                    Destroy(obj.gameObject);
                }
                target.TakeDamage(this.MonsterEntity, 1);
                SpriteAnimator.Play($"{AnimPreifxName}-Idle");
                currentState = BehaviourState.IDLE;
                break;
        }

        ActorState = ActorState.End;
    }

    private bool PlayerDetected()
    {
        this.target = null;
        foreach (var coord in scanRangeCoords)
        {
            if (EntityManager.TryGetEntityOnTile<PlayerEntity>((Vector3Int)coord + LocalPosition, out Entity player))
            {
                this.target = player;
                return true;
            }
        }
        return false;
    }

    public bool AttackReady()
    {
        foreach (var coord in attackRangeCoords)
        {
            if (this.target == null) return false;

            if (EntityManager.TryGetEntityOnTile<PlayerEntity>((Vector3Int)coord + LocalPosition, out Entity target))
            {
                this.target = target;
                currentState = BehaviourState.ATTACK;
                return true;
            }
        }
        return false;
    }

    private void Awake()
    {
        this.transform.position = tilemapManager.RepositioningTheWorld(this.transform.position);
        ActorObject = this.gameObject;
        scanRangeCoords = CardNameSpace.Base.CoordConverter.ConvertToCoords(scanRangeString);
        attackRangeCoords = CardNameSpace.Base.CoordConverter.ConvertToCoords(attackRangeString);

        MonsterEntity.deathAction = () =>
        {
            SpriteAnimator.Play($"{AnimPreifxName}-Death");
            Destroy(this.gameObject, SpriteAnimator.AnimationLength);
        };
    }

    private void Start()
    {
        TileGroup.CreateClones(scanRagneTilePrefab, scanRangeCoords, LocalPosition);
        TileGroup.CreateClones(attackRangeTilePrefab, attackRangeCoords, LocalPosition);

        SpriteAnimator.Play($"{AnimPreifxName}-Idle");
    }
}

