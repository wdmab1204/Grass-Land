﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TurnSystem;
using UnityEngine.SceneManagement;
using System;

namespace GameEntity
{
	public class EntityManager : MonoBehaviour
	{
		private List<Entity> entityList = new List<Entity>();
        [SerializeField] private TilemapReader TilemapReader;

        private void Start()
        {
            var objArr = SceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < objArr.Length; i++)
                if (objArr[i].TryGetComponent<Entity>(out Entity entity))
                    entityList.Add(entity);
        }

        public bool IsEntityOnTile<T>(Vector3Int tileLocalPosition) where T : Entity
        {
            if (TilemapReader.HasTile(tileLocalPosition)) return false;

            foreach (var entity in entityList)
            {
                var entityPosition = TilemapReader.ChangeWorldToLocalPosition(entity.transform.position);
                if (entityPosition == tileLocalPosition && entity is T)
                    return true;
            }
            return false;
        }

        public bool TryGetEntityOnTile<T>(Vector3Int tileLocalPosition, out Entity entity) where T : Entity
        {
            entity = null;
            if (!TilemapReader.HasTile(tileLocalPosition)) return false;

            foreach (var e in entityList)
            {
                var entityPosition = TilemapReader.ChangeWorldToLocalPosition(e.transform.position);
                if (entityPosition == tileLocalPosition && e is T)
                {
                    entity = e;
                    return true;
                }
                    
            }
            return false;
        }

    }
}

