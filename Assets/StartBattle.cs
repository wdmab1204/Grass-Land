using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StartBattle : StateMachineBehaviour
{
    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform warrior = GameObject.Find("Warrior").transform;
        Transform me = animator.transform.parent;
        SpriteRenderer spriteRenderer = animator.transform.parent.GetComponent<SpriteRenderer>();

        if (warrior.position.x < me.position.x) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject panel = GameObject.Find("Panel");

        panel.transform.DOMoveY(0, 1).SetEase(Ease.OutQuad);

        animator.gameObject.SetActive(false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
