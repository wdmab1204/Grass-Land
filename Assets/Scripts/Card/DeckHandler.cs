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
        private Deck deck { get; set; }
        [SerializeField] private CardHandler[] cardHandlers;
        private bool isCLickedCard = false;

        private void SendCardToHandler(Card card, CardHandler cardHandler)
        {
            cardHandler.Card = card ?? Card.Empty;
        }

        public bool DrawCard()
        {
            if (deck.IsEmpty()) return false;

            var card = deck.DrawCard();
            foreach (var handler in cardHandlers)
            {
                if (!handler.HasCard())
                {
                    SendCardToHandler(card, handler);
                    return true;
                }
            }
            return false;
        }

        public bool DrawCards(int count)
        {
            for (int i = 0; i < count; i++)
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

        public bool AreHandlersEmpty()
        {
            bool allEmpty = true;
            foreach (var handler in cardHandlers)
            {
                allEmpty = allEmpty && !handler.HasCard();
            }
            return allEmpty;
        }

        public IEnumerator WaitForClickCard()
        {
            yield return new WaitUntil(() => isCLickedCard);
        }

        private void Start()
        {
            this.deck = new Deck(CreateCardsFromDatabase());
            deck.Shuffle();
            DrawCards(3);
            foreach (var handler in cardHandlers)
            {
                handler.MouseClickEnterEvent += () => isCLickedCard = true;

                handler.MouseClickExitEvent += HandleCardExitEvent;
                handler.MouseClickExitEvent += () => handler.previewImage.Show();

                Hide();
            }
        }

        private void HandleCardExitEvent()
        {
            if (AreHandlersEmpty())
            {
                deck = new Deck(CreateCardsFromDatabase());
                deck.Shuffle();
                DrawCards(3);
            }
            else
            {
                DrawCard();
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
