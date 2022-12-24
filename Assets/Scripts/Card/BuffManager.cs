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
            if (itemDic.ContainsKey(item.info.name))
            {
                itemDic[item.info.name].Upgrade();
            }
            else
            {
                //가지고 있지 않다면
                itemDic.Add(item.info.name, item);
                itemDic[item.info.name].Start();
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