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
    private TileGroup tilegroup;
    private SpriteAnimator animator;
    private MonsterEntity entity;
    [SerializeField] private string AnimPreifxName = "Golem";

    MeleeMonsterBT behaviourTree;


    public override IEnumerator ActionCoroutine()
    {
        do
        {
            behaviourTree.Update();
            yield return null;
        } while (behaviourTree.CurrentRootNodeState == BehaviourTree.NodeState.RUNNING);

        ActorState = ActorState.End;
    }

    private void Awake()
    {
        tilegroup = GetComponent<TileGroup>();
        animator = transform.GetChild(0).GetComponent<SpriteAnimator>();
        entity = GetComponent<MonsterEntity>();

        ActorObject = this.gameObject;

        entity.deathAction = () =>
        {
            animator.Play("Death");
            Destroy(this.gameObject, animator.AnimationLength);
        };

        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        navigation = tilemap.CreateNavigation();
    }

    private void Start()
    {
        this.transform.position = tilemap.RepositioningTheWorld(this.transform.position);
        behaviourTree = new MeleeMonsterBT(this.transform, tilegroup);
        behaviourTree.Initialize();


        animator.Play("Idle");
    }
}

