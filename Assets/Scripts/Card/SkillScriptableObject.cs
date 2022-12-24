using UnityEditor;
using UnityEngine;

namespace CardNameSpace
{
    [CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object Asset/Skill")]
    public class SkillScriptableObject : CardScriptableObject
    {
        public int objCount;
        public float objSpeed;
        public float circleRadius;
        public float objDistance;
        public GameObject objPrefab;
    }
}