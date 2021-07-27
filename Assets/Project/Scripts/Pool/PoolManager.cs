using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using EmojiChat.UI;

namespace EmojiChat
{
    public class PoolManager : GameObjectSingleton<PoolManager>
    {
        private readonly PooledObjectType[] TypesToFree =
        {
            PooledObjectType.FreeOnBattleEnd,
        };

        private Dictionary<PooledBehaviour, Queue<PooledBehaviour>> _pooledObjects = new Dictionary<PooledBehaviour, Queue<PooledBehaviour>>();

        protected override void Init()
        {
            base.Init();

            //GameManager.GameLoaded += GameManager_GameLoaded;
            //ResultPopup.Quited += ResultPopup_Quited;
            //GameManager.GameFinished += GameManager_GameFinished;
        }

        protected override void DeInit()
        {
            base.DeInit();

            //GameManager.GameLoaded -= GameManager_GameLoaded;
            //GameManager.GameFinished -= GameManager_GameFinished;
            //ResultPopup.Quited -= ResultPopup_Quited;
        }

        private void PreparePool()
        {

        }

        private PooledBehaviour PrepareObject(PooledBehaviour pooledBehaviour, PooledObjectType pooledType)
        {
            PooledBehaviour obj = Instantiate(pooledBehaviour, gameObject.transform);
            obj.Prepare(pooledType);
            obj.Init();
            obj.Free();

            return obj;
        }

        private void Prepare(PooledBehaviour pooledBehaviour, PooledObjectType pooledType, int count)
        {
            if (!_pooledObjects.ContainsKey(pooledBehaviour))
            {
                Queue<PooledBehaviour> objectPool = new Queue<PooledBehaviour>();

                for (int i = 0; i < count; i++)
                {
                    var obj = PrepareObject(pooledBehaviour, pooledType);

                    objectPool.Enqueue(obj);
                }

                _pooledObjects.Add(pooledBehaviour, objectPool);
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogException(new Exception($"{pooledBehaviour} actialy contains in pooled objects"));
#endif
            }
        }

        public T Get<T>(PooledBehaviour obj, Vector3 position, Quaternion rotation, Transform parent = null) where T : PooledBehaviour
        {
            if (!_pooledObjects.ContainsKey(obj))
            {
                Prepare(obj, PooledObjectType.FreeOnBattleEnd, 1);

#if UNITY_EDITOR
                Debug.LogError("PoolObjects with Tag " + obj + " doesn't exist ..");
#endif
            }

            var pooledObject = _pooledObjects[obj].FirstOrDefault(item => item.IsFree);

            if (pooledObject == null)
            {
                pooledObject = PrepareObject(obj, PooledObjectType.FreeOnBattleEnd);

                _pooledObjects[obj].Enqueue(pooledObject);

#if UNITY_EDITOR || FORCE_DEBUG
                Debug.LogError($"prepare object: {obj}");
#endif
            }

            if (parent)
            {
                pooledObject.transform.SetParent(parent);
            }

            pooledObject.transform.position = position;
            pooledObject.transform.rotation = rotation;

            pooledObject.SpawnFromPool();

            return (T)pooledObject;
        }

        public void FreeObject(PooledBehaviour obj)
        {
            obj.Free();
        }

        private void FreePool()
        {
            foreach (var pair in _pooledObjects.ToArray())
            {
                if (TypesToFree.Contains(pair.Key.Type))
                {
                    DestroyAll(pair.Key);
                }
            }
        }

        private void DestroyAll(PooledBehaviour prefab)
        {
            if (_pooledObjects.TryGetValue(prefab, out Queue<PooledBehaviour> queue))
            {
                foreach (var entry in queue)
                {
                    Destroy(entry.gameObject);
                }

                _pooledObjects.Remove(prefab);
            }
        }

        private void GameManager_GameLoaded()
        {
            PreparePool();
        }

        private void GameManager_GameFinished()
        {
            //FreePool();
        }

        private void ResultPopup_Quited()
        {
            FreePool();
        }
    }
}