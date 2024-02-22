using Assets.Code.Scripts.Game.Player;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(MovementSystem))]
public partial struct PlayerTargetSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerTarget>();
    }

    public void OnUpdate(ref SystemState state)
    {
        NativeArray<PlayerTarget> nativeArray = new NativeArray<PlayerTarget>(1, Allocator.TempJob);

        nativeArray[0] = SystemAPI.GetSingleton<PlayerTarget>();

        TargetJob targetJob = new TargetJob
        {
            inputComponent = SystemAPI.GetSingleton<InputComponent>(),
            target = nativeArray,
        };
        state.Dependency = targetJob.Schedule(state.Dependency);
        state.Dependency.Complete();
        SystemAPI.SetSingleton(nativeArray[0]);
        nativeArray.Dispose();

    }

    [BurstCompile, WithNone(typeof(Player)), WithAll(typeof(Enemy))]
    public partial struct TargetJob : IJobEntity
    {
        public InputComponent inputComponent;
        public NativeArray<PlayerTarget> target;
        public void Execute(Entity entity, in LocalTransform localTransform)
        { 
            PlayerTarget playerTargetCpy = target[0];
            if (entity == target[0].enemy)
            {
                playerTargetCpy.enemyPosition = localTransform.Position;
                playerTargetCpy.DistanceToClosestEnemy = Vector3.Distance((Vector3)inputComponent.PlayerPosition, (Vector3)localTransform.Position);
            }
            float newDistance = Vector3.Distance((Vector3)inputComponent.PlayerPosition, (Vector3)localTransform.Position);
            if (newDistance <= target[0].DistanceToClosestEnemy)
            {
                playerTargetCpy.DistanceToClosestEnemy = newDistance;
                playerTargetCpy.enemyPosition = localTransform.Position;
                playerTargetCpy.enemy = entity;
            }
            target[0] = playerTargetCpy;
        }
    }
}
