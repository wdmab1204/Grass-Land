using UnityEngine;
using System.Collections;
using CardNameSpace.Base;
using UnityEngine.EventSystems;

namespace CardNameSpace
{
    public class CardHandler : MonoBehaviour, IPointerClickHandler
    {
        private Card card;
        public bool hasCard { get => card != null; }

        public bool SetCard(Card card)
        {
            if (hasCard) return false;
            this.card = card;
            return true;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            // use card
        }
    }
}