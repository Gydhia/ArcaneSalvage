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
    private List<DictionnaryItems> _gameObjectItems = new List<DictionnaryItems>();

    public class BakerScript : Baker<PoolingAuthoring>
    {
        private static GameObject entityGO;

        public override void Bake(PoolingAuthoring poolingManager)
        {

            EntityManager entitymanager = World.DefaultGameObjectInjectionWorld.EntityManager;
            Entity entityTest = entitymanager.CreateEntity();


            NativeArray<IPoolingEntityComponent> tamere = new NativeArray<IPoolingEntityComponent>(1, Allocator.Persistent);

            IPoolingEntityComponent cacaProut = new IPoolingEntityComponent()
            {
                nbrOfEntity = 98,
                entity = GetEntity(poolingManager._gameObjectItems[0]._entity, TransformUsageFlags.Dynamic)
            };
            

            //entitymanager.AddBuffer<IPoolingEntityComponent>(entityTest);
            //DynamicBuffer<IPoolingEntityComponent> entityBuffer = entitymanager.GetBuffer<IPoolingEntityComponent>(entityTest);

            tamere[0] = cacaProut;
            //entityBuffer.Add(cacaProut);
            
            //DynamicBuffer<IPoolingEntityComponent> intDynamicBuffer = entityBuffer.Reinterpret<IPoolingEntityComponent>();
            

            /*for (int i = 0; i < entityComponents.Length; i++)
            {
                IPoolingEntityComponent newPoolingEntity = new IPoolingEntityComponent()
                {
                    entity = GetEntity(poolingManager._gameObjectItems[i]._entity, TransformUsageFlags.Dynamic),
                    nbrOfEntity = poolingManager._gameObjectItems[i]._nbrOfGameObject
                };
                entityComponents[i] = newPoolingEntity;
            }*/



            Debug.Log("GROS FILS E PUTUUUUUUTTTTEEE !!!!!!!!!" + tamere[0].nbrOfEntity);

            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent<IPoolingComponent>(entity, new IPoolingComponent()
            {
                _gameObjects = tamere
            });



            


        }
    }

    public List<DictionnaryItems> GetGameObjects()
    {
        return _gameObjectItems;
    }
}


[Serializable]
public struct DictionnaryItems
{
    public GameObject _entity;
    public int _nbrOfGameObject;

}
