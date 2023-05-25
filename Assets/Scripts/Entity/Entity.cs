using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    const float slowdownFactor = 0.95f; // 감속 계수
    public ParticleSystem dust;

    [Space()]
    public AudioSource hitSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < 0.1f) dust.Stop();
        rb.velocity *= slowdownFactor;
    }

    public void PlayHitSFX()
    {
        hitSound.Play();
        animator.Play("Hurt");
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
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
