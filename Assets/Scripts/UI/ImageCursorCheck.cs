using UnityEngine;
using UnityEngine.UI;

public class ImageCursorCheck : MonoBehaviour
{
    public Image image;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = image.GetComponent<RectTransform>();
    }

    private void Update()
    {
        // 마우스 위치를 캔버스 좌표계로 변환
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            Input.mousePosition,
            null,
            out localPoint
        );

        // Image의 크기와 위치를 가져와서 마우스 위치와 비교
        if (localPoint.x > -rectTransform.sizeDelta.x &&
            localPoint.x < rectTransform.sizeDelta.x&&
            localPoint.y > -rectTransform.sizeDelta.y&&
            localPoint.y < rectTransform.sizeDelta.y)
        {
            Debug.Log("Mouse is inside the image");
        }
        else
        {
            Debug.Log("Mouse is outside the image");
        }
    }
}
