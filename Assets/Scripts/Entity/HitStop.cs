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
            SetTimeScale(timeScale);

            shakeTween = Camera.main.transform.DOShakePosition(shakeDuration, strength, vibrato, randomness).SetUpdate(true);

            entity.PlayHurtSFX();
            hitSound.Play();
            
            yield return new WaitForSecondsRealtime(duration);

            ResetTimeScale();
        }

        entity.Push(force);
        entity.FlipX(entity.transform.position.x <= transform.parent.position.x);

        shakeTween.Kill();
    }

    void SetTimeScale(float timeScale)
    {
        originalFixedDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = originalFixedDeltaTime * Time.timeScale;
    }

    void ResetTimeScale()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = originalFixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TargetTag))
        {
            Vector3 force = (other.transform.position - transform.parent.position).normalized * forceScala;
            force.z = 0;

            Entity enemy = other.GetComponent<Entity>();

            ApplyHitStop(duration, timeScale, count, enemy, force);

        }
    }
}
