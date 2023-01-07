using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace CardNameSpace.Base
{
    public enum CardType
    {
        ATTACK,
        MOVE,
        HEAL,
        ETC
    };

    [System.Serializable]
    public class CardInfo
    {
        public string name;
        public string desc;
        public CardType cardType;

        public CardInfo(string name, string desc, CardType cardType)
        {
            this.name = name;
            this.desc = desc;
            this.cardType = cardType;
        }

        public override string ToString()
        {
            var wynik = new System.Text.StringBuilder("[");
            wynik.Append($"{nameof(name)} : {name}, {nameof(desc)} : {desc}");
            
            return wynik.Append("]").ToString();
        }
    }

    

    public class Card
    {
        public CardInfo CardInfo { get; set; }
        protected object User { get; set; }

        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void Exit() { }
        public virtual void Upgrade() { }

        public Card(CardInfo cardInfo)
        {
            this.CardInfo = cardInfo;
        }

        public override string ToString()
        {
            return CardInfo.ToString();
        }

        public static Card Empty
        {
            get => new Card(new CardInfo("", "", CardType.ETC));
        }

    }
}
