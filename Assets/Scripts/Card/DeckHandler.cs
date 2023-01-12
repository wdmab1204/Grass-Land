using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CardNameSpace.Base;
using UnityEngine;
using UnityEngine.UI;

namespace CardNameSpace
{
    public class DeckHandler : MonoBehaviour, IGraphicsDisplay
    {
        private Deck Deck { get; set; }
        [SerializeField] private CardHandler[] cardHandlers;
        private bool isCLickedCard = false;

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

        public IEnumerator WaitForClickCard()
        {
            yield return new WaitUntil(() => isCLickedCard);
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
                handler.MouseClickEnterEvent += () => isCLickedCard = true;

                handler.MouseClickExitEvent += () => handler.privewImage.Show();
                

                Hide();

                Debug.Log(handler.Card.ToString());
            }
            
        }

        public void Show()
        {
            GetComponent<Image>().enabled = true;
            foreach(var cardHandler in cardHandlers)
            {
                cardHandler.Show();
            }
            isCLickedCard = false;
        }

        public void Hide()
        {
            GetComponent<Image>().enabled = false;
            foreach (var cardHandler in cardHandlers)
            {
                cardHandler.Hide();
            }
        }
    }
}
