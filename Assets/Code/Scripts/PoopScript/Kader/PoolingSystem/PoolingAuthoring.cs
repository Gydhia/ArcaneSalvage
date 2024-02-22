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
    public List<DictionnaryItems> _gameObjectItems = new List<DictionnaryItems>();

    public Dictionary<PoolingType, NativeArray<Entity>> pool;

    public EntityManager entityManager;

    public class BakerScript : Baker<PoolingAuthoring>
    {
        public override void Bake(PoolingAuthoring poolingManager)
        {
            poolingManager.entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

            poolingManager.pool = new Dictionary<PoolingType, NativeArray<Entity>>();

            for (int i = 0; i < poolingManager._gameObjectItems.Count; i++)
            {
                Entity entity = poolingManager.entityManager.CreateEntity(typeof(PoolableComponent));
                poolingManager.entityManager.SetComponentData(entity, new PoolableComponent
                {
                    isActive = false
                });
                poolingManager.entityManager.AddComponentObject(entity, poolingManager._gameObjectItems[i]._entity);
         
                NativeArray<Entity> entities = new NativeArray<Entity>(poolingManager._gameObjectItems[i]._nbrOfGameObject, Allocator.Persistent);
                poolingManager.entityManager.Instantiate(entity,entities);
                poolingManager.pool.Add(poolingManager._gameObjectItems[i]._poolingType, entities);
            }

            /*Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            for (int i = 0; i < poolingManager._gameObjectItems.Count; i++)
            {
                Entity newEntity = entity;
                
                AddComponent(entity, new IPoolingEntityComponent()
                {
                    nbrOfEntity = poolingManager._gameObjectItems[i]._nbrOfGameObject,
                    entity = GetEntity(poolingManager._gameObjectItems[i]._entity, TransformUsageFlags.Dynamic)
                });
                
            }*/
        }
    }
    
    public Entity GetObject(PoolingType prefabType)
    {
        
        
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
