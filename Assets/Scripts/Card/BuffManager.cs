using System.Collections.Generic;
using CardNameSpace.Base;

namespace CardNameSpace
{
    public class BuffManager
    {

        Dictionary<string, Card> itemDic;

        public BuffManager()
        {
            itemDic = new Dictionary<string, Card>();
        }


        public void AddBuffListener(Card item)
        {
            //이미 아이템을 가지고있는가?
            if (itemDic.ContainsKey(item.CardInfo.name))
            {
                itemDic[item.CardInfo.name].Upgrade();
            }
            else
            {
                //가지고 있지 않다면
                itemDic.Add(item.CardInfo.name, item);
                itemDic[item.CardInfo.name].Start();
            }

        }

        public void DeleteBuffListener(Card buff)
        {
            buff.Exit();
        }

        public void Update()
        {
            foreach(var item in itemDic.Values)
            {
                item.Update();
            }
        }
    }
}