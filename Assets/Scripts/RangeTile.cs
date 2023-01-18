using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class RangeTile : MonoBehaviour, IGraphicsDisplay
{
    private SpriteRenderer spriteRenderer;

    void IGraphicsDisplay.Hide()
    {
        spriteRenderer.enabled = false;
    }

    void IGraphicsDisplay.Show()
    {
        spriteRenderer.enabled = true;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}

