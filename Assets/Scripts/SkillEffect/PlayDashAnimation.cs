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
        //transform.DOMove(to, 0.5f).SetEase(Ease.OutQuart).OnComplete(() =>
        //{
        //    col.enabled = true;
        //    effectObject.SetActive(false);
        //    GameRuleSystem.Instance.Next();
        //});

        //StartCoroutine(MoveObjectUsingTranslate(to, 0.5f));

        StartCoroutine(SmoothCoroutine(to, 0.5f));
    }

    IEnumerator SmoothCoroutine(Vector3 target, float time)
    {
        Vector3 velocity = Vector3.zero;

        while (Vector2.Distance(this.transform.position,target)>0.01f)
        {
            this.transform.position
                = Vector3.SmoothDamp(this.transform.position, target, ref velocity, time);
            yield return null;
        }

        transform.position = target;

        col.enabled = true;
        effectObject.SetActive(false);
        GameRuleSystem.Instance.Next();

        yield return null;
    }

    IEnumerator MoveObjectUsingTranslate(Vector3 targetPosition, float moveDuration)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        col.enabled = true;
        effectObject.SetActive(false);
        GameRuleSystem.Instance.Next();
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
