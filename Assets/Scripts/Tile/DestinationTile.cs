using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class DestinationTile : MonoBehaviour, IGraphicsDisplay, IPointerClickHandler
{
	private SpriteRenderer spriteRenderer;
	public delegate void HighlightTileClickEvent(Vector3 tileWorldPosition);
	public HighlightTileClickEvent clickEvent;

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
		Debug.Log("click!");
		clickEvent?.Invoke(this.transform.position);
    }
}

