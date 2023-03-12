﻿
using System.Collections.Generic;
using BehaviourTree.Tree.GuartBT;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameEntity;

namespace BehaviourTree.Tree
{
	public class MonsterBT : Tree
	{
        protected readonly string scanRangeString =
        "[2,2][2,1][2,0][2,-1][2,-2]" +
        "[1,2][1,1][1,0][1,-1][1,-2]" +
        "[0,2][0,1][0,0][0,-1][0,-2]" +
        "[-1,2][-1,1][-1,0][-1,-1][-1,-2]" +
        "[-2,2][-2,1][-2,0][-2,-1][-2,-2]";
        private Range scanRange;
        protected readonly string attackRangeString =
            "[-1,1][0,1][1,1]" +
            "[-1,0]     [1,0]" +
            "[-1,-1][0,-1][1,-1]";
        private Range attackRange;

        public LayerMask layermask;

        private Tilemap tilemap;

        protected virtual void Awake()
        {
            tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
            scanRange = new Range(transform, scanRangeString, tilemap);
            attackRange = new Range(transform, attackRangeString, tilemap);
        }

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override Node SetupBehaviourtree()
        {
            Node root = new Selector(new List<Node>()
            {
                new Sequence(new List<Node>()
                {
                    new CheckTargetInFOVRange(scanRange),
                    new TaskGoToTarget(transform),
                })
                , new TaskIdle()
            });

            return root;
        }
    }

    class CheckTargetInFOVRange : Node
    {
        private readonly LayerMask playerLayerMask;

        private Range range;
        
        public CheckTargetInFOVRange(Range range)
        {
            this.range = range;
        }

        public override NodeState Evaluate()
        {
            object target = GetData("target");
            if (target == null)
            {
                foreach (var coord in range.worldCoords)
                {
                    var result = Physics2D.OverlapCircleAll(coord, 1.0f, 1<<LayerMask.NameToLayer("Player"));
                    if (result.Length > 0)
                    {
                        parent.parent.SetData("target", result[0].transform);
                        state = NodeState.SUCCESS;
                        return state;
                    }
                    else
                    {
                        state = NodeState.FAILURE;
                        return state;
                    }
                }
            }

            return NodeState.SUCCESS;
        }
    }

    class TaskGoToTarget : Node
    {
        private UnityEngine.Transform transform;
        public TaskGoToTarget(Transform transform)
        {
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (target == null) return NodeState.FAILURE;

            if (Vector2.Distance(transform.position, target.position) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position, target.position, GuardBT.speed * Time.deltaTime);
            }

            state = NodeState.RUNNING;
            return state;
        }
    }

    class TaskIdle : Node
    {
        private float _waitTime = 1.0f;
        private float _waitCounter = 0.0f;
        private bool _waiting;
        public TaskIdle()
        {

        }

        public override NodeState Evaluate()
        {
            //idle animation

            return NodeState.RUNNING;
        }
    }

    class TaskAttackToTarget : Node
    {
        private Entity entity;
        public TaskAttackToTarget(Transform transform)
        {
            entity = transform.GetComponent<Entity>();
        }

        public override NodeState Evaluate()
        {
            GameEntity.Entity entity = ((Transform)GetData("target")).GetComponent<GameEntity.Entity>();
            entity.TakeDamage(10);
            return NodeState.SUCCESS;
        }
    }
}

