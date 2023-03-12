using UnityEngine;
using UnityEngine.Tilemaps;

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
        protected Tilemap tilemap;
        public Vector3Int LocalPosition => tilemap.ChangeWorldToLocalPosition(this.transform.position);
        public Vector3 WorldPosition => this.transform.position;
        public abstract void TakeDamage(int damage);
        public abstract void Recovery(int amount);

        protected virtual void Awake()
        {
            tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        }
    }
}