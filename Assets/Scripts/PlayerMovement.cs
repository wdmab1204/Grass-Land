using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody rb;
    private Animator anim;
    private SpriteRenderer sprite;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        Vector2 moveDir = new Vector2(horizontalMove, verticalMove).normalized;

        rb.velocity = moveDir * moveSpeed;

        if (moveDir != Vector2.zero)
        {
            anim.SetBool("IsMoving", true);
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        // 이동 방향에 따라 이미지 뒤집기
        if (moveDir.x < 0) sprite.flipX = true;
        else if (moveDir.x > 0) sprite.flipX = false;
    }
}