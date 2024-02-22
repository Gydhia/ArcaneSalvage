using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Helper;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;

public class PoolingAuthoring : Singleton<PoolingAuthoring>
{

    [SerializeField]
    public List<DictionnaryItems> _gameObjectItems;

    public Dictionary<PoolingType, NativeArray<Entity>> pool;

    public EntityManager entityManager;
    
    

    /*private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        pool = new Dictionary<PoolingType, NativeArray<Entity>>();
            
        for (int i = 0; i < _gameObjectItems.Count; i++)
        {
            Entity entity = entityManager.CreateEntity();
            entityManager.AddComponent(entity, typeof(PoolableComponent));
            
            PoolableComponent poolComp = new PoolableComponent()
            {
                isActive = false
            };
            entityManager.SetComponentData(entity, poolComp);
            
            //Debug.Log(entity);
            PoolableComponent poolableTest = entityManager.GetComponentData<PoolableComponent>(entity);
            
            entityManager.AddComponentObject(entity, _gameObjectItems[i]._entity);
         
            NativeArray<Entity> entities = new NativeArray<Entity>(_gameObjectItems[i]._nbrOfGameObject, Allocator.Persistent);
            entityManager.Instantiate(entity,entities);
            pool.Add(_gameObjectItems[i]._poolingType, entities);
        }
    }*/

    public class BakerScript : Baker<PoolingAuthoring>
    {
        public override void Bake(PoolingAuthoring poolingManager)
        {
            PoolingAuthoring.Instance = poolingManager;
            PoolingAuthoring.Instance.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            PoolingAuthoring.Instance.pool = new Dictionary<PoolingType, NativeArray<Entity>>();

            Debug.Log("gros hdhdhkdfhjdfh" + PoolingAuthoring.Instance._gameObjectItems.Count);

            for (int i = 0; i < PoolingAuthoring.Instance._gameObjectItems.Count; i++)
            {
                Entity entity = GetEntity(PoolingAuthoring.Instance._gameObjectItems[i]._entity, TransformUsageFlags.Dynamic);
                PoolableComponent poolNew = new PoolableComponent()
                {
                    isActive = false
                };
                PoolingAuthoring.Instance.entityManager.AddComponent(entity, typeof(PoolableComponent));
                PoolingAuthoring.Instance.entityManager.SetComponentData(entity, poolNew);
                

                NativeArray<Entity> entities = new NativeArray<Entity>(PoolingAuthoring.Instance._gameObjectItems[i]._nbrOfGameObject, Allocator.Persistent);
                PoolingAuthoring.Instance.entityManager.Instantiate(entity, entities);
                PoolingAuthoring.Instance.pool.Add(PoolingAuthoring.Instance._gameObjectItems[i]._poolingType, entities);
                Debug.Log(PoolingAuthoring.Instance.pool.Count);

            }
        }
    }

    public Entity GetObject(PoolingType prefabType)
    {
        
        Debug.Log(pool);
        
        for (int i = 0; i < pool[prefabType].Length; i++)
        {
            Entity entity = pool[prefabType][i];
            
            PoolableComponent poolable = entityManager.GetComponentData<PoolableComponent>(entity);
            if (!poolable.isActive)
            {
                entityManager.SetComponentData(entity, new PoolableComponent
                {
                    isActive = true
                });
                return entity;
            }
        }

        return Entity.Null;
    }

    public void ReturnObject(Entity entity)
    {
        entityManager.SetComponentData(entity, new PoolableComponent
        {
            isActive = false
        });
    }

    public List<DictionnaryItems> GetGameObjects()
    {
        return _gameObjectItems;
    }
}

public struct PoolableComponent : IComponentData
{
    public bool isActive;
}


[Serializable]
public struct DictionnaryItems
{
    public GameObject _entity;
    public int _nbrOfGameObject;
    public PoolingType _poolingType;
}

public enum PoolingType
{
    BulletEnnemy,
    BulletPlayer,
    Ennemy
}
