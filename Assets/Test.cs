using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Test : MonoBehaviour
{
    public ParticleSystem particle;

    void Start()
    {
        StartCoroutine(a());
    }

    IEnumerator a()
    {
        particle.Play();
        yield return new WaitForSeconds(2.0f);
        particle.Stop();
    }
}