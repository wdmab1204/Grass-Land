using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CardSlot : MonoBehaviour, IDropHandler
{

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            GameObject dropped = eventData.pointerDrag;
            DragAndDrop dragAndDrop = dropped.GetComponent<DragAndDrop>();
            dragAndDrop.parentAfterDrag = transform;
        }
    }

    private void OnEnable()
    {
        Debug.Log("create!");
    }
}

