using UnityEngine;
using System.Collections;

namespace BehaviourTree.Tree
{
	public abstract class Tree : MonoBehaviour
	{
        private Node rootNode;

        protected virtual void Start()
        {
            rootNode = SetupBehaviourtree();
        }

        protected virtual void Update()
        {
            rootNode?.Evaluate();
        }

        protected abstract Node SetupBehaviourtree();
    }

}