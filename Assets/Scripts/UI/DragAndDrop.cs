using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.Events;

public class DragAndDrop : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Image image;
    [HideInInspector]public Transform parentAfterDrag;
    RectTransform panelRect;

    [Space]
    public Sprite targetSprite;
    Sprite originalSprite;
    Vector2 originalSizeDelta;
    Tween scaleTween;
    bool scaleTweenPlaying = false;

    public UnityEvent skillActionIfDrop;

    private void Awake()
    {
        originalSprite = image.sprite;
        originalSizeDelta = image.GetComponent<RectTransform>().sizeDelta;
    }

    private void Start()
    {
        panelRect = image.transform.parent.parent.GetComponent<RectTransform>();
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

        //마우스커서가 패널 바깥에 있다면
        if (!IsCursorInImage(panelRect))
        {
            //카드이미지를 커서이미지로 바꿈
            ChangeCursorImage();

            if (scaleTweenPlaying == false)
            {
                //커서이미지 애니메이션
                TweenTargetCursor();
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;

        if (IsCursorInImage(panelRect))
        {
            //마우스커서가 패널안에 있어서 카드의 사용이 되지 않았을 때
            ComebackOriginalPosition();
        }
        else
        {
            //마우스커서가 패널밖에 있어서 카드가 사용되었을 때
            DoActionAndDestroyCard();
        }
    }

    void TweenTargetCursor()
    {
        scaleTween = image.rectTransform.DOScale(Vector3.one * 1.5f, 0.2f) // 스케일이 2배로 커짐
                .SetLoops(-1, LoopType.Yoyo); // 애니메이션을 반복하고, 역방향으로 되돌아옴
        scaleTweenPlaying = true;
    }

    bool IsCursorInImage(RectTransform rect)
    {
        // 마우스 위치를 캔버스 좌표계로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rect,
            Input.mousePosition,
            null,
            out localPoint
        );

        //return true if cursor in image
        return localPoint.x > -rect.sizeDelta.x &&
            localPoint.x < rect.sizeDelta.x &&
            localPoint.y > -rect.sizeDelta.y &&
            localPoint.y < rect.sizeDelta.y;
        
    }

    void ChangeCursorImage()
    {
        image.sprite = targetSprite;
        image.rectTransform.sizeDelta = new Vector2(100, 100);
        image.rectTransform.localScale = Vector3.one;
    }

    void ComebackOriginalPosition()
    {
        image.sprite = originalSprite;
        image.rectTransform.sizeDelta = originalSizeDelta;
        image.rectTransform.localScale = new Vector3(0.5f, 0.5f);
        scaleTween?.Kill();
        scaleTweenPlaying = false;
    }

    void DoActionAndDestroyCard()
    {
        skillActionIfDrop.Invoke();
        Destroy(transform.parent.gameObject);
    }

    private void OnDisable()
    {
        DOTween.Kill(scaleTween);
    }
}
