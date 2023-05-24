using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitStop : MonoBehaviour
{
    float originalFixedDeltaTime;
    Tween shakeTween;

    [Header("HitStop Info")]
    public float forceScala = 3;
    public float duration = .5f;
    public float timeScale = .2f;
    public int count = 1;

    [Header("Shake Cmaera Info")]
    public float shakeDuration = .3f;
    public float strength = .5f;
    [Range(1, 50)] public int vibrato = 30;
    [Range(0, 90)] public float randomness = 90;

    public void ApplyHitStop(float duration, float timeScale, int count, Enemy enemy, Vector2 force)
    {
        StartCoroutine(HitStopCoroutine(duration, timeScale, count, enemy, force));
    }

    public void ApplyHitStop(Enemy target, Vector2 direction)
    {
        StartCoroutine(HitStopCoroutine(duration, timeScale, count, target, direction * 3));
    }

    private IEnumerator HitStopCoroutine(float duration, float timeScale, int count, Enemy enemy, Vector2 force)
    {
        while (count-- > 0)
        {
            originalFixedDeltaTime = Time.fixedDeltaTime;
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;

            shakeTween = Camera.main.transform.DOShakePosition(shakeDuration, strength, vibrato, randomness).SetUpdate(true); //setupdate : can play during timescale is 0
            enemy.PlayHitSFX();
            yield return new WaitForSecondsRealtime(duration);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = originalFixedDeltaTime;
        }
        enemy.Push(force);

        //shield form
        if (enemy.transform.position.x > transform.parent.position.x)
            enemy.FlipX(isRight: false);
        else
            enemy.FlipX(isRight: true);

        shakeTween.Kill();
    }

    //private void OnEnable()
    //{
    //    var enemy = Physics2D.OverlapBox(transform.position, new Vector2(2.25f, 2.25f), 0, LayerMask.GetMask("Enemy")).GetComponent<Enemy>();
    //    Debug.Log(enemy?.name);

    //    //if fource is 300, dust duration is 0.3, else if 100, duration is 0.1
    //    Vector2 direction = (enemy.transform.position - transform.parent.position).normalized * 1f;

    //    ApplyHitStop(duration: 0.5f, timeScale: 0.2f, count: 1, enemy, direction);

    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print(collision.name);
        if (collision.CompareTag("Enemy"))
        {
            //if fource is 300, dust duration is 0.3, else if 100, duration is 0.1
            Vector2 force = (collision.transform.position - transform.parent.position).normalized * forceScala;

            Enemy enemy = collision.GetComponent<Enemy>();

            ApplyHitStop(duration, timeScale, count, enemy, force);

        }
    }
}
