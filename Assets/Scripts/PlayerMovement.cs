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

    public void FlipBasedOnTheCursor()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 1;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        bool isRight = (mouseWorldPosition.x > transform.position.x);

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