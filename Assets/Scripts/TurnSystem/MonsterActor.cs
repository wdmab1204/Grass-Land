using UnityEngine;
using TurnSystem;
using System.Collections;
using GameEntity;
using System.Collections.Generic;
using SimpleSpriteAnimator;
using UnityEngine.Tilemaps;
using BehaviourTree.Tree;

[DisallowMultipleComponent]
public class MonsterActor : TurnActor
{
    enum BehaviourState { IDLE,CHASE,ATTACK };

    public GameObject ActorObject { get; set; }
    [SerializeField] private ActorState ActorState;

    private Tilemap tilemap;
    private Navigation navigation;
    [SerializeField] private TileGroup TileGroup;
    [SerializeField] private SpriteAnimator SpriteAnimator;
    [SerializeField] private MonsterEntity MonsterEntity;
    [SerializeField] private string AnimPreifxName = "Golem";

    MonsterBT behaviourTree;


    public override IEnumerator ActionCoroutine()
    {
        do
        {
            behaviourTree.Update();
            yield return null;
        } while (behaviourTree.CurrentRootNodeState != BehaviourTree.NodeState.SUCCESS);

        ActorState = ActorState.End;
    }

    private void Awake()
    {
        ActorObject = this.gameObject;

        MonsterEntity.deathAction = () =>
        {
            SpriteAnimator.Play($"{AnimPreifxName}-Death");
            Destroy(this.gameObject, SpriteAnimator.AnimationLength);
        };

        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        navigation = tilemap.CreateNavigation();
    }

    private void Start()
    {
        this.transform.position = tilemap.RepositioningTheWorld(this.transform.position);
        behaviourTree = new MonsterBT(this.transform, TileGroup);
        behaviourTree.Initialize();


        SpriteAnimator.Play($"{AnimPreifxName}-Idle");
    }
}

