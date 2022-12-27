using System.Collections;
using System.Collections.Generic;
using CardNameSpace.Base;
using UnityEngine;

namespace CardNameSpace
{
    public class DeckHandler : MonoBehaviour
    {
        private Deck Deck { get; set; }

        private CardHandler[] cardHandlers;

        private bool SendToCard(Card card, CardHandler cardHandler) => cardHandler.SetCard(card);
    }
}
