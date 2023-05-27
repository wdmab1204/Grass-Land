using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    const float slowdownFactor = 0.95f; // 감속 계수
    public ParticleSystem dust;

    [Space()]
    public AudioSource hitSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        //if (string.Compare(gameObject.name, "Warrior") == 0) print(rb.velocity.magnitude);
        if (rb.velocity.magnitude < 0.1f) dust.Stop();
    }

    float speedThreshold = .1f;
    private void Update()
    {
        // 플레이어의 속도를 가져옴
        float speed = rb.velocity.magnitude;

        // 속도가 임계값보다 작으면 파티클 재생을 멈춤
        if (speed < speedThreshold)
        {
            dust.Stop();
        }
        else
        {
            // 속도가 임계값보다 크면 파티클 재생을 시작함
            if (!dust.isPlaying)
            {
                dust.Play();
            }
        }
    }

    public void PlayHitSFX()
    {
        hitSound.Play();
        animator.Play("Hurt");
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
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
