using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CardNameSpace.Base;
using UnityEngine;

namespace CardNameSpace
{
    public class DeckHandler : MonoBehaviour
    {
        private Deck Deck { get; set; }
        [SerializeField] private CardHandler[] cardHandlers;

        private bool SendToHand(Card card, CardHandler cardHandler) => cardHandler.SetCard(card);

        public bool DrawCard()
        {
            var card = Deck.DrawCard();
            for(int i=0; i<cardHandlers.Length; i++)
            {
                if (!cardHandlers[i].HasCard)
                {
                    SendToHand(card, cardHandlers[i]);
                    return true;
                }
            }
            return false;
        }

        public bool DrawCard(int count)
        {
            while (count-->0)
            {
                if (!DrawCard()) return false;
            }
            return true;
        }

        private Card[] CreateCardsFromDatabase()
        {
            DB.GameDB gameDB = new DB.GameDB();
            Card[] result = gameDB.list.ToArray();
            return result;
        }


        private void Start()
        {
            this.Deck = new Deck(CreateCardsFromDatabase());
            Deck.Shuffle();
            DrawCard(3);
        }
    }
}
