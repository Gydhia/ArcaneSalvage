using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct ShootingSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Shooting>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);

        ShootingObjectJob shootingObjectJob = new ShootingObjectJob
        {
            PlayerPosition = playerPosition,
            EntityCommandBuffer = entityCommandBuffer,
            Time = Time.time,
        };
        shootingObjectJob.Schedule();
        state.Dependency.Complete();
        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }

    [BurstCompile]
    public partial struct ShootingObjectJob: IJobEntity
    {
        public Vector3 PlayerPosition;
        public float Time;
        public EntityCommandBuffer EntityCommandBuffer;
        public void Execute(in LocalTransform localTransform, in Shooting shootData)
        {
            float x = PlayerPosition.x - localTransform.Position.x;
            float y = PlayerPosition.y - localTransform.Position.y;
            if (CooldownManager.IsDone(shootData.CooldownID, Time) &&
                Math.Sqrt(x * x + y * y) <= shootData.FireRange)
            {
                float3 movementDirection = math.normalizesafe(
                    new float3(PlayerPosition.x - localTransform.Position.x,
                    PlayerPosition.y - localTransform.Position.y, 0.0f));

                Entity spawnedEntity = EntityCommandBuffer.Instantiate(shootData.ProjectilePrefabEntity);
                EntityCommandBuffer.SetComponent(spawnedEntity, new LocalTransform
                {
                    Position = localTransform.Position,
                    Scale = 0.2f,
                    Rotation = Quaternion.identity,
                });
                    LocalTransform.FromPosition(localTransform.Position); ;
                EntityCommandBuffer.SetComponent(spawnedEntity, new Moving
                {
                    MoveSpeedValue = shootData.BulletMoveSpeed,
                    Direction = movementDirection,
                });

                CooldownManager.Start(shootData.CooldownID, shootData.FireRate, Time);
            }
        }
    }
}
