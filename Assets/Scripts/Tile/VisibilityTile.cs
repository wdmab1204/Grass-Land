using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class VisibilityTile : MonoBehaviour, IGraphicsDisplay
{
    private SpriteRenderer SpriteRenderer;
    public string controllerName;
    
    public void Hide()
    {
        SpriteRenderer.enabled = false;
    }

    public void Show()
    {
        SpriteRenderer.enabled = true;
    }

    protected virtual void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}

