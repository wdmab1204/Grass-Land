using System.Collections;
using TurnSystem;
using UnityEngine;

[DisallowMultipleComponent]
public class MonsterActor : TurnActor
{
    Animator animator;
    public new Collider2D collider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        collider.enabled = false;
    }

    public override void UpdateTurn()
    {
        animator.Play("Enemy1-Attack1");
    }

    public void OnCollider() => collider.enabled = true;
    public void OffCollider() => collider.enabled = false;
    public void Next() => GameRuleSystem.Instance.Next();
}

