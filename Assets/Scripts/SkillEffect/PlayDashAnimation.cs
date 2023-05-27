using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayDashAnimation : MonoBehaviour
{
    Animator animator;
    Vector3 mousePosition;
    const float slowdownFactor = 0.95f; // 감속 계수

    //public GameObject effectPrefab;
    public GameObject effectObject;
    public AudioSource skillSound;
    SoundPlayer player;
    Collider col;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponent<SoundPlayer>();
        col = GetComponent<Collider>();
    }

    //card unity event 
    public void SetMousePosition()
    {
        mousePosition = Input.mousePosition;
    }

    public void Dash()
    {
        col.enabled = false;

        effectObject.SetActive(true);
        Vector3 to = Camera.main.ScreenToWorldPoint(mousePosition);
        to.z = transform.position.z;

        skillSound.Play();
        transform.DOMove(to, 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
        {
            col.enabled = true;
            effectObject.SetActive(false);
            GameRuleSystem.Instance.Next();
        });
    }

    //public float pushForce = 10f;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Enemy") && collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D otherRigidbody))
    //    {
    //        print(collision.name);
    //        //// 충돌한 물체를 밀어내기 위해 힘을 가하기
    //        Vector3 pushDirection = collision.transform.position - transform.position;
    //        otherRigidbody.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
    //    }
    //}
}
