using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using CardNameSpace.Base;
using UnityEngine;
using UnityEngine.UI;
using SimpleSpriteAnimator;
using UnityEngine.Tilemaps;

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
        private Tilemap tilemap;
        [SerializeField] private GameRuleSystem GameRuleSystem;
        [SerializeField] private int CardHandlerCount = 3;
        [SerializeField] private SpriteAnimator animator;
        //[SerializeField] private EntityManager EntityManager;
        //[SerializeField] private PlayerEntity PlayerEntity;

        public Action OnAttackTarget;

        public bool DrawCard()
        {
            if (deck.IsEmpty()) return false;

            var card = deck.DrawCard();
            var handler = Instantiate<CardHandler>(cardHandlerPrefab, this.transform);
            cardHandlerList.Add(handler);
            handler.Card = card ?? Card.Empty;


            handler.PointerEnterEvent += ShowPriviewImageDelegate;
            handler.PointerEnterEvent += ShowRangeTilesDelegate;
            
            handler.PointerExitEvent += HidePriviewImageDelegate;
            handler.PointerExitEvent += HideRangeTilesDelegate;

            handler.MouseClickEnterEvent += PlayAnimationDelegate;
            handler.MouseClickEnterEvent += EntityInteractionEventDelegate;
            handler.MouseClickEnterEvent += (card) => isCLickedCard = true;
            handler.MouseClickExitEvent += HandleCardExitEvent;

            return true;
        }

        //적들과 상호작용하는 이벤트. ex) 카드 효과 구현
        private void EntityInteractionEventDelegate(CardHandler handler)
        {
            var card = handler.Card;
            var currentActorObject = GameRuleSystem.CurrentActor;

            switch (card.CardInfo.cardType)
            {
                case CardType.ATTACK:
                    foreach(var tilePosition in card.CardInfo.Ranges)
                    {
                        var rangeLocalPosition = tilemap.ChangeWorldToLocalPosition(currentActorObject.transform.position) + (Vector3Int)tilePosition;
                        //공격범위 안에 적이 있는가 ?
                        //if (EntityManager.TryGetEntityOnTile<MonsterEntity>(rangeLocalPosition, out Entity target))
                        //{
                        //    ((MonsterEntity)target).TakeDamage(100);
                        //    OnAttackTarget?.Invoke();
                        //}
                    }
                    break;
                case CardType.MOVE:
                    break;
                case CardType.HEAL:
                    var currentActorEntity = currentActorObject.GetComponent<Entity>();
                    //currentActorEntity.Recovery(33);
                    break;
            }
        }

        private void RemoveCardHandler(CardHandler handler)
        {
            HidePriviewImageDelegate(handler);
            HideRangeTilesDelegate(handler);
            cardHandlerList.Remove(handler);
            Destroy(handler.gameObject);
        }

        public bool DrawCards(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!DrawCard()) return false;
            }
            return true;
        }

        private void PlayAnimationDelegate(CardHandler handler)
        {
            //var card = handler.Card.CardInfo;
            //var dics = AnimationConverter.GetDics();
            //var animationName = dics[card.name];
            //animator.Play(animationName);
            //yield return new WaitForSeconds(animator.AnimationLength);
            //animator.Play("Drink Potion");
        }

        private void ShowPriviewImageDelegate(CardHandler handler)
        {
            var card = handler.Card.CardInfo;
            priviewImage.Show();
            priviewImage.NameText = card.name ?? "Unknown";
            priviewImage.DescText = card.desc ?? "No description available.";
        }

        private void ShowRangeTilesDelegate(CardHandler handler)
        {
            var card = handler.Card.CardInfo;
            if (card.Ranges.Length == 0) return;

            for (int i = 0; i < card.Ranges.Length; i++)
            {
                var coord = card.Ranges[i];
                var currentActor = GameRuleSystem.CurrentActor;
                var worldPosition = currentActor.transform.position;

                var localPosition = tilemap.ChangeWorldToLocalPosition(worldPosition);
                var rangePosition = localPosition + (Vector3Int)coord;
                var rangeWorldPosition = tilemap.ChangeLocalToWorldPosition(rangePosition);

                rangeTiles[i].transform.position = rangeWorldPosition;
                rangeTiles[i].Show();
            }
        }

        private void HidePriviewImageDelegate(CardHandler handler)
        {
            priviewImage.Clear();
            priviewImage.Hide();
        }

        private void HideRangeTilesDelegate(CardHandler handler)
        {
            for (int i = 0; i < rangeTiles.Length; i++) rangeTiles[i].Hide();
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

        private void HandleCardExitEvent(CardHandler handler)
        {
            RemoveCardHandler(handler);

            // 손이 모두 비어있다면 덱을 다시 리필하고 3장 드로우
            if (AreHandlersEmpty())
            {
                deck = new Deck(CreateCardsFromDatabase());
                deck.Shuffle();
                DrawCards(CardHandlerCount);
            }
            // 덱에 카드가 남아있다면 1장 드로우
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

        private void Start()
        {
            for (int i = 0; i < rangeTiles.Length; i++)
            {
                rangeTiles[i] = Instantiate<RangeTile>(rangeTilePrefab);
                rangeTiles[i].Hide();
            }
            priviewImage.Hide();
        }

        private void Awake()
        {
            this.deck = new Deck(CreateCardsFromDatabase());
            deck.Shuffle();
            DrawCards(CardHandlerCount);
            Hide();

            tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        }
    }
}
