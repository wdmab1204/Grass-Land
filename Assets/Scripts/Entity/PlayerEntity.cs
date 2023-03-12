﻿using UnityEngine;
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
        public override void TakeDamage(int damage)
        {
            Hp -= damage;
            if(Hp<=0)
                OnDeath();
        }

        public override void Recovery(int amount)
        {
            Hp += amount;
        }

        protected override void Awake()
        {
            base.Awake();
            this.MaxHp = Hp;
        }
    }
}

