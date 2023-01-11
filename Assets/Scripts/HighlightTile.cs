using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class HighlightTile : MonoBehaviour
{
	private SpriteRenderer sprite;
	public delegate void HighlightTileClickEvent(HighlightTile hTile);
	public HighlightTileClickEvent clickEvent;

	public void Show()
	{
		sprite.enabled = true;
	}

	public void Hide()
	{
		sprite.enabled = false;
	}

    private void Start()
    {
		sprite = GetComponent<SpriteRenderer>();
		Hide();
    }

    private void OnMouseDown()
    {
        Debug.Log("I cliked!!");
        clickEvent?.Invoke(this);
    }

}

