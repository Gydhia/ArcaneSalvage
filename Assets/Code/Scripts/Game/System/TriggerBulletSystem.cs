using System.Collections;
using System.Collections.Generic;
using Assets.Code.Scripts.Game.Player;
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
            PlayerGroup = SystemAPI.GetComponentLookup<InputVariables>(),
            HealthGroup = SystemAPI.GetComponentLookup<Health>(),
            EnemyGroup = SystemAPI.GetComponentLookup<Enemy>(),
        }
        .Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);


    }

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

            bool isBodyATrigger = BulletGroup.HasComponent(entityA);
            bool isBodyBTrigger = BulletGroup.HasComponent(entityB);

            //Ignoring Bullet overlapping other Bullets
            if (isBodyATrigger && isBodyBTrigger)
            {
                Debug.Log("Bullet overlapping");
                return;
            }

            (bool, Entity) playerCheck = FindEntityWithComponent(entityA,entityB, PlayerGroup);
            (bool, Entity) bulletCheck = FindEntityWithComponent(entityA,entityB, BulletGroup);
            (bool, Entity) enemyCheck = FindEntityWithComponent(entityA,entityB, EnemyGroup);
            
            if (playerCheck.Item1 && bulletCheck.Item1 && !enemyCheck.Item1)
            {
                DamageEntity(playerCheck.Item2, bulletCheck.Item2);
            }

            if (!playerCheck.Item1 && bulletCheck.Item1 && enemyCheck.Item1)
            {
                DamageEntity(enemyCheck.Item2, bulletCheck.Item2);
            }
        }

        private (bool,Entity) FindEntityWithComponent<T>(Entity entityA, Entity entityB, ComponentLookup<T> group) where T : unmanaged, IComponentData
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

        private void DamageEntity(Entity character, Entity bullet)
        {
            //Reduce Health Of Hit Body
            Debug.Log("Survived! We are going to lower health");

            var playerHealthComponent = HealthGroup[character];
            playerHealthComponent.CurrentHealth -= BulletGroup[bullet].Damage;
            HealthGroup[character] = playerHealthComponent;

            //Reduce Health Of Bullet
            var bulletHealthComponent = HealthGroup[bullet];
            bulletHealthComponent.CurrentHealth --;
            HealthGroup[bullet] = bulletHealthComponent;
        }
    }
}
