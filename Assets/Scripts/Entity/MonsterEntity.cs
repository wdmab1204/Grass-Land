using UnityEngine;
using System.Collections;
using System;

namespace GameEntity
{
    public class MonsterEntity : Entity
    {
        public Action deathAction;
        public override void TakeDamage(int damage)
        {
            Hp -= damage;
            OnHealthChanged(this.Hp);
            if (Hp <= 0)
                deathAction();
        }

        public override void Recovery(int amount)
        {
            Hp += amount;
        }

        private void Awake()
        {
            this.MaxHp = Hp;
        }
    }
}

