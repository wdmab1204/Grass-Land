using UnityEngine;
using System.Collections;
using CardNameSpace.Base;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace CardNameSpace
{
    public class CardHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        private Card card;
        public Card Card
        {   get => card;
            set
            {
                if (value == null)
                {
                    nameText.text = "";
                    descText.text = "";
                }
                else
                {
                    nameText.text = value.CardInfo.name;
                    descText.text = value.CardInfo.desc;
                }
                card = value;
            }
        }
        private Image cardImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descText;
        [SerializeField] private GameObject privewImage;

        public delegate void ClickEvent();
        public ClickEvent MouseClickEvent;

        public bool HasCard() => card != null;

        public void HideImage() => cardImage.enabled = false;

        public void ShowImage() => cardImage.enabled = true;

        private void Start()
        {
            privewImage.SetActive(false);
        }

        // 이미지를 클릭했을 때 
        public void OnPointerClick(PointerEventData eventData)
        {

            Card = null;
            HideImage();

            //use card
            MouseClickEvent();
        }

        // 마우스 커서가 이미지 안으로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            privewImage.SetActive(true);

            if (card == null)
            {
                Debug.LogError("Handler doesn't have a card.");
                return;
            }
            nameText.text = card.CardInfo.name;
            descText.text = card.CardInfo.desc;
        }

        // 마우스 커서가 이미지 밖으로 나갈 때
        public void OnPointerExit(PointerEventData eventData)
        {
            privewImage.SetActive(false);
            nameText.text = "";
            descText.text = "";
        }

        private void Awake()
        {
            cardImage = GetComponent<Image>();
        }
    }
}