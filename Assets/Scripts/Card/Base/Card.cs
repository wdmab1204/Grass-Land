using UnityEditor;
using UnityEngine;


namespace CardNameSpace.Base
{

    public class CardInfo
    {
        public string name;
        public string desc;

        public CardInfo(string name, string desc)
        {
            this.name = name;
            this.desc = desc;
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

    }
}
