using System;
using ArcanaSalvage.UI;
using Assets.Code.Scripts.Game.Player;
using Code.Scripts.Game.Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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
        
        EntityCommandBuffer entityCommandBufferHealthManager = new EntityCommandBuffer(Allocator.TempJob);
        HealthManagerJob healthManagerJob = new HealthManagerJob
        {
            EntityCommandBuffer = entityCommandBufferHealthManager,
        };
        healthManagerJob.Schedule();
        
        // Used for PlayerDied and KillCount
        NativeArray<Inventory> invSingletonNative = new NativeArray<Inventory>(1, Allocator.TempJob);

        SystemAPI.TryGetSingleton(out Inventory invSingleton);
        invSingletonNative[0] = invSingleton;
        
        EntityCommandBuffer entityCommandBufferEnemyHealth = new EntityCommandBuffer(Allocator.TempJob);
        EnemyDeathJob enemyDeathJob = new EnemyDeathJob
        {
            EntityCommandBuffer = entityCommandBufferEnemyHealth,
            InventorySingleton = invSingletonNative
        };
        enemyDeathJob.Schedule();
        
        EntityCommandBuffer entityCommandBufferPlayerHealthManager = new EntityCommandBuffer(Allocator.TempJob);
        PlayerDeathJob playerDeathJob = new PlayerDeathJob
        {
            EntityCommandBuffer = entityCommandBufferPlayerHealthManager,
            InventorySingleton = invSingletonNative
        };
        playerDeathJob.Schedule();
        state.Dependency.Complete();

        entityCommandBufferLifetimeManager.Playback(state.EntityManager);
        entityCommandBufferLifetimeManager.Dispose();
        entityCommandBufferHealthManager.Playback(state.EntityManager);
        entityCommandBufferHealthManager.Dispose();
        entityCommandBufferEnemyHealth.Playback(state.EntityManager);
        entityCommandBufferEnemyHealth.Dispose();
        entityCommandBufferPlayerHealthManager.Playback(state.EntityManager);
        entityCommandBufferPlayerHealthManager.Dispose();
        

        SystemAPI.SetSingleton(invSingletonNative[0]);

        invSingletonNative.Dispose();
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

    [BurstCompile, WithAll(typeof(Health)), WithNone(typeof(InputVariables), typeof(Enemy))]
    public partial struct HealthManagerJob : IJobEntity
    {
        public EntityCommandBuffer EntityCommandBuffer;

        private void Execute(Entity entity ,in Health health) 
        {
            if (health.CurrentHealth > 0.0f)
                return;
            
            if (health.DieOnDeath)
            {
                EntityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
    
    [BurstCompile, WithAll(typeof(Health), typeof(Enemy))]
    public partial struct EnemyDeathJob : IJobEntity
    {
        public EntityCommandBuffer EntityCommandBuffer;
        public NativeArray<Inventory> InventorySingleton;

        private void Execute(Entity entity ,in Health health) 
        {
            if (health.CurrentHealth > 0.0f)
                return;
            
            Inventory dataSingleton = InventorySingleton[0];
            
            if (health.DieOnDeath)
            {
                dataSingleton.KillsCounter++;
                EntityCommandBuffer.DestroyEntity(entity);
            }
            //Do something object pooling idk;
            
            InventorySingleton[0] = dataSingleton;
        }
    }
    
    [BurstCompile, WithAll(typeof(InputVariables), typeof(Health))]
    public partial struct PlayerDeathJob : IJobEntity
    {
        public EntityCommandBuffer EntityCommandBuffer;
        public NativeArray<Inventory> InventorySingleton;

        private void Execute(Entity entity ,in Health health) 
        {
            if (health.CurrentHealth > 0.0f)
                return;

            Inventory dataSingleton = InventorySingleton[0];

            dataSingleton.PlayerDead = true;

            if (health.DieOnDeath)
            {
                EntityCommandBuffer.DestroyEntity(entity);
            }
            //Do something object pooling idk;

            InventorySingleton[0] = dataSingleton;
        }
    }
}
