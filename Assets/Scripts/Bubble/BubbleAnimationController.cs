using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BubbleAnimationController : MonoBehaviour
{
    public SpriteRenderer bubbleSprite;
    public Animator bubbleAnim;
    Transform target;

    void Awake()
    {
        //bubbleSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        //bubbleAnim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        time = 0;
    }

    float time = 0f;
    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        time += Time.deltaTime;
        bubbleAnim.SetFloat("Time", time);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        time = -1;
        bubbleAnim.SetFloat("Time", time);
    }
}
