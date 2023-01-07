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


        private void SendToHand(Card card, CardHandler cardHandler)
        {
            if(card == null)
            {
                Debug.LogError("Handler doesn't have a card.");
                return;
            }
            cardHandler.Card = card;
        }

        public bool DrawCard()
        {
            if (Deck.IsEmpty()) return false;
            var card = Deck.DrawCard();
            for(int i=0; i<cardHandlers.Length; i++)
            {
                if (!cardHandlers[i].HasCard())
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
            Card[] result = DB.GameDB.GetDataArrayFromDB();
            return result;
        }

        public bool EmptyHands()
        {
            bool b = true;
            foreach(var handler in cardHandlers)
            {
                b = b && !handler.HasCard();
            }
            return b;
        }

        private void Start()
        {
            this.Deck = new Deck(CreateCardsFromDatabase());
            Deck.Shuffle();
            DrawCard(3);
            foreach (var handler in cardHandlers)
            {
                handler.MouseClickExitEvent += delegate ()
                {
                    bool b = EmptyHands();
                    Debug.Log("is empty : " + b.ToString());
                    if (EmptyHands())
                    {
                        this.Deck = new Deck(CreateCardsFromDatabase());
                        this.Deck.Shuffle();
                        Debug.Log("덱 리셋 ");
                        DrawCard(3);
                    }
                    else
                    {
                        DrawCard();
                    }
                };
                handler.MouseClickExitEvent += () => handler.privewImage.ShowImage();

                Debug.Log(handler.Card.ToString());
            }
            
        }
    }
}
