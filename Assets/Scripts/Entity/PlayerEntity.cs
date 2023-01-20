using UnityEngine;
using System.Collections;

namespace GameEntity
{
    public class PlayerEntity : Entity
    {
        [SerializeField] private int hp;

        public void TakeDamage(int damage)
        {
            hp -= damage;
        }

        public void Recovery(int amount)
        {
            hp += amount;
        }

        private void Awake()
        {
            this.maxHp = hp;
        }
    }
}

