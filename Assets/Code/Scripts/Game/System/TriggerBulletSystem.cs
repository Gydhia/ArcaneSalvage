using System.Collections;
using System.Collections.Generic;
using Unity.Assertions;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
[UpdateAfter(typeof(PhysicsSystemGroup))]
public partial struct TriggerBulletSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SimulationSingleton>();
    }

    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new BulletTriggerJob
        {
            BulletGroup = SystemAPI.GetComponentLookup<Bullet>(),
            PhysicsVelocityGroup = SystemAPI.GetComponentLookup<PhysicsVelocity>(),
            HealthGroup = SystemAPI.GetComponentLookup<Health>(),
        }
        .Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
    }

    public partial struct BulletTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<Bullet> BulletGroup;
        [ReadOnly] public ComponentLookup<PhysicsVelocity> PhysicsVelocityGroup;
        public ComponentLookup<Health> HealthGroup;
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            bool isBodyATrigger = BulletGroup.HasComponent(entityA);
            bool isBodyBTrigger = BulletGroup.HasComponent(entityB);

            //Ignoring Bullet overlapping other Bullets
            if (isBodyATrigger && isBodyBTrigger)
            {
                Debug.Log("Bullet overlapping");
                return;
            }
                

            bool isBodyADynamic = PhysicsVelocityGroup.HasComponent(entityA);
            bool isBodyBDynamic = PhysicsVelocityGroup.HasComponent(entityB);

            //Ignoring overlapping static bodies
            if ((isBodyATrigger && !isBodyBDynamic) ||
                (isBodyBTrigger && !isBodyADynamic))
            {
                Debug.Log("Overlapping static bodies");
                return;
            }
                

            var triggerEntity = isBodyADynamic ? entityA : entityB;
            var dynamicEntity = isBodyADynamic ? entityB : entityA;

            //Reduce Health Of Hit Body
            Debug.Log("Survived! We are going to lower health");
            var dynamicHealthComponent = HealthGroup[dynamicEntity];
            dynamicHealthComponent.CurrentHealth -= BulletGroup[triggerEntity].Damage;
            HealthGroup[dynamicEntity] = dynamicHealthComponent;
            
            //Reduce Health Of Bullet
            var triggerHealthComponent = HealthGroup[triggerEntity];
            triggerHealthComponent.CurrentHealth -= BulletGroup[dynamicEntity].Damage;
            HealthGroup[triggerEntity] = triggerHealthComponent;
        }
    }
}
