using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CardNameSpace.Base;
using UnityEngine;
using UnityEngine.UI;
using SimpleSpriteAnimator;

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
        [SerializeField] private TilemapReader TilemapReader;
        [SerializeField] private GameRuleSystem GameRuleSystem;
        [SerializeField] private int CardHandlerCount = 3;
        [SerializeField] private SpriteAnimator animator;

        public bool DrawCard()
        {
            if (deck.IsEmpty()) return false;

            var card = deck.DrawCard();
            var handler = Instantiate<CardHandler>(cardHandlerPrefab, this.transform);
            cardHandlerList.Add(handler);
            handler.Card = card ?? Card.Empty;
            handler.PointerEnterEvent += ShowPriviewImage;
            handler.PointerEnterEvent += ShowRangeTiles;
            handler.MouseClickEnterEvent += PlayAnimation;
            handler.PointerExitEvent += HidePriviewImage;
            handler.PointerExitEvent += HideRangeTiles;

            return true;
        }

        private void PlayAnimation(CardInfo card)
        {
            var dics = AnimationConverter.GetDics();
            var animationName = dics[card.name];
            animator.Play(animationName);
        }

        private void ShowPriviewImage(CardInfo card)
        {
            priviewImage.Show();
            priviewImage.NameText = card.name ?? "Unknown";
            priviewImage.DescText = card.desc ?? "No description available.";

            
        }

        private void ShowRangeTiles(CardInfo card)
        {
            if (card.Coverage.Length == 0) return;

            for (int i = 0; i < card.Coverage.Length; i++)
            {
                var coord = card.Coverage[i];
                var currentActor = GameRuleSystem.CurrentActor;
                var worldPosition = currentActor.Actor.transform.position;

                var localPosition = TilemapReader.ChangeWorldToLocalPosition(worldPosition);
                var rangePosition = localPosition + (Vector3Int)coord;
                var rangeWorldPosition = TilemapReader.ChangeLocalToWorldPosition(rangePosition);

                rangeTiles[i].transform.position = rangeWorldPosition;
                rangeTiles[i].Show();
            }
        }

        private void HidePriviewImage(CardInfo card)
        {
            priviewImage.Clear();
            priviewImage.Hide();
        }

        private void HideRangeTiles(CardInfo card)
        {
            for (int i = 0; i < rangeTiles.Length; i++) rangeTiles[i].Hide();
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
            DrawCards(CardHandlerCount);
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
            priviewImage.Hide();
        }

        private void HandleCardExitEvent(CardInfo card)
        {
            if (AreHandlersEmpty())
            {
                deck = new Deck(CreateCardsFromDatabase());
                deck.Shuffle();
                DrawCards(CardHandlerCount);
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
