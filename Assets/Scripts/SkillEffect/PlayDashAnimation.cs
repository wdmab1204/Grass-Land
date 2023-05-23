using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayDashAnimation : MonoBehaviour
{
    Animator animator;
    Vector3 mousePosition;
    const float slowdownFactor = 0.95f; // 감속 계수
    Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    //card unity event 
    public void SetMousePosition()
    {
        mousePosition = Input.mousePosition;
    }

    public void Dash()
    {
        Vector3 to = Camera.main.ScreenToWorldPoint(mousePosition);
        to.z = transform.position.z;
        transform.DOMove(to, 0.7f).SetEase(Ease.OutQuart);
    }

    private void FixedUpdate()
    {
        rb.velocity *= slowdownFactor;
    }
}
