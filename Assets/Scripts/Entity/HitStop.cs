using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum Target { PLAYER,ENEMY };

public class HitStop : MonoBehaviour
{
    float originalFixedDeltaTime;
    Tween shakeTween;
    public AudioSource hitSound;

    [Header("HitStop Info")]
    public float forceScala = 3;
    public float duration = .5f;
    public float timeScale = .2f;
    public int count = 1;
    public Target target = Target.ENEMY;
    public string TargetTag
    {
        get
        {
            if (this.target == Target.PLAYER) return "Player";
            else return "Enemy";
        }
    }

    [Header("Shake Cmaera Info")]
    public float shakeDuration = .3f;
    public float strength = .5f;
    [Range(1, 50)] public int vibrato = 30;
    [Range(0, 90)] public float randomness = 90;

    public void ApplyHitStop(float duration, float timeScale, int count, Entity enemy, Vector2 force)
    {
        StartCoroutine(HitStopCoroutine(duration, timeScale, count, enemy, force));
    }

    public void ApplyHitStop(Entity target, Vector2 direction)
    {
        StartCoroutine(HitStopCoroutine(duration, timeScale, count, target, direction * 3));
    }

    private IEnumerator HitStopCoroutine(float duration, float timeScale, int count, Entity entity, Vector3 force)
    {
        while (count-- > 0)
        {
            originalFixedDeltaTime = Time.fixedDeltaTime;
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;

            shakeTween = Camera.main.transform.DOShakePosition(shakeDuration, strength, vibrato, randomness).SetUpdate(true); //setupdate : can play during timescale is 0
            entity.PlayHitSFX();
            hitSound.Play();
            yield return new WaitForSecondsRealtime(duration);

            Time.timeScale = 1f;
            Time.fixedDeltaTime = originalFixedDeltaTime;
        }
        entity.Push(force);

        //shield form
        if (entity.transform.position.x > transform.parent.position.x)
            entity.FlipX(isRight: false);
        else
            entity.FlipX(isRight: true);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            //if fource is 300, dust duration is 0.3, else if 100, duration is 0.1
            Vector3 force = (other.transform.position - transform.parent.position).normalized * forceScala;
            force.z = 0;

            Entity enemy = other.GetComponent<Entity>();

            ApplyHitStop(duration, timeScale, count, enemy, force);

        }
    }
}
