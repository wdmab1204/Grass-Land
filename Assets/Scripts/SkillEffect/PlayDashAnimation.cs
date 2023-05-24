using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayDashAnimation : MonoBehaviour
{
    Animator animator;
    Vector3 mousePosition;
    const float slowdownFactor = 0.95f; // 감속 계수

    public GameObject effectPrefab;
    GameObject effectObject;
    SoundPlayer player;
    Collider2D col;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<SoundPlayer>();
        col = GetComponent<BoxCollider2D>();
    }

    //card unity event 
    public void SetMousePosition()
    {
        mousePosition = Input.mousePosition;
    }

    public void Dash()
    {
        col.enabled = false;

        if (effectObject == null)
        {
            effectObject = Instantiate(effectPrefab, transform);
            effectObject.transform.localPosition = Vector3.zero;
        }

        effectObject.SetActive(true);
        Vector3 to = Camera.main.ScreenToWorldPoint(mousePosition);
        to.z = transform.position.z;
        transform.DOMove(to, 0.3f).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            col.enabled = true;
            effectObject.SetActive(false);
        });
    }

    public float pushForce = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D otherRigidbody))
        {
            print(collision.name);
            //// 충돌한 물체를 밀어내기 위해 힘을 가하기
            Vector3 pushDirection = collision.transform.position - transform.position;
            otherRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
        }
    }
}
