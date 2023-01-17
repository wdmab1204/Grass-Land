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
                    previewImage.Clear();
                }
                else
                {
                    previewImage.NameText = value.CardInfo.name ?? "Unknown";
                    previewImage.DescText = value.CardInfo.desc ?? "No description available.";

                }
                card = value;
                this.cardInfo = card?.CardInfo;
            }
        }

        public CardPrivew previewImage;
        private Image smallImage;

        public delegate void ClickEvent();
        public ClickEvent MouseClickEnterEvent;
        public ClickEvent MouseCLickUpdateEvent;
        public ClickEvent MouseClickExitEvent;

        public bool HasCard() => card != null;

        private void Start()
        {
            previewImage.Hide();
        }

        // 이미지를 클릭했을 때 
        public void OnPointerClick(PointerEventData eventData)
        {
            MouseClickEnterEvent?.Invoke();
            //use Card



            Card = null;
            previewImage.Hide();

            MouseClickExitEvent?.Invoke();
        }
        

        private IEnumerator StartCardClickEventCoroutine()
        {
            MouseClickEnterEvent?.Invoke();
            //use Card
            Card = null;
            previewImage.Hide();
            yield return new WaitForSeconds(2.0f);

            MouseClickExitEvent?.Invoke();
        }

        // 마우스 커서가 이미지 안으로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            previewImage.Show();

            previewImage.NameText = card?.CardInfo.name ?? "Unknown";
            previewImage.DescText = card?.CardInfo.desc ?? "No description available.";
        }

        // 마우스 커서가 이미지 밖으로 나갈 때
        public void OnPointerExit(PointerEventData eventData)
        {
            previewImage.Hide();
            previewImage.Clear();
        }

        private void Awake()
        {
            smallImage = GetComponent<Image>();
        }

        public void Show()
        {
            if (card == null)
            {
                return;
            }

            smallImage.enabled = true;
        }

        public void Hide()
        {
            smallImage.enabled = false;
            previewImage.Hide();
        }
    }
}