using System.Collections;
using System.Collections.Generic;
using Assets.Code.Scripts.Game.Player;
using Unity.Assertions;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
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
            PlayerGroup = SystemAPI.GetComponentLookup<InputVariables>(),
            HealthGroup = SystemAPI.GetComponentLookup<Health>(),
            EnemyGroup = SystemAPI.GetComponentLookup<Enemy>(),
        }
        .Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);


    }

    [BurstCompile]
    public partial struct BulletTriggerJob : ITriggerEventsJob
    {
        [ReadOnly] public ComponentLookup<Bullet> BulletGroup;
        [ReadOnly] public ComponentLookup<InputVariables> PlayerGroup;
        [ReadOnly] public ComponentLookup<Enemy> EnemyGroup;
        public ComponentLookup<Health> HealthGroup;
        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;

            (bool, Entity) playerCheck = FindEntityWithComponent(entityA,entityB, PlayerGroup);
            (bool, Entity) bulletCheck = FindEntityWithComponent(entityA,entityB, BulletGroup);
            (bool, Entity) enemyCheck = FindEntityWithComponent(entityA,entityB, EnemyGroup);
            
            if (playerCheck.Item1 && bulletCheck.Item1 && BulletGroup[bulletCheck.Item2].OwnerType != OwnerType.Player && !enemyCheck.Item1)
            {
                DamageEntity(playerCheck.Item2, bulletCheck.Item2);
                return;
            }

            if (!playerCheck.Item1 && bulletCheck.Item1 && BulletGroup[bulletCheck.Item2].OwnerType != OwnerType.Enemy && enemyCheck.Item1)
            {
                DamageEntity(enemyCheck.Item2, bulletCheck.Item2);
            }
        }

        
        private void DamageEntity(Entity character, Entity bullet)
        {
            //Reduce Health Of Hit Body
            var characterHealthComponent = HealthGroup[character];
            characterHealthComponent.CurrentHealth -= BulletGroup[bullet].Damage;
            HealthGroup[character] = characterHealthComponent;

            //Reduce Health Of Bullet
            var bulletHealthComponent = HealthGroup[bullet];
            bulletHealthComponent.CurrentHealth --;
            HealthGroup[bullet] = bulletHealthComponent;
        }
    }
    public static (bool,Entity) FindEntityWithComponent<T>(Entity entityA, Entity entityB, ComponentLookup<T> group) where T : unmanaged, IComponentData
    {
        if (group.HasComponent(entityA))
        {
            return (true, entityA);
        }
        if (group.HasComponent(entityB))
        {
            return (true, entityB);
        }
        return (false, new Entity());
    }
}
