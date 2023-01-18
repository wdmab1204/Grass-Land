using System;
using System.Collections.Generic;
using CardNameSpace.Base;

namespace DB
{
	public class GameDB
	{
        private GameDB() { }
		public static Card[] GetDataArrayFromDB()
		{
			List<Card> list = new List<Card>();
            list = new List<Card>();
            list.Add(new Card(new CardInfo("검기 발사", "전방을 향해 검을 휘두룹니다.", CardType.ATTACK, "[-1,1][0,1][1,1]")));
            list.Add(new Card(new CardInfo("파이어 볼", "네 방향으로 파이어볼을 발사합니다.", CardType.ATTACK, "[-4,0][4,0][0,-4][0,4]")));
            //list.Add(new Card(new CardInfo("뒷걸음질", "적의 공격을 피해 뒤로 후퇴합니다.", CardType.MOVE, "[0,-3]")));
            list.Add(new Card(new CardInfo("자가 치유", "다친 상처를 스스로 치유합니다.(+3)", CardType.HEAL, "[0,0]")));
            //list.Add(new Card(new CardInfo("순간이동", "자신이 바라보는 방향으로 순간이동합니다.", CardType.MOVE, "[0,4]")));
            list.Add(new Card(new CardInfo("화살 발사", "자신이 바라보는 방향으로 화살을 발사합니다.", CardType.ATTACK, "[0,4]")));
            list.Add(new Card(new CardInfo("아이스 스피어", "자신이 바라보는 방향으로 아이스 스피어를 발사합니다.", CardType.ATTACK, "[0,5]")));
            return list.ToArray();
        }
	}
}
