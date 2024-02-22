using System;
using ArcanaSalvage.UI;
using Assets.Code.Scripts.Game.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public partial struct HealthSystem : ISystem
{
    
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Health>();
    }

    public void OnUpdate(ref SystemState state)
    {
        Debug.Log("BEGAN HEALTH SYSTEM");
        
        EntityCommandBuffer entityCommandBufferLifetimeManager = new EntityCommandBuffer(Allocator.TempJob);
        
        LifetimeManagerJob lifetimeManagerJob = new LifetimeManagerJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            EntityCommandBuffer = entityCommandBufferLifetimeManager,
        };
        lifetimeManagerJob.Schedule();

        
        NativeArray<DataSingleton> dataSingletonNative = new NativeArray<DataSingleton>(1, Allocator.TempJob);

        SystemAPI.TryGetSingleton(out DataSingleton dataSingleton);
        dataSingletonNative[0] = dataSingleton;
        
        EntityCommandBuffer entityCommandBufferHealthManager = new EntityCommandBuffer(Allocator.TempJob);
        HealthManagerJob healthManagerJob = new HealthManagerJob
        {
            EntityCommandBuffer = entityCommandBufferHealthManager,
            DataSingleton = dataSingletonNative
        };
        healthManagerJob.Schedule();

        
        EntityCommandBuffer entityCommandBufferPlayerHealthManager = new EntityCommandBuffer(Allocator.TempJob);
        PlayerDeathManager playerDeathManager = new PlayerDeathManager
        {
            EntityCommandBuffer = entityCommandBufferPlayerHealthManager,
            DataSingleton = dataSingletonNative
        };
        playerDeathManager.Schedule();
        state.Dependency.Complete();

        entityCommandBufferLifetimeManager.Playback(state.EntityManager);
        entityCommandBufferLifetimeManager.Dispose();
        entityCommandBufferHealthManager.Playback(state.EntityManager);
        entityCommandBufferHealthManager.Dispose();
        entityCommandBufferPlayerHealthManager.Playback(state.EntityManager);
        entityCommandBufferPlayerHealthManager.Dispose();

        SystemAPI.SetSingleton(dataSingletonNative[0]);

        dataSingletonNative.Dispose();
    }
    
    [BurstCompile]
    public partial struct LifetimeManagerJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer EntityCommandBuffer;
        private void Execute(Entity entity, ref Health health)
        {
            if (health.Lifetime <= -1.0f)
                return;

            health.Lifetime -= DeltaTime;

            if (health.Lifetime > 0.0f)
                return;
            
            EntityCommandBuffer.DestroyEntity(entity);
        }
    }

    [BurstCompile, WithAll(typeof(Health)), WithNone(typeof(InputVariables))]
    public partial struct HealthManagerJob : IJobEntity
    {
        public EntityCommandBuffer EntityCommandBuffer;
        public NativeArray<DataSingleton> DataSingleton;

        private void Execute(Entity entity ,in Health health) 
        {
            if (health.CurrentHealth > 0.0f)
                return;
            
            DataSingleton dataSingleton = DataSingleton[0];
            
            if (health.DieOnDeath)
            {
                dataSingleton.KillsCounter++;
                EntityCommandBuffer.DestroyEntity(entity);
            }
            //Do something object pooling idk;
            
            DataSingleton[0] = dataSingleton;
        }
    }
    
    [BurstCompile, WithAll(typeof(InputVariables), typeof(Health))]
    public partial struct PlayerDeathManager : IJobEntity
    {
        public EntityCommandBuffer EntityCommandBuffer;
        public NativeArray<DataSingleton> DataSingleton;

        private void Execute(Entity entity ,in Health health) 
        {
            if (health.CurrentHealth > 0.0f)
                return;

            DataSingleton dataSingleton = DataSingleton[0];

            dataSingleton.PlayerDead = true;

            if (health.DieOnDeath)
            {
                EntityCommandBuffer.DestroyEntity(entity);
            }
            //Do something object pooling idk;

            DataSingleton[0] = dataSingleton;
        }
    }
}
