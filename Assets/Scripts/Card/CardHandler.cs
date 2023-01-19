using UnityEngine;
using System.Collections;
using CardNameSpace.Base;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using SimpleSpriteAnimator;

namespace CardNameSpace
{
    public class CardHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IGraphicsDisplay
    {
        private Card card;
        [SerializeField] CardInfo cardInfo;
        public Card Card
        {   get => card;
            set
            {
                if (value == null || value == Card.Empty)
                {
                    previewImage.Clear();
                }
                else
                {
                    previewImage.NameText = value.CardInfo.name ?? "Unknown";
                    previewImage.DescText = value.CardInfo.desc ?? "No description available.";

                }
                card = value;
                this.cardInfo = card?.CardInfo;
            }
        }

        public CardPrivew previewImage;
        private Image smallImage;

        public delegate void ClickEvent();
        public ClickEvent MouseClickEnterEvent;
        public ClickEvent MouseCLickUpdateEvent;
        public ClickEvent MouseClickExitEvent;

        public bool HasCard() => card != null;

        public SpriteAnimator animator;

        public RangeTile rangeTilePrefab;
        private RangeTile[] rangeTiles;
        public GameRuleSystem TurnSystem;
        public TilemapReader TilemapReader;

        private void Start()
        {
            previewImage.Hide();
            rangeTiles = new RangeTile[10];
            for(int i=0; i<rangeTiles.Length; i++)
            {
                rangeTiles[i] = Instantiate(rangeTilePrefab);
                rangeTiles[i].Hide();
            }
        }

        // 이미지를 클릭했을 때 
        public void OnPointerClick(PointerEventData eventData)
        {
            MouseClickEnterEvent?.Invoke();
            //use Card

            var dics = AnimationConverter.GetDics();
            var animationName = dics[card.CardInfo.name];
            animator.Play(animationName);
            Debug.Log(card.Coverage.Length);

            Card = null;
            previewImage.Hide();

            MouseClickExitEvent?.Invoke();
        }
        

        private IEnumerator StartCardClickEventCoroutine()
        {
            MouseClickEnterEvent?.Invoke();
            //use Card
            Card = null;
            previewImage.Hide();
            yield return new WaitForSeconds(2.0f);

            MouseClickExitEvent?.Invoke();
        }

        // 마우스 커서가 이미지 안으로 들어왔을 때
        public void OnPointerEnter(PointerEventData eventData)
        {
            previewImage.Show();

            previewImage.NameText = card?.CardInfo.name ?? "Unknown";
            previewImage.DescText = card?.CardInfo.desc ?? "No description available.";

            if (string.IsNullOrWhiteSpace(card.CardInfo.rangesString)) return;

            for(int i=0; i<card.Coverage.Length; i++)
            {
                var coord = card.Coverage[i];
                var currentActor = TurnSystem.CurrentActor;
                var worldPosition = currentActor.Actor.transform.position;

                var localPosition = TilemapReader.ChangeWorldToLocalPosition(worldPosition);
                var rangePosition = localPosition + (Vector3Int)coord;
                var rangeWorldPosition = TilemapReader.ChangeLocalToWorldPosition(rangePosition);

                rangeTiles[i].transform.position = rangeWorldPosition;
                rangeTiles[i].Show();
            }
        }

        // 마우스 커서가 이미지 밖으로 나갈 때
        public void OnPointerExit(PointerEventData eventData)
        {
            previewImage.Hide();
            previewImage.Clear();
            for (int i = 0; i < rangeTiles.Length; i++) rangeTiles[i].Hide();
        }

        private void Awake()
        {
            smallImage = GetComponent<Image>();
        }

        public void Show()
        {
            if (card == null)
            {
                return;
            }

            smallImage.enabled = true;
        }

        public void Hide()
        {
            smallImage.enabled = false;
            previewImage.Hide();
        }
    }
}