using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;


namespace CardNameSpace.Base
{
    using Coord = Vector2Int;
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
        public Coord[] Coverage { get; set; }

        public CardInfo(string name, string desc, CardType cardType, string rangesString)
        {
            this.name = name;
            this.desc = desc;
            this.cardType = cardType;
            this.Coverage = CoordConverter.ConvertToCoords(rangesString);
        }

        public override string ToString()
        {
            var wynik = new System.Text.StringBuilder("[");
            wynik.Append($"{nameof(name)} : {name}, {nameof(desc)} : {desc}");

            return wynik.Append("]").ToString();
        }
    }

    ///<Summary>
    /// <see href="https://www.notion.so/ga-gang/5fef78ed92694cc3afdd1d24cc656717?v=c80c097238ee4173ae46490d94ea5ad1">Click</see> here for a detailed description of the encrypted string.
    ///</Summary>
    public class CoordConverter
    {
        private static readonly string coordHead = "[";
        private static readonly string coordTail = "]";
        private static readonly string coordDelimiter = ",";
        private static readonly int coordMinX = -100;
        private static readonly int coordMaxX = 100;
        private static readonly int coordMinY = -100;
        private static readonly int coordMaxY = 100;

        /// <summary>
        /// ConvertToCoords method converts string of coordinates "[x1,y1][x2,y2][x3,y3]" into an array of Coord objects.
        ///Input string must be in correct format and x, y should be between 0 and 100
        /// </summary>
        /// <param name="coordsString"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Coord[] ConvertToCoords(string coordsString)
        {
            if (string.IsNullOrWhiteSpace(coordsString)) return null;

            // check if the input string is in the correct format
            if (!coordsString.StartsWith(coordHead) || !coordsString.EndsWith(coordTail))
            {
                throw new ArgumentException("Invalid input format. Expected format: [x1,y1][x2,y2][x3,y3]");
            }

            var coordList = new List<Coord>();
            int start = 0;
            int end = coordsString.IndexOf(coordTail);
            while (end > 0)
            {
                var coordString = coordsString.Substring(start, end - start);
                coordString = coordString.Replace(coordHead, "").Replace(coordTail, "");
                var coordValues = coordString.Split(coordDelimiter);
                if (coordValues.Length == 2 && int.TryParse(coordValues[0], out int x) && int.TryParse(coordValues[1], out int y) && x >= coordMinX && x <= coordMaxX && y >= coordMinY && y <= coordMaxY)
                {
                    var coord = new Coord(x, y);
                    if (!coordList.Contains(coord))
                    {
                        coordList.Add(coord);
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid input format. Expected format: [x1,y1][x2,y2][x3,y3]");
                }
                start = end + 1;
                end = coordsString.IndexOf(coordTail, start);
            }

            return coordList.ToArray();
        }
    }


    public class Card : ICard
    {
        public CardInfo CardInfo { get; set; }
        public object User { get; set; }

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
            get => new Card(new CardInfo("", "", CardType.ETC, ""));
        }
    }
}
