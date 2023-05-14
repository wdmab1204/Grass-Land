using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BubbleAnimationController : MonoBehaviour
{
    SpriteRenderer bubbleSprite;
    Animator bubbleAnim;
    Transform target;

    public UIDocument ui;

    void Awake()
    {
        bubbleSprite = transform.GetComponent<SpriteRenderer>();
        bubbleAnim = transform.GetComponent<Animator>();

        //ui.enabled = false;
    }

    public void EnterCombatMode()
    {
        ui.enabled = true;
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
