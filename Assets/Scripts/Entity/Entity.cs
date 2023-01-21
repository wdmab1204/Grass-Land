namespace GameEntity
{
    public abstract class Entity : UnityEngine.MonoBehaviour
    {
        protected int maxHp;

        public abstract void TakeDamage(int damage);
        public abstract void Recovery(int amount);
    }
}