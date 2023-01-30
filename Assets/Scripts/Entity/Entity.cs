namespace GameEntity
{
    public abstract class Entity : UnityEngine.MonoBehaviour
    {
        private int maxHp;
        public virtual int MaxHp { get => maxHp; protected set => maxHp = value; }
        [UnityEngine.SerializeField] private int hp;
        public virtual int Hp { get => hp; protected set
            {
                hp = value;
                OnHealthChanged(value);
            }
        }
        public System.Action<int> OnHealthChanged;
        public abstract void TakeDamage(int damage);
        public abstract void Recovery(int amount);
    }
}