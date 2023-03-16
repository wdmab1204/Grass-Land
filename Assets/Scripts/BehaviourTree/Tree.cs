using UnityEngine;
using System.Collections;

namespace BehaviourTree.Tree
{
    public abstract class Tree
    {
        private Node rootNode;

        private NodeState currentRootNodeState = NodeState.FAILURE;
        public NodeState CurrentRootNodeState => currentRootNodeState;

        public virtual void Initialize()
        {
            rootNode = SetupBehaviourtree();
        }

        public virtual void Update()
        {
            currentRootNodeState = rootNode.Evaluate();
        }

        protected abstract Node SetupBehaviourtree();
    }

}