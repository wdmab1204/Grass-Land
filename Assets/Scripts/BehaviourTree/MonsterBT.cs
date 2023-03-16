
using System.Collections.Generic;
using BehaviourTree.Tree;
using UnityEngine;
using UnityEngine.Tilemaps;
using GameEntity;
using SimpleSpriteAnimator;
using static UnityEngine.GraphicsBuffer;

namespace BehaviourTree.Tree
{
	public class MonsterBT : Tree
	{
        protected readonly string scanRangeString =
        "[2,2][2,1][2,0][2,-1][2,-2]" +
        "[1,2]                [1,-2]" +
        "[0,2]                [0,-2]" +
        "[-1,2]               [-1,-2]" +
        "[-2,2][-2,1][-2,0][-2,-1][-2,-2]";
        private Range scanRange;
        protected readonly string attackRangeString =
            "[-1,1][0,1][1,1]" +
            "[-1,0][0,0][1,0]" +
            "[-1,-1][0,-1][1,-1]";
        private Range attackRange;

        public LayerMask layermask;

        private Tilemap tilemap;

        private Navigation navigation;

        private TileGroup tileGroup;

        Transform transform;

        public MonsterBT(Transform transform, TileGroup tileGroup)
        {
            this.transform = transform;
            this.tileGroup = tileGroup;
        }

        public override void Initialize()
        {
            tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
            navigation = tilemap.CreateNavigation();
            scanRange = new Range(scanRangeString, tilemap);
            attackRange = new Range(attackRangeString, tilemap);

            base.Initialize();

            transform.position = tilemap.RepositioningTheWorld(transform.position);

            tileGroup.CreateClones("range-tile_1", scanRange, transform.position);
            tileGroup.CreateClones("range-tile_0", attackRange, transform.position);
        }

        public void OnDrawGizmos()
        {
            if (scanRange == null) return;

            foreach(var coord in scanRange.worldCoords)
            {
                Vector3 worldPos = transform.position + coord;
                Gizmos.DrawCube(worldPos, Vector3Int.one);
            }
        }

        public override void Update()
        {
            base.Update();
        }

        protected override Node SetupBehaviourtree()
        {
            Node root = new Selector(new List<Node>()
            {
                new Sequence(new List<Node>()
                {
                    new CheckTargetInAttackRange(transform,attackRange),
                    new TaskAttackToTarget(transform),
                })
                ,
                new Sequence(new List<Node>()
                {
                    new CheckTargetInFOVRange(transform, scanRange),
                    new TaskGoToTarget(transform,navigation, tileGroup),
                })
                , new TaskIdle()
            });

            return root;
        }
    }

    class CheckTargetInFOVRange : Node
    {
        private readonly LayerMask playerLayerMask = 1 << LayerMask.NameToLayer("Player");

        Transform transform;
        Range range;
        
        public CheckTargetInFOVRange(Transform transform, Range range)
        {
            this.transform = transform;
            this.range = range;
        }

        public override NodeState Evaluate()
        {
            object target = GetData("target");
            if (target == null)
            {
                foreach (var cellWorldPosition in range.worldCoords)
                {
                    Vector3 tileWorldPosition = transform.position + cellWorldPosition;
                    var result = Physics2D.OverlapCircleAll(tileWorldPosition, 0.3f, 1 << LayerMask.NameToLayer("Player"));
                    if (result.Length > 0)
                    {
                        parent.parent.SetData("target", result[0].transform);
                        state = NodeState.SUCCESS;

                        return state;
                    }
                }

                state = NodeState.FAILURE;
                return state;
            }

            return NodeState.SUCCESS;
        }
    }

    class TaskGoToTarget : Node
    {
        private Transform transform;
        private Navigation navigation;
        private TileGroup tilegroup;
        private SpriteAnimator anim;
        private float speed = 1.0f;

        Vector3 nextPosition;

        private bool calledInitMethod;

        public TaskGoToTarget(Transform transform, Navigation navigation, TileGroup tilegroup)
        {
            this.transform = transform;
            this.navigation = navigation;
            this.tilegroup = tilegroup;
            this.anim = transform.GetChild(0).GetComponent<SpriteAnimator>();
            calledInitMethod = false;
        }

        void Init(Transform target)
        {
            int moveDistance = 1;
            Navigation.Path path = navigation.GetShortestPath(transform.position, target.position);
            nextPosition = path[moveDistance];

            anim.Play("Golem-Idle");

            tilegroup.Hide();
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (calledInitMethod == false)
            {
                Init(target);
                calledInitMethod = true;
            }

            if (Vector3.Distance(transform.position, nextPosition) > 0.01f)
            {
                // 목표 위치까지 이동 벡터 계산
                Vector3 direction = (nextPosition - transform.position).normalized;
                Vector3 movement = direction * 1 * Time.deltaTime;

                transform.position += movement;

                state = NodeState.RUNNING;
                return state;
            }
            else
            {
                state = NodeState.SUCCESS;
                calledInitMethod = false;
                return state;
            }


            
        }
    }

    class TaskIdle : Node
    {
        private float waitTime = 1.0f;
        private float waitCounter = 0.0f;
        public TaskIdle()
        {

        }

        public override NodeState Evaluate()
        {
            //idle animation
            waitCounter += Time.deltaTime;
            if (waitCounter < waitTime)
            {
                state = NodeState.RUNNING;
                return state;
            }
            else
            {
                waitCounter = 0;
                state = NodeState.SUCCESS;
                return state;
            }
        }
    }

    class CheckTargetInAttackRange : Node
    {
        private readonly LayerMask playerLayerMask = 1 << LayerMask.NameToLayer("Player");

        Transform transform;
        Range range;
        SpriteAnimator anim;

        public CheckTargetInAttackRange(Transform transform, Range range)
        {
            this.transform = transform;
            this.range = range;
            this.anim = transform.GetChild(0).GetComponent<SpriteAnimator>();
        }

        public override NodeState Evaluate()
        {
            object target = GetData("target");

            if (target == null)
            {
                state = NodeState.FAILURE;
                return state;
            }

            foreach (var cellWorldPosition in range.worldCoords)
            {
                Vector3 tileWorldPosition = transform.position + cellWorldPosition;
                var result = Physics2D.OverlapCircleAll(tileWorldPosition, 1.0f, playerLayerMask);
                if (result.Length > 0)
                {
                    parent.parent.SetData("target", result[0].transform);
                    state = NodeState.SUCCESS;

                    return state;
                }
            }

            state = NodeState.FAILURE;
            return state;

        }
    }

    class TaskAttackToTarget : Node
    {
        private SpriteAnimator anim;

        private float waitTime;
        private float waitCounter = 0.0f;
        private bool calledInitMethod = false;

        public TaskAttackToTarget(Transform transform)
        {
            anim = transform.GetChild(0).GetComponent<SpriteAnimator>();
            waitTime = anim.GetAnimationTime("Golem-Attack");
        }

        void Init()
        {
            anim.Play("Golem-Attack");
        }

        public override NodeState Evaluate()
        {
            if (calledInitMethod == false)
            {
                Init();
                calledInitMethod = true;
            }

            waitCounter += Time.deltaTime;
            if (waitCounter < waitTime)
            {
                state = NodeState.RUNNING;
                return state;
            }
            else
            {
                Transform target = (Transform)GetData("target");
                if (target == null)
                {
                    state = NodeState.FAILURE;
                    return state;
                }

                Debug.Log(target.name);
                target.GetComponent<Entity>().TakeDamage(1);

                waitCounter = 0;
                calledInitMethod = false;
                state = NodeState.SUCCESS;
                return state;
            }
        }
    }
}

