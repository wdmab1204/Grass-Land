using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImageMouseEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject cardImage;

    private void Start()
    {
        cardImage.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardImage.SetActive(false);
    }
}
