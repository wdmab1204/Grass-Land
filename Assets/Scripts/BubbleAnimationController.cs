using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BubbleAnimationController : MonoBehaviour
{
    SpriteRenderer bubbleSprite;
    Animator bubbleAnim;
    Transform target;

    void Awake()
    {
        bubbleSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        bubbleAnim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        time = 0;
    }

    float time = 0f;
    private void OnTriggerStay2D(Collider2D collision)
    {
        time += Time.deltaTime;
        bubbleAnim.SetFloat("Time", time);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        time = -1;
        bubbleAnim.SetFloat("Time", time);
    }

    public void Flip()
    {

    }
}