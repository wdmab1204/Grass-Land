using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardNameSpace
{

    public class CardPrivew : MonoBehaviour, IGraphicsDisplay
    {
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text descText;
        private Image cardImage;

        public string NameText
        {
            get => nameText.text;
            set => nameText.text = value;
        }

        public string DescText
        {
            get => descText.text;
            set => descText.text = value;
        }

        public bool Clear()
        {
            if (nameText == null || descText == null) return false;

            nameText.text = "";
            descText.text = "";

            return true;
        }

        public void Hide()
        {
            cardImage.enabled = false;
            nameText.enabled = false;
            descText.enabled = false;
        }

        public void Show()
        {
            cardImage.enabled = true;
            nameText.enabled = true;
            descText.enabled = true;
        }


        void Start()
        {
            cardImage = GetComponent<Image>();
        }
    }
}