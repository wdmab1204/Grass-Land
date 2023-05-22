using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    AudioSource audioSource;
    Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();
    public string[] audioNames;
    public AudioClip[] clips;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.pitch = 1;
        for (int i=0; i<audioNames.Length; i++)
        {
            audioDictionary.Add(audioNames[i], clips[i]);
        }
    }

    public void Play(string key)
    {
        audioSource.PlayOneShot(audioDictionary[key]);
    }
}
