using UnityEngine;
using TurnSystem;
using System.Collections;
using GameEntity;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class MonsterActor : MonoBehaviour, ITurnActor
{
    enum BehaviourState { IDLE,FOLLOW,ATTACK };

    public GameObject ActorObject { get; set; }
    public ActorState ActorState { get; set; }

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
    [SerializeField] private BehaviourState behaviour = BehaviourState.IDLE;
    private Navigation navigation;
    private Vector3Int LocalPosition { get => TilemapReader.ChangeWorldToLocalPosition(this.transform.position); }
    [SerializeField] private VisibilityTile scanRagneTilePrefab;
    [SerializeField] private VisibilityTile attackRangeTilePrefab;
    [SerializeField] private TilemapReader TilemapReader;
    [SerializeField] private EntityManager EntityManager;
    [SerializeField] private TileGroup TileGroup;
    private Entity target = null;

    private int pathIndex = 0;
    

    public IEnumerator ActionCoroutine()
    {
        ActorState = ActorState.Start;

        //플레이어가 시야범위안에 들어왔는지 체크
        foreach (var coord in scanRangeCoords)
        {
            if (EntityManager.TryGetEntityOnTile<PlayerEntity>((Vector3Int)coord + LocalPosition, out Entity target))
            {
                behaviour = BehaviourState.FOLLOW;
                this.target = target;
                TileGroup.Hide();
                break;
            }
        }

        foreach(var coord in attackRangeCoords)
        {
            if (this.target == null) break;

            if (EntityManager.TryGetEntityOnTile<PlayerEntity>((Vector3Int)coord + LocalPosition, out Entity target))
            {
                behaviour = BehaviourState.ATTACK;
                this.target = target;
                break;
            }
        }

        if (behaviour == BehaviourState.FOLLOW)
        {
            //시야범위안에 있다면 쫒아감.
            navigation.SetDestination(target.transform.position);
            yield return navigation.GoDestination((TileNode)LocalPosition, end: navigation.Destination, this.transform, 1);
        }
        else if(behaviour == BehaviourState.ATTACK)
        {
            target.TakeDamage(100);
        }

        ActorState = ActorState.End;
    }

    private void Awake()
    {
        navigation = new Navigation(TilemapReader);
        this.transform.position = TilemapReader.RepositioningTheWorld(this.transform.position);
        ActorObject = this.gameObject;
        scanRangeCoords = CardNameSpace.Base.CoordConverter.ConvertToCoords(scanRangeString);
        attackRangeCoords = CardNameSpace.Base.CoordConverter.ConvertToCoords(attackRangeString);
    }

    private void Start()
    {
        TileGroup.CreateClones(scanRagneTilePrefab, scanRangeCoords, LocalPosition);
        TileGroup.CreateClones(attackRangeTilePrefab, attackRangeCoords, LocalPosition);
    }
}

