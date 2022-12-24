using UnityEditor;
using UnityEngine;

namespace CardNameSpace
{
    public enum CardType
    {
        Attack,
        Move,
        Heal
    };

    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object Asset/Item")]
    public class CardScriptableObject : ScriptableObject
    {
        new public string name = "New Card";
        public Sprite img = null;
        public string description;
        public CardType cardType;
    }
}