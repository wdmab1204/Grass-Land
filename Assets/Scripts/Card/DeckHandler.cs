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
        private List<CardHandler> cardHandlerList = new List<CardHandler>();
        [SerializeField] private CardHandler cardHandlerPrefab;
        private bool isCLickedCard = false;
        [SerializeField] private CardPrivew priviewImage;
        [SerializeField] private RangeTile rangeTilePrefab;
        private RangeTile[] rangeTiles = new RangeTile[10];

        public bool DrawCard()
        {
            if (deck.IsEmpty()) return false;

            var card = deck.DrawCard();
            var handler = Instantiate<CardHandler>(cardHandlerPrefab, this.transform);
            cardHandlerList.Add(handler);
            handler.Card = card ?? Card.Empty;
            handler.PointerEnterEvent += ShowPriviewImage;
            handler.PointerExitEvent += HidePriviewImage;

            return true;
        }

        private void ShowPriviewImage(CardInfo card)
        {
            priviewImage.Show();
            priviewImage.NameText = card.name ?? "Unknown";
            priviewImage.DescText = card.desc ?? "No description available.";
        }

        private void HidePriviewImage(CardInfo card)
        {
            priviewImage.Clear();
            priviewImage.Hide();
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
            foreach (var handler in cardHandlerList)
            {
                allEmpty = allEmpty && !handler.HasCard();
            }
            return allEmpty;
        }

        public IEnumerator WaitForClickCard()
        {
            yield return new WaitUntil(() => isCLickedCard);
        }

        private void Awake()
        {
            this.deck = new Deck(CreateCardsFromDatabase());
            deck.Shuffle();
            DrawCards(3);
            foreach (var handler in cardHandlerList)
            {
                handler.MouseClickEnterEvent += (card) => isCLickedCard = true;

                handler.MouseClickExitEvent += HandleCardExitEvent;

                Hide();
            }
        }

        private void Start()
        {
            for(int i=0; i<rangeTiles.Length; i++)
            {
                rangeTiles[i] = Instantiate<RangeTile>(rangeTilePrefab);
                rangeTiles[i].Hide();
            }
        }

        private void HandleCardExitEvent(CardInfo card)
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
            foreach(var cardHandler in cardHandlerList)
            {
                cardHandler.Show();
            }
            isCLickedCard = false;
        }

        public void Hide()
        {
            GetComponent<Image>().enabled = false;
            foreach (var cardHandler in cardHandlerList)
            {
                cardHandler.Hide();
            }
        }
    }
}
