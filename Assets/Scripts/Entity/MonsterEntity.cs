using UnityEngine;
using System.Collections;

namespace GameEntity
{
    public class MonsterEntity : Entity
    {
        [SerializeField] private int hp;

        public override void TakeDamage(int damage)
        {
            hp -= damage;
        }

        public override void Recovery(int amount)
        {
            hp += amount;
        }

        private void Awake()
        {
            this.maxHp = hp;
        }
    }
}

