using UnityEngine;
using System.Collections;
using System;

namespace GameEntity
{
    public class PlayerEntity : Entity
    {
        public override int Hp
        {
            get => base.Hp;
            protected set
            {
                base.Hp = value;
                OnHealthChanged(Hp);
            }
        }
        public Action deathAction;
        public override void TakeDamage(int damage)
        {
            Hp -= damage;
            if(Hp<=0)
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

