using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HitStop : MonoBehaviour
{
    float originalFixedDeltaTime;
    Tween shakeTween;

    public void ApplyHitStop(float duration, float timeScale, int count, Enemy enemy, Vector2 force)
    {
        StartCoroutine(HitStopCoroutine(duration, timeScale, count, enemy, force));
    }

    private IEnumerator HitStopCoroutine(float duration, float timeScale, int count, Enemy enemy, Vector2 force)
    {
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
        enemy.PlaySoundHurt();
        enemy.Push(force);
        shakeTween.Kill();
    }

    private void OnEnable()
    {
        var enemy = Physics2D.OverlapBox(transform.position, new Vector2(2.25f, 2.25f), 0, LayerMask.GetMask("Enemy")).GetComponent<Enemy>();
        Debug.Log(enemy?.name);

        //if fource is 300, dust duration is 0.3, else if 100, duration is 0.1
        Vector2 force = (enemy.transform.position - transform.parent.position).normalized * 5f;

        ApplyHitStop(duration: 0.5f, timeScale: 0.2f, count: 1, enemy, force);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Enemy"))
        //{
        //    //if fource is 300, dust duration is 0.3, else if 100, duration is 0.1
        //    Vector2 force = (collision.transform.position-transform.parent.position).normalized * 300f;
            
        //    Enemy enemy = collision.GetComponent<Enemy>();
        //    enemy.PlaySoundHurt();
        //    enemy.Push(force);

        //    ApplyHitStop(duration: 0.3f, timeScale: 0.3f, count: 1, force);

        //}
    }
}
