using System;
using System.Collections.Generic;
using CardNameSpace.Base;

namespace DB
{
	public class GameDB
	{
		public List<Card> list;

		public GameDB()
		{
			list = new List<Card>();
			list.Add(new Card(new CardInfo("검기 발사 ", "전방을 향해 검기를 발사합니다.")));
			list.Add(new Card(new CardInfo("파이어 볼 ", "네 방향으로 파이어볼을 발사합니다.")));
			list.Add(new Card(new CardInfo("백 스텝", "적의 공격을 피해 뒤로 후퇴합니다.")));
			list.Add(new Card(new CardInfo("A", "A")));
			list.Add(new Card(new CardInfo("B", "B")));
			list.Add(new Card(new CardInfo("C", "C")));
		}
	}
}
