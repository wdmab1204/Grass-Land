using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree.Tree.GuartBT
{
    public class GuardBT : Tree
    {
        public static float speed = 2f;
        public static float fovRange = 6f;

        public Transform[] waypoints;

        protected override void Start()
        {
            base.Start();
        }

        protected override Node SetupBehaviourtree()
        {
            Node root = new Selector(new List<Node>()
            {
                new Sequence(new List<Node>()
                {
                    new CheckEnemyInFOVRange(transform),
                    new TaskGoToTarget(transform),
                })
                , new TaskPatrol(transform,waypoints)
            });
            //Node root = new TaskPatrol(transform, waypoints);


            return root;
        }

    }

    public class TaskPatrol : Node
    {
        private Transform transform;
        private Transform[] waypoints;

        private int _currentWaypointIndex = 0;

        private float _waitTime = 1f; // in seconds
        private float _waitCounter = 0f;
        private bool _waiting = false;

        public TaskPatrol(Transform transform, Transform[] waypoints)
        {
            this.transform = transform;
            this.waypoints = waypoints;
        }

        public override NodeState Evaluate()
        {
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter >= _waitTime)
                {
                    _waiting = false;
                }
            }
            else
            {
                Transform wp = waypoints[_currentWaypointIndex];
                if (Vector3.Distance(transform.position, wp.position) < 0.01f)
                {
                    transform.position = wp.position;
                    _waitCounter = 0f;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(
                        transform.position, wp.position, GuardBT.speed * Time.deltaTime);
                    transform.LookAt(wp.position);
                }
            }

            state = NodeState.RUNNING;
            return state;
        }
    }

    public class CheckEnemyInFOVRange : Node
    {
        private static int enemyLayerMask = 1 << 6;
        private Transform _transform;

        public CheckEnemyInFOVRange(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            object t = GetData("target");
            if(t?.ToString()==null)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(_transform.position, GuardBT.fovRange, enemyLayerMask);

                if (colliders.Length > 0)
                {
                    parent.parent.SetData("target", colliders[0].transform);
                    state = NodeState.SUCCESS;
                    return state;
                }

                state = NodeState.FAILURE;
                return state;
            }

            state = NodeState.SUCCESS;
            return state;
        }
    }

    public class TaskGoToTarget : Node
    {
        private Transform _transform;

        public TaskGoToTarget(Transform transform)
        {
            _transform = transform;
        }

        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");

            if (Vector2.Distance(_transform.position, target.position) > 0.01f)
            {
                _transform.position = Vector3.MoveTowards(
                    _transform.position, target.position, GuardBT.speed * Time.deltaTime);
            }

            state = NodeState.RUNNING;
            return state;
        }
    }
}