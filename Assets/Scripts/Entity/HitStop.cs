using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitStop : MonoBehaviour
{
    float originalFixedDeltaTime;
    Tween shakeTween;
    Rigidbody2D rb;
    float slowdownFactor = 0.95f; // 감속 계수

    public SoundPlayer player;

    private void Awake()
    {
        rb = transform.parent.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity *= slowdownFactor;
    }

    public void ApplyHitStop(float duration, float timeScale, int count, Vector2 force)
    {
        StartCoroutine(HitStopCoroutine(duration, timeScale, count, force));
    }

    private IEnumerator HitStopCoroutine(float duration, float timeScale, int count, Vector2 force)
    {
        player.Play("Hit");
        while (count-- > 0)
        {
            originalFixedDeltaTime = Time.fixedDeltaTime;
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;

            shakeTween = Camera.main.transform.DOShakePosition(duration: 0.3f, strength: 0.1f, vibrato: 30, randomness: 90).SetUpdate(true); //setupdate : can play during timescale is 0
            yield return new WaitForSecondsRealtime(duration);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = originalFixedDeltaTime;
        }

        rb.AddForce(force);
        shakeTween.Kill();
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
        if (collision.CompareTag("Skill"))
        {
            Debug.Log("Hit stop");
            Vector2 force = (this.transform.position - collision.transform.parent.position).normalized * 100f;

            ApplyHitStop(duration: 0.3f, timeScale: 0.3f, count: 1, force);
        }
    }


}
