using System;
using BehaviourTree;
using BehaviourTree.Tree;
using UnityEngine;
using SimpleSpriteAnimator;
using System.Collections.Generic;
using UnityEngine.SocialPlatforms;

namespace BehaviourTree.Tree
{
    abstract class AnimationNode : Node
    {
        protected SpriteAnimator anim;
        string animationName;

        bool canPlay = true;

        float waitCounter = 0;
        float animationTime;

        protected bool Playing => waitCounter < animationTime;

        protected bool PlayOnce = true;

        protected System.Action PlayAction;

        public AnimationNode(Transform transform, string animationName) : base()
        {
            this.anim = transform.GetChild(0).GetComponent<SpriteAnimator>();
            this.animationName = animationName;
            animationTime = anim.GetAnimationTime(animationName);
        }

        protected void Reset()
        {
            canPlay = true;
            PlayOnce = true;
        }

        protected void PlayAnimation()
        {
            if (canPlay && PlayOnce)
            {
                anim.Play(animationName);
                PlayAction?.Invoke();
                canPlay = false;
                PlayOnce = false;
                waitCounter = 0;
            }
        }

        public override NodeState Evaluate()
        {
            if (waitCounter >= animationTime)
            {
                canPlay = true;
                waitCounter = 0;
            }
            else
            {
                waitCounter += Time.deltaTime;
            }
            return base.Evaluate();
        }
    }

    public class BossBT : MonsterBT
    {
        Range bressgasRange;
        Range jumpAttackRange;
        ParticleSystem particle;
        string jumpRangeString =
            "[-2,2][-1,2][0,2][1,2][2,2]" +
            "[-2,1][-1,1][0,1][1,1][2,1]" +
            "[-2,0][-1,0]     [1,0][2,0]" +
            "[-2,-1][-1,-1][0,-1][1,-1][2,-1]" +
            "[-2,-2][-1,-2][0,-2][1,-2][2,-2]";

        public BossBT(Transform transform) : base(transform)
        {
            particle = transform.GetChild(1).GetComponent<ParticleSystem>();
            jumpAttackRange = new Range(jumpRangeString, tilemap);
        }

        protected override Node SetupBehaviourtree()
        {
            return new Selector(new List<Node>()
            {
                new Sequence(new List<Node>()
                {
                    new CheckTarget(),
                    new RandomChoiceNode(new List<Node>()
                    {
                        new TaskBressGas(transform,particle),
                        new TaskJumpAttack(transform,jumpAttackRange),
                    })
                })
            });
        }

        protected override void SetupTileGroup()
        {
            tileGroup.CreateClones("range-tile_1", jumpAttackRange, transform.position);
        }
    }

    class CheckTarget : Node
    {
        public override NodeState Evaluate()
        {
            Transform target = (Transform)GetData("target");
            if (target == null)
            {
                var result = Physics2D.OverlapCircleAll(Vector3.zero, 10000.0f, 1 << LayerMask.NameToLayer("Player"));
                if (result.Length > 0)
                {
                    parent.parent.SetData("target", result[0].transform);
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

    class RandomChoiceNode : Node
    {
        System.Random random;
        Node nextNode = null;
        Node currentNode = null;

        public RandomChoiceNode(List<Node> nodes) : base(nodes)
        {
            random = new System.Random();
        }

        public override NodeState Evaluate()
        {
            if (nextNode == null)
            {
                currentNode = children[random.Next(0, children.Count)];
            }
            else
            {
                nextNode = currentNode;
            }

            state = currentNode.Evaluate();

            if (state == NodeState.RUNNING)
            {
                nextNode = currentNode;
            }
            else if (state == NodeState.SUCCESS)
            {
                nextNode = null;
            }

            return state;
        }
    }

    
    class CheckJumpAttackInRange : Node
    {
        Transform transform;
        Range range;

        public CheckJumpAttackInRange(Transform transform, Range range)
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

    class TaskJumpAttack : AnimationNode
    {
        private readonly LayerMask playerLayerMask = 1 << LayerMask.NameToLayer("Player");

        Range range;
        Transform transform;

        public TaskJumpAttack(Transform transform, Range range) : base(transform, "Jumping")
        {
            this.range = range;
            this.transform = transform;
        }

        public override NodeState Evaluate()
        {
            base.Evaluate();

            PlayAnimation();

            if (!Playing)
            {
                foreach (var coord in range.worldCoords)
                {
                    var result = Physics2D.OverlapCircle(transform.position + coord, 0.3f, playerLayerMask);
                    if (result != null)
                    {
                        result.GetComponent<GameEntity.Entity>().TakeDamage(1);
                    }
                }

                Reset();
                anim.Play("Idle");
                state = NodeState.SUCCESS;
                return state;
            }

            state = NodeState.RUNNING;
            return state;
        }
    }

    //class TaskJumpUp : AnimationNode
    //{

    //    float jumpDistance = 100.0f;
    //    bool calledInitMethod = false;
    //    public TaskJumpUp(Transform transform, Range range) : base(transform, "Jump up")
    //    {

    //    }

    //    public override NodeState Evaluate()
    //    {
    //        PlayAnimation();

    //        parent.parent.SetData("isJumping", true);



    //        base.Evaluate();
    //    }
    //}

    //class TaskJumpDown : Node
    //{
    //    SpriteAnimator anim;
    //    public TaskJumpDown(Transform transform)
    //    {
    //        anim = transform.GetComponent<SpriteAnimator>();
    //    }

    //    public override NodeState Evaluate()
    //    {
    //        anim.Play("Jump Down");
    //        return base.Evaluate();
    //    }
    //}

    class CheckTargetInBressGasRange : Node
    {
        Transform transform;
        Range range;

        private readonly LayerMask playerLayerMask = 1 << LayerMask.NameToLayer("Player");

        public CheckTargetInBressGasRange(Transform transform, Range range)
        {
            this.transform = transform;
            this.range = range;
        }

        public override NodeState Evaluate()
        {
            foreach (var cellWorldPosition in range.worldCoords)
            {
                Vector3 tileWorldPosition = transform.position + cellWorldPosition;
                var result = Physics2D.OverlapCircleAll(tileWorldPosition, 1.0f, playerLayerMask);
                if (result.Length > 0)
                {
                    state = NodeState.SUCCESS;
                    return state;
                }
            }

            state = NodeState.FAILURE;
            return state;

        }
    }

    class TaskBressGas : AnimationNode
    {
        ParticleSystem particle;

        public TaskBressGas(Transform transform, ParticleSystem particle) : base(transform, "BressGas")
        {
            this.particle = particle;
            PlayAction += () => { particle.Play(); };
        }

        public override NodeState Evaluate()
        {
            base.Evaluate();
            PlayAnimation();

            if (Playing)
            {
                state = NodeState.RUNNING;
                return state;
            }
            else
            {
                anim.Play("Idle");
                Reset();
                state = NodeState.SUCCESS;
                return state;
            }
        }
    }
}

