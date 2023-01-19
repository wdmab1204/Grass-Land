using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class RangeTile : MonoBehaviour, IGraphicsDisplay
{
    private SpriteRenderer spriteRenderer;

    public void Hide()
    {
        spriteRenderer.enabled = false;
    }

    public void Show()
    {
        spriteRenderer.enabled = true;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}

