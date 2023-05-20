using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        AdjustSortingLayer();
    }

    void AdjustSortingLayer()
    {
        sprite.sortingOrder = (int)(transform.position.y * -100);
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
        if (moveDir.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveDir.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    [ContextMenu("Run")]
    void asdf()
    {
        anim.Play("Warrior-Single Swing 3", 0);
    }
}