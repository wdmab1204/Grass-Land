using UnityEngine;
using System.Collections;
using CardNameSpace.Base;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using SimpleSpriteAnimator;

namespace CardNameSpace
{
    public class CardHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IGraphicsDisplay
    {
        private Card card;
        [SerializeField] CardInfo cardInfo;
        public Card Card
        {   get => card;
            set
            {
                card = value;
                this.cardInfo = card?.CardInfo;
            }
        }

        private Image cardImage;

        public delegate void ClickEvent(CardInfo card);
        public ClickEvent PointerEnterEvent;
        public ClickEvent PointerExitEvent;
        public ClickEvent MouseClickEnterEvent;
        public ClickEvent MouseCLickUpdateEvent;
        public ClickEvent MouseClickExitEvent;

        public bool HasCard() => card != null;

        public Sprite attackCard;
        public Sprite moveCard;
        public Sprite healCard;


        private void Start()
        {
            UpdateCardSprite();
        }

        private void UpdateCardSprite()
        {
            if (this.card.CardInfo.cardType == CardType.ATTACK) cardImage.sprite = attackCard;
            else if (this.card.CardInfo.cardType == CardType.MOVE) cardImage.sprite = moveCard;
            else if (this.card.CardInfo.cardType == CardType.HEAL) cardImage.sprite = healCard;
        }

        // 이미지를 클릭했을 때 
        public void OnPointerClick(PointerEventData eventData)
        {
            MouseClickEnterEvent?.Invoke(this.cardInfo);
            //use Card

            Card = null;

            MouseClickExitEvent?.Invoke(this.cardInfo);
        }
        

        private IEnumerator StartCardClickEventCoroutine()
        {
            MouseClickEnterEvent?.Invoke(this.cardInfo);
            //use Card
            Card = null;
            yield return new WaitForSeconds(2.0f);

            MouseClickExitEvent?.Invoke(this.cardInfo);
        }

        // 마우스 커서가 이미지 안으로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            PointerEnterEvent?.Invoke(this.cardInfo);

        }

        // 마우스 커서가 이미지 밖으로 나갈 때
        public void OnPointerExit(PointerEventData eventData)
        {
            PointerExitEvent?.Invoke(this.cardInfo);
        }

        private void Awake()
        {
            cardImage = GetComponent<Image>();
        }

        public void Show()
        {
            if (card == null)
            {
                return;
            }

            cardImage.enabled = true;
        }

        public void Hide()
        {
            cardImage.enabled = false;
        }
    }
}