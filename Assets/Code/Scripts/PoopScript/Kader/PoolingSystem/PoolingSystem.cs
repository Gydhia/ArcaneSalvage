using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEngine;

public partial class PoolingSystem : SystemBase
{
    private EntityManager entityManager;
    
    private Entity poolingEntity;

    private IPoolingComponent poolingComponent;

    protected override void OnCreate()
    {
        
    }

    protected override void OnUpdate()
    {
        /*
        Entities.ForEach((IPoolingComponent entityPooling) =>
        {
            Debug.Log("TES GROSSE COUILLES " + entityPooling._gameObjects[0].nbrOfEntity);
        }).Schedule();
        */

        foreach (var entityPooling in SystemAPI.Query<RefRO<IPoolingComponent>>())
        {
            Debug.Log("TES GROSSE COUILLES " + entityPooling.ValueRO._gameObjects[0].nbrOfEntity);
        }
        
    }

    /*public void OnCreate(ref SystemState state)
    {
        //state.RequireForUpdate<IPoolingComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        
        /*foreach (var poolingComponentAspect in SystemAPI.Query<PoolingComponentAspect>())
        {
            if (poolingComponentAspect._poolingEntityComponent.Length != Core.ChunkSettings.ChunkBlockCount)
            {
                poolingComponentAspect._poolingEntityComponent = new NativeArray<IPoolingEntityComponent>(Core.ChunkSettings.ChunkBlockCount, Allocator.Persistent);;
            }
        }#1#
        
        

        PoolingComponentJob newPoolingComponentJob = new PoolingComponentJob();
        newPoolingComponentJob.Schedule();
        //state.Dependency = newPoolingComponentJob.ScheduleParallel(state.Dependency);
        /*entityManager = state.EntityManager;

        foreach (var VARIABLE in COLLECTION)
        {
            
        }
        
        poolingEntity = SystemAPI.GetSingletonEntity<IPoolingComponent>();

        poolingComponent = entityManager.GetComponentData<IPoolingComponent>(poolingEntity);
        
        Debug.Log("gros caca" + poolingComponent._gameObjects);#1#

        //Debug.Log("gros caca" + poolingComponent._gameObjects[0].nbrOfEntity);
        /*Debug.Log(component.EntityECS.ToString());
        Debug.Log(component.nbrEntity);#1#
    }
    
    [BurstCompile, WithAll(typeof(IPoolingComponent))]
    public partial struct PoolingComponentJob : IJobEntity
    {
        public void Execute(in IPoolingComponent poolingComponent)
        {
            Debug.Log("grosse bite");
            Debug.Log(poolingComponent._gameObjects.nbrOfEntity);
        }
    }*/
    
}

/*public readonly partial struct PoolingComponentAspect : IAspect
{
    private readonly RefRW<IPoolingComponent> _poolingComponent;
    public ref NativeArray<IPoolingEntityComponent> _gameObjectsRO => ref _poolingComponent.ValueRW._gameObjects;
 
    public NativeArray<IPoolingEntityComponent> _poolingEntityComponent
    {
        get => _poolingComponent.ValueRO._gameObjects;
        set => _poolingComponent.ValueRW._gameObjects = value;
    }
 
    public void SetBlock(int index, IPoolingEntityComponent gameObject)
    {
        _poolingComponent.ValueRW._gameObjects[index] = gameObject;
    }
}*/
