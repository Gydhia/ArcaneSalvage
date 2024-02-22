using System;
using Assets.Code.Scripts.Game.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public partial struct HealthSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Health>();
        state.RequireForUpdate<DataSingleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBufferLifetimeManager = new EntityCommandBuffer(Allocator.TempJob);

        LifetimeManagerJob lifetimeManagerJob = new LifetimeManagerJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            EntityCommandBuffer = entityCommandBufferLifetimeManager,
        };
        lifetimeManagerJob.Schedule();
        state.Dependency.Complete();
        entityCommandBufferLifetimeManager.Playback(state.EntityManager);
        entityCommandBufferLifetimeManager.Dispose();

        EntityCommandBuffer entityCommandBufferHealthManager = new EntityCommandBuffer(Allocator.TempJob);
        HealthManagerJob healthManagerJob = new HealthManagerJob
        {
            EntityCommandBuffer = entityCommandBufferHealthManager,
        };
        healthManagerJob.Schedule();
        state.Dependency.Complete();
        entityCommandBufferHealthManager.Playback(state.EntityManager);
        entityCommandBufferHealthManager.Dispose();

        PlayerDeathManager playerDeathManager = new PlayerDeathManager
        {
            DataSingleton = SystemAPI.GetSingleton<DataSingleton>()
        };
        playerDeathManager.Schedule();

        EnemyDeathManager enemyDeathManager = new EnemyDeathManager
        {
            DataSingleton = SystemAPI.GetSingleton<DataSingleton>()
        };
        enemyDeathManager.Schedule();
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

        private void Execute(Entity entity ,in Health health) 
        {
            if (health.CurrentHealth > 0.0f)
                return;

            if (health.DieOnDeath)
            {
                EntityCommandBuffer.DestroyEntity(entity);
                return;
            }
            //Do something object pooling idk;
                
        }
    }
    
    [BurstCompile, WithAll(typeof(Health),typeof(InputVariables))]
    public partial struct PlayerDeathManager : IJobEntity
    {
        public DataSingleton DataSingleton;
        
        public void Execute(in Health health)
        {
            if (health.CurrentHealth > 0.0f)
                return;
            Debug.Log("Player is Dead");
            DataSingleton.PlayerDead = true;
        }
    }
    [BurstCompile, WithAll(typeof(Health),typeof(Enemy))]
    public partial struct EnemyDeathManager : IJobEntity
    {
        public DataSingleton DataSingleton;

        public void Execute(in Health health)
        {
            if (health.CurrentHealth > 0.0f)
                return;
            Debug.Log("Enemy is Dead");
            DataSingleton.KillsCounter++;
        }
    }
}
