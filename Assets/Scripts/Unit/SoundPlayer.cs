using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            this.gameObject.SetActive(false);
        }
    }
}
