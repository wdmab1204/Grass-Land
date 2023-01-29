using UnityEngine;
using System.Collections;
using System;

namespace GameEntity
{
    public class MonsterEntity : Entity
    {
        [SerializeField] private int hp;
        public Action deathAction;
        public override void TakeDamage(int damage)
        {
            hp -= damage;

            if (hp <= 0)
                deathAction();
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

