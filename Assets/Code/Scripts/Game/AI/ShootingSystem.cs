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
                if (shootData.NumberOfShoot % 2 == 0)
                    ShootEven(localTransform, shootData, movementDirection);    
                else 
                    ShootOdd(localTransform, shootData, movementDirection);
                //for (int i = 0; i < shootData.NumberOfShoot; i++)
                //{
                //}
                //
                //Entity spawnedEntity = EntityCommandBuffer.Instantiate(shootData.ProjectilePrefabEntity);
                //EntityCommandBuffer.SetComponent(spawnedEntity, new LocalTransform
                //{
                //    Position = localTransform.Position,
                //    Scale = 0.2f,
                //    Rotation = Quaternion.identity,
                //});
                //    LocalTransform.FromPosition(localTransform.Position); ;
                //EntityCommandBuffer.SetComponent(spawnedEntity, new Moving
                //{
                //    MoveSpeedValue = shootData.BulletMoveSpeed,
                //    Direction = movementDirection,
                //});

                CooldownManager.Start(shootData.CooldownID, shootData.FireRate, Time);
            }
        }

        private void ShootEven(in LocalTransform localTransform, in Shooting shootData, in float3 originMovementDirection)
        {
            NativeArray<Entity> entities = new NativeArray<Entity>(shootData.NumberOfShoot, Allocator.Temp);
            EntityCommandBuffer.Instantiate(shootData.ProjectilePrefabEntity, entities);
            for (int i = 1; i <= shootData.NumberOfShoot; i++)
            {
                EntityCommandBuffer.SetComponent(entities[i - 1], new LocalTransform
                {
                    Position = localTransform.Position,
                    Scale = 0.2f,
                    Rotation = Quaternion.identity,
                });
                Debug.Log($"Spawn bullet");
                Vector3 vectorMovementDirection = originMovementDirection;
                EntityCommandBuffer.SetComponent(entities[i - 1], new Moving
                {
                    MoveSpeedValue = shootData.BulletMoveSpeed,
                    Direction = Quaternion.AngleAxis(i % 2 == 0 ?
                        shootData.AngleDifference * (i / 2) :
                        -shootData.AngleDifference * ((i + 1) / 2),
                        new Vector3(localTransform.Forward().x, localTransform.Forward().y, localTransform.Forward().z)) * 
                        vectorMovementDirection,
                });
            }
        }

        private void ShootOdd(in LocalTransform localTransform, in Shooting shootData, in float3 originMovementDirection)
        {
            NativeArray<Entity> entities = new NativeArray<Entity>(shootData.NumberOfShoot, Allocator.Temp);
            EntityCommandBuffer.Instantiate(shootData.ProjectilePrefabEntity, entities);
            for (int i = 1; i <= shootData.NumberOfShoot; i++)
            {
                EntityCommandBuffer.SetComponent(entities[i - 1], new LocalTransform
                {
                    Position = localTransform.Position,
                    Scale = 0.2f,
                    Rotation = Quaternion.identity,
                });
                if (i == 1)
                {
                    EntityCommandBuffer.SetComponent(entities[i - 1], new Moving
                    {
                        MoveSpeedValue = shootData.BulletMoveSpeed,
                        Direction = originMovementDirection
                    });
                }
                else
                {
                    Vector3 vectorMovementDirection = originMovementDirection;
                    int j = i - 1;
                    EntityCommandBuffer.SetComponent(entities[i - 1], new Moving
                    {
                        MoveSpeedValue = shootData.BulletMoveSpeed,
                        Direction = Quaternion.AngleAxis(j % 2 == 0 ?
                        shootData.AngleDifference * (j / 2) :
                        -shootData.AngleDifference * ((j + 1) / 2),
                        new Vector3(localTransform.Forward().x, localTransform.Forward().y, localTransform.Forward().z)) *
                        vectorMovementDirection,
                    });
                }
            }
        }
    }
}
