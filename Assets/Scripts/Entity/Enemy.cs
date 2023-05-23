using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;
    SoundPlayer soundPlayer;
    const float slowdownFactor = 0.95f; // 감속 계수
    public ParticleSystem dust;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        soundPlayer = GetComponent<SoundPlayer>();
    }

    private void FixedUpdate()
    {
        if (rb != null)
            rb.velocity *= slowdownFactor;
    }

    public void PlaySoundHurt()
    {
        soundPlayer.Play("Hit");
    }

    public void Push(Vector2 force)
    {
        rb.AddForce(force, ForceMode2D.Impulse);
        var mainModule = dust.main;
        mainModule.duration = force.magnitude / 3.14f;
        dust.Play();
    }
}
