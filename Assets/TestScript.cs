using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleSpriteAnimator;

public class TestScript : MonoBehaviour
{
    SpriteAnimator SpriteAnimation;

    private void Awake()
    {
        SpriteAnimation = GetComponent<SpriteAnimator>();
    }

    private void Start()
    {
        SpriteAnimation.Play("Test Animation");
    }
}
