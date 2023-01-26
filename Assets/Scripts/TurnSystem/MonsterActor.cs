using UnityEngine;
using TurnSystem;
using System.Collections;
using GameEntity;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class MonsterActor : MonoBehaviour, ITurnActor
{
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
        "[-1,0]     [0,1]" +
        "[-1,-1][-1,0][-1,1]";
    [SerializeField] private bool isFollowing = false;
    private Navigation navigation;
    private Vector3Int LocalPosition { get => TilemapReader.ChangeWorldToLocalPosition(this.transform.position); }
    [SerializeField] private VisibilityTile scanRagneTilePrefab;
    [SerializeField] private TilemapReader TilemapReader;
    [SerializeField] private EntityManager EntityManager;
    [SerializeField] private TileGroup TileGroup;
    private Entity target = null;

    private int pathIndex = 0;
    

    public IEnumerator ActionCoroutine()
    {
        ActorState = ActorState.Start;
        
        if (isFollowing)
        {
            //시야범위안에 있다면 쫒아감.
            navigation.SetDestination(target.transform.position);
            yield return navigation.GoDestination((TileNode)LocalPosition, end: navigation.Destination, this.transform, 1);
        }

        else
        {
            //플레이어가 시야범위안에 들어왔는지 체크
            foreach (var coord in scanRangeCoords)
            {
                if (EntityManager.TryGetEntityOnTile<PlayerEntity>((Vector3Int)coord + LocalPosition, out Entity target))
                {
                    isFollowing = true;
                    this.target = target;
                    TileGroup.Hide();
                    navigation.SetDestination(target.transform.position);
                    yield return navigation.GoDestination((TileNode)LocalPosition, end: navigation.Destination, this.transform, 1);

                }
            }
        }
        ActorState = ActorState.End;
    }

    private void Awake()
    {
        navigation = new Navigation(TilemapReader);
        this.transform.position = TilemapReader.RepositioningTheWorld(this.transform.position);
        ActorObject = this.gameObject;
        scanRangeCoords = CardNameSpace.Base.CoordConverter.ConvertToCoords(scanRangeString);
    }

    private void Start()
    {
        TileGroup.CreateClones(scanRagneTilePrefab, scanRangeCoords, LocalPosition);
    }
}

