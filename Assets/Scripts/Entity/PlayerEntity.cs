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
            set
            {
                base.Hp = value;
                OnHealthChanged(Hp);
            }
        }
        public Action OnDeath;
        public Action<MonsterEntity, PlayerEntity> OnTakeDamage;
        public override void TakeDamage(Entity attacker, int damage)
        {
            Hp -= damage;
            OnTakeDamage((MonsterEntity)attacker, this);
            if(Hp<=0)
                OnDeath();
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

