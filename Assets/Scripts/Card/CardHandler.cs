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
        [SerializeField] CardInfo cardInfo;
        public Card Card
        {   get => card;
            set
            {
                if (value == null)
                {
                    privewImage.Clear();
                }
                else
                {
                    privewImage.NameText = value.CardInfo.name;
                    privewImage.DescText = value.CardInfo.desc;
                    
                }
                card = value;
                this.cardInfo = card?.CardInfo;
            }
        }

        public CardPrivew privewImage;

        public delegate void ClickEvent();
        public ClickEvent MouseClickEvent;

        public bool HasCard() => card != null;

        private void Start()
        {
            privewImage.HideImage();
        }

        // 이미지를 클릭했을 때 
        public void OnPointerClick(PointerEventData eventData)
        {

            Card = null;
            privewImage.HideImage();

            //use card
            MouseClickEvent();
        }

        // 마우스 커서가 이미지 안으로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            privewImage.ShowImage();

            if (card == null)
            {
                Debug.LogError("Handler doesn't have a card.");
                return;
            }
            privewImage.NameText = card.CardInfo.name;
            privewImage.DescText = card.CardInfo.desc;
        }

        // 마우스 커서가 이미지 밖으로 나갈 때
        public void OnPointerExit(PointerEventData eventData)
        {
            privewImage.HideImage();
            privewImage.Clear();
        }

        private void Awake()
        {

        }
    }
}