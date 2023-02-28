using UnityEngine;
using UnityEngine.EventSystems;
using System;

[RequireComponent(typeof(SpriteRenderer))]
public class ArrowTile : MonoBehaviour, IGraphicsDisplay, IPointerClickHandler
{
    private SpriteRenderer spriteRenderer;
    public ArrowTileGroup arrowTileGroup;

    public void Show()
    {
        spriteRenderer.enabled = true;
    }

    public void Hide()
    {
        spriteRenderer.enabled = false;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        arrowTileGroup.OnClickEvent(this.transform.position);
    }
}
