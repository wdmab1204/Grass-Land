using UnityEngine;
using System.Collections;
using CardNameSpace.Base;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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
                if (value == null || value == Card.Empty)
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
        private Image smallImage;

        public delegate void ClickEvent();
        public ClickEvent MouseClickEnterEvent;
        public ClickEvent MouseCLickUpdateEvent;
        public ClickEvent MouseClickExitEvent;

        public bool HasCard() => card != null;

        private void Start()
        {
            privewImage.Hide();
        }

        // 이미지를 클릭했을 때 
        public void OnPointerClick(PointerEventData eventData)
        {
            MouseClickEnterEvent?.Invoke();
            //use Card
            Card = null;
            privewImage.Hide();

            MouseClickExitEvent?.Invoke();
        }

        private IEnumerator StartCardClickEventCoroutine()
        {
            MouseClickEnterEvent?.Invoke();
            //use Card
            Card = null;
            privewImage.Hide();
            yield return new WaitForSeconds(2.0f);

            MouseClickExitEvent?.Invoke();
        }

        // 마우스 커서가 이미지 안으로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            privewImage.Show();

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
            privewImage.Hide();
            privewImage.Clear();
        }

        private void Awake()
        {
            smallImage = GetComponent<Image>();
        }

        public void Show()
        {
            smallImage.enabled = true;
        }

        public void Hide()
        {
            smallImage.enabled = false;
            privewImage.Hide();
        }
    }
}