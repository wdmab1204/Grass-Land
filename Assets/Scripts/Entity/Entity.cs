using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    Rigidbody rb;
    Animator animator;
    SpriteRenderer sprite;
    const float slowdownFactor = 0.95f; // 감속 계수
    public ParticleSystem dust;

    [Space()]
    public AudioSource hitSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    float speedThreshold = .1f;
    private void Update()
    {
        AdjustSortingLayer();
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

    void AdjustSortingLayer()
    {
        sprite.sortingOrder = (int)(transform.position.y * -100);
    }

    public void PlayHurtSFX()
    {
        hitSound.Play();
        animator.Play("Hurt");
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
        dust.Play();
    }

    public void FlipBasedOnTheCursor()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 1;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        bool isRight = (mouseWorldPosition.x > transform.position.x);

        FlipX(isRight);
    }

    public void FlipX(bool isRight)
    {
        if (isRight)
        {
            //transform.localScale = Vector3.one;
            sprite.flipX = false;
        }
        else
        {
            //transform.localScale = new Vector3(-1, 1);
            sprite.flipX = true;
        }
    }
}
