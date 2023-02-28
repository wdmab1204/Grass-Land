using UnityEngine;
using System.Collections;
using System;
using Unity.VisualScripting;

public class ArrowTileGroup : MonoBehaviour, IGraphicsDisplay
{
	public Action<Vector3> ClickEvent;

	public ArrowTile[] childs;

    private void Awake()
    {
		childs = new ArrowTile[transform.childCount];
        for(int i=0; i<transform.childCount; i++)
		{
			var child = transform.GetChild(i);
			childs[i] = child.gameObject.GetComponent<ArrowTile>();
		}
    }

	public ArrowTile GetChildDisplay(int index) => childs[index];

    public void OnClickEvent(Vector3 position)
	{
		ClickEvent?.Invoke(position);
	}

	public void Show()
	{
		for (int i = 0; i < childs.Length; i++)
			childs[i].Show();
	}

	public void Hide()
	{
        for (int i = 0; i < childs.Length; i++)
            childs[i].Hide();
    }
}

