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

[BurstCompile]
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
            inputComponent = SystemAPI.GetSingleton<DataSingleton>(),
            target = nativeArray,
        };
        state.Dependency = targetJob.Schedule(state.Dependency);
        state.Dependency.Complete();
        SystemAPI.SetSingleton(nativeArray[0]);
        Debug.Log("Target : " + nativeArray[0].enemy + " | Pos = " + nativeArray[0].DistanceToClosestEnemy);

        nativeArray.Dispose();
    }

    [BurstCompile, WithNone(typeof(Player)), WithAll(typeof(Enemy))]
    public partial struct TargetJob : IJobEntity
    {
        public DataSingleton inputComponent;
        public NativeArray<PlayerTarget> target;
        public void Execute(Entity entity, in LocalTransform localTransform)
        { 
            PlayerTarget playerTargetCpy = target[0];
            
            float currentTargetPos = Vector3.Distance((Vector3)inputComponent.PlayerPosition, (Vector3)target[0].enemyPosition);
            float newTargetPos = Vector3.Distance((Vector3)inputComponent.PlayerPosition, (Vector3)localTransform.Position);

            if (newTargetPos < currentTargetPos)
            {
                playerTargetCpy.DistanceToClosestEnemy = newTargetPos;
                playerTargetCpy.enemyPosition = localTransform.Position;
                playerTargetCpy.enemy = entity;
            }
            else
            {
                playerTargetCpy.DistanceToClosestEnemy = currentTargetPos;
            }

            target[0] = playerTargetCpy;
        }
        
        // PlayerTarget playerTargetCpy = target[0];
        // if (entity == target[0].enemy)
        // {
        //     playerTargetCpy.enemyPosition = localTransform.Position;
        //     playerTargetCpy.DistanceToClosestEnemy = Vector3.Distance((Vector3)inputComponent.PlayerPosition, (Vector3)localTransform.Position);
        //     target[0] = playerTargetCpy;
        // }
        // float newDistance = Vector3.Distance((Vector3)inputComponent.PlayerPosition, (Vector3)localTransform.Position);
        //     if (newDistance <= target[0].DistanceToClosestEnemy)
        // {
        //     playerTargetCpy.DistanceToClosestEnemy = newDistance;
        //     playerTargetCpy.enemyPosition = localTransform.Position;
        //     playerTargetCpy.enemy = entity;
        // }
        // target[0] = playerTargetCpy;
    }
}
