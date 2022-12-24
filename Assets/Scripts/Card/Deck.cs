using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardNameSpace.Base;

namespace CardNameSpace
{
    public class Deck : IEnumerable
    {
        private int maxCount;
        private List<Card> originalCardList;
        private Queue<Card> cardQueue;
        private Card[] cardArray { get => cardQueue.ToArray(); }

        public Deck(Card[] cards, int maxCount)
        {
            originalCardList = cards.ToList();
            cardQueue = new Queue<Card>();
            this.maxCount = maxCount;
        }

        private void Shuffle()
        {
            Random random = new Random();

            var cardList = cardQueue.ToList();
            cardList = cardList.OrderBy(x => random.Next()).ToList();
            
            cardQueue = new Queue<Card>(cardList);
        }

        public Card DrawCard() => cardQueue.Dequeue();

        /// <summary>
        /// Add a new card to the deck.
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public bool AddCard(Card card)
        {
            if (cardQueue.Count + 1 > maxCount) return false;
            originalCardList.Add(card);
            cardQueue.Enqueue(card);
            return true;
        }

        /// <summary>
        /// Return true if the deck is empty, otherwise false.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty() => cardQueue.Count == 0;

        /// <summary>
        /// When all the cards have been used, refill the deck.
        /// </summary>
        /// <returns></returns>
        public bool Refill()
        {
            if (originalCardList.Count == 0) return false;
            cardQueue = new Queue<Card>(originalCardList);
            return true;
        }

        public override string ToString()
        {
            Dictionary<Card, int> deckInfos = new Dictionary<Card, int>();
            foreach(var card in cardArray)
            {
                if (deckInfos.ContainsKey(card)) deckInfos[card]++;
                else deckInfos[card] = 1;
            }

            var wynik = new System.Text.StringBuilder("{");
            foreach (var deckInfo in deckInfos)
            {
                wynik.Append($"{{{deckInfo.Key.info.name}'s Count : {deckInfo.Value}}} ;");
            }
            return wynik.Append('}').ToString();
        }

        private class MyEnumerator : IEnumerator
        {
            public Card[] cardList;
            int position = -1;

            //constructor
            public MyEnumerator(Card[] list)
            {
                cardList = list;
            }
            private IEnumerator getEnumerator()
            {
                return (IEnumerator)this;
            }
            //IEnumerator
            public bool MoveNext()
            {
                position++;
                return (position < cardList.Length);
            }
            //IEnumerator
            public void Reset()
            {
                position = -1;
            }
            //IEnumerator
            public object Current
            {
                get
                {
                    try
                    {
                        return cardList[position];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new MyEnumerator(cardArray);
        }
    }
}
