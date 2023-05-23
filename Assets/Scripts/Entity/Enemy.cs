using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    SoundPlayer soundPlayer;
    Animator animator;
    const float slowdownFactor = 0.95f; // 감속 계수
    public ParticleSystem dust;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        soundPlayer = GetComponent<SoundPlayer>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        rb.velocity *= slowdownFactor;
    }

    public void Hit()
    {
        soundPlayer.Play("Hit");
        animator.Play("Enemy1-Hurt");
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
        var mainModule = dust.main;
        mainModule.duration = force.magnitude / 3.14f;
        dust.Play();
    }

    public void FlipX(bool isRight)
    {
        if (isRight)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1);
        }
    }
}
