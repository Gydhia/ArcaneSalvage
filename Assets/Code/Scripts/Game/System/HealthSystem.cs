using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct HealthSystem : ISystem
{
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
    }

    public partial struct LifetimeManagerJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer EntityCommandBuffer;
        public void Execute(Entity entity, ref Health health)
        {
            if (health.Lifetime <= -1.0f)
                return;

            health.Lifetime -= DeltaTime;

            if (health.Lifetime > 0.0f)
                return;
            
            EntityCommandBuffer.DestroyEntity(entity);
        }
    }

    public partial struct HealthManagerJob : IJobEntity
    {
        public EntityCommandBuffer EntityCommandBuffer;
        public void Execute(Entity entity ,in Health health) 
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
}
