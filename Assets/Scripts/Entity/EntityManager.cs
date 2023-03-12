using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TurnSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System;

namespace GameEntity
{
	public class EntityManager : MonoBehaviour
	{
		private List<Entity> entityList = new List<Entity>();
        private Tilemap tilemap;

        private void Awake()
        {
            var objArr = SceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < objArr.Length; i++)
                if (objArr[i].TryGetComponent<Entity>(out Entity entity))
                    entityList.Add(entity);

            tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        }

        public bool IsEntityOnTile<T>(Vector3Int tileLocalPosition) where T : Entity
        {
            if (tilemap.HasTile(tileLocalPosition)) return false;

            foreach (var entity in entityList)
            {
                var entityPosition = tilemap.ChangeWorldToLocalPosition(entity.transform.position);
                if (entityPosition == tileLocalPosition && entity is T)
                    return true;
            }
            return false;
        }

        public bool TryGetEntityOnTile<T>(Vector3Int tileLocalPosition, out Entity entity) where T : Entity
        {
            entity = null;
            if (!tilemap.HasTile(tileLocalPosition)) return false;

            foreach (var e in entityList)
            {
                var entityPosition = tilemap.ChangeWorldToLocalPosition(e.transform.position);
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

