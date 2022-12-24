using UnityEditor;
using UnityEngine;


namespace CardNameSpace.Base
{

    public struct CardInfo
    {
        public string name;
        public Sprite img;
        public string desc;

        public CardInfo(string name, Sprite img, string desc)
        {
            this.name = name;
            this.img = img;
            this.desc = desc;
        }
    }

    public struct SkillInfo
    {
        public CardInfo itemInfo;
        public int objCount;
        public float objSpeed;
        public float circleRadius;
        public float objDistance;
        public GameObject objPrefab;

        public SkillInfo(string name, Sprite icon, int objCount, float objSpeed, float circleRadius, float objDistance, GameObject objPrefab) : this()
        {
            this.itemInfo.name = name;
            this.itemInfo.img = icon;
            this.objCount = objCount;
            this.objSpeed = objSpeed;
            this.circleRadius = circleRadius;
            this.objDistance = objDistance;
            this.objPrefab = objPrefab;
        }
    }

    public abstract class Card
    {
        public CardInfo info;
        protected object user;

        public Card(CardInfo cardInfo) 
        {
            this.info = cardInfo;
        }

        public abstract void Start();
        public abstract void Update();
        public abstract void Exit();
        public virtual void SetUser(object user) => this.user = user;
        public abstract void Upgrade();

    }
}
