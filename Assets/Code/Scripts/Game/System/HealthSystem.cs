using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Code.Scripts.Game.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Events;

public partial struct HealthSystem : ISystem
{
    public static UnityEvent PlayerDeathEvent = new UnityEvent();
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Health>();
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


        EntityCommandBuffer entityCommandBufferHealthManager = new EntityCommandBuffer(Allocator.TempJob);
        HealthManagerJob healthManagerJob = new HealthManagerJob
        {
            EntityCommandBuffer = entityCommandBufferHealthManager,
        };
        healthManagerJob.Schedule();
        

        PlayerDeathManager playerDeathManager = new PlayerDeathManager();
        playerDeathManager.Schedule();

        state.Dependency.Complete();

        entityCommandBufferLifetimeManager.Playback(state.EntityManager);
        entityCommandBufferLifetimeManager.Dispose();
        entityCommandBufferHealthManager.Playback(state.EntityManager);
        entityCommandBufferHealthManager.Dispose();
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
        public void Execute(in Health health)
        {
            if (health.CurrentHealth > 0.0f)
                return;
            Debug.Log("Dead");
            PlayerDeathEvent.Invoke();
        }
    }
    
}
