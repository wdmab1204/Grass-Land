using UnityEngine;

namespace GameEntity
{
    public abstract class Entity : MonoBehaviour
    {
        private int maxHp;
        public virtual int MaxHp { get => maxHp; protected set => maxHp = value; }
        [UnityEngine.SerializeField] private int hp;
        public virtual int Hp { get => hp; set
            {
                hp = value;
                OnHealthChanged(value);
            }
        }
        public System.Action<int> OnHealthChanged;
        [SerializeField] protected TilemapReader TilemapReader;
        public Vector3Int LocalPosition => TilemapReader.ChangeWorldToLocalPosition(this.transform.position);
        public Vector3 WorldPosition => this.transform.position;
        public abstract void TakeDamage(Entity attacker, int damage);
        public abstract void Recovery(int amount);
    }
}