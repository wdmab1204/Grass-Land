using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Image image;
    [HideInInspector]public Transform parentAfterDrag;
    RectTransform panelRect;

    [Space]
    public Sprite targetSprite;
    Sprite originalSprite;
    Vector2 originalSizeDelta;

    private void Awake()
    {
        panelRect = image.transform.parent.parent.GetComponent<RectTransform>();
        originalSprite = image.sprite;
        originalSizeDelta = image.GetComponent<RectTransform>().sizeDelta;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        Image panel = transform.parent.GetComponent<Image>();

        // 마우스 위치를 캔버스 좌표계로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panelRect,
            Input.mousePosition,
            null,
            out localPoint
        );

        // Image의 크기와 위치를 가져와서 마우스 위치와 비교
        if (localPoint.x > -panelRect.sizeDelta.x &&
            localPoint.x < panelRect.sizeDelta.x &&
            localPoint.y > -panelRect.sizeDelta.y &&
            localPoint.y < panelRect.sizeDelta.y)
        {

        }
        else
        {
            image.sprite = targetSprite;
            image.rectTransform.sizeDelta = new Vector2(100,100);
            image.rectTransform.localScale = Vector3.one;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

        image.sprite = originalSprite;
        image.rectTransform.sizeDelta = originalSizeDelta;
        image.rectTransform.localScale = new Vector3(0.5f, 0.5f);
    }
}
