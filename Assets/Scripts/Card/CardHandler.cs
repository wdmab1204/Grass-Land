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
        private Image cardImage;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descText;
        [SerializeField] private GameObject privewImage;

        public bool HasCard { get => card != null; }

        public bool SetCard(Card card)
        {
            if (HasCard) return false;
            this.card = card;
            
            return true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // use card

            card = null;
            nameText.text = "";
            descText.text = "";
            HideImage();
        }

        private void HideImage() => cardImage.enabled = false;

        private void ShowImage() => cardImage.enabled = true;

        private void Start()
        {
            privewImage.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            privewImage.SetActive(true);
            nameText.text = card.CardInfo.name;
            descText.text = card.CardInfo.desc;
        }

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