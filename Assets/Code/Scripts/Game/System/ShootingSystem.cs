using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class ShootingSystem : SystemBase
{
    [BurstCompile]
    protected override void OnUpdate()
    {
        Vector3 playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;

        EntityCommandBuffer entityCommandBufferStraightJob = new EntityCommandBuffer(Allocator.TempJob);

        ShootingStraightJob shootingObjectJob = new ShootingStraightJob
        {
            PlayerPosition = playerPosition,
            EntityCommandBuffer = entityCommandBufferStraightJob,
            Time = (float)SystemAPI.Time.ElapsedTime,
        };
        shootingObjectJob.Schedule();
        Dependency.Complete();
        entityCommandBufferStraightJob.Playback(EntityManager);
        entityCommandBufferStraightJob.Dispose();

        EntityCommandBuffer entityCommandBufferSpinningJob = new EntityCommandBuffer(Allocator.TempJob);

        ShootingSpinningJob shootingSpinningJob = new ShootingSpinningJob
        {
            EntityCommandBuffer = entityCommandBufferSpinningJob,
            Time = (float)SystemAPI.Time.ElapsedTime,
        };
        shootingSpinningJob.Schedule();
        Dependency.Complete();
        entityCommandBufferSpinningJob.Playback(EntityManager);
        entityCommandBufferSpinningJob.Dispose();
    }

    [BurstCompile, WithAll(typeof(ShootingStraight))]
    public partial struct ShootingStraightJob : IJobEntity
    {
        public Vector3 PlayerPosition;
        public float Time;
        public EntityCommandBuffer EntityCommandBuffer;
        public void Execute(in LocalTransform localTransform, in ShootingStraight shootData)
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

                CooldownManager.Start(shootData.CooldownID, shootData.FireRate, Time);
            }
        }
        private void ShootEven(in LocalTransform localTransform, in ShootingStraight shootData, in float3 originMovementDirection)
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

        private void ShootOdd(in LocalTransform localTransform, in ShootingStraight shootData, in float3 originMovementDirection)
        {
            NativeArray<Entity> entities = new NativeArray<Entity>(shootData.NumberOfShoot, Allocator.Temp);
            EntityCommandBuffer.Instantiate(shootData.ProjectilePrefabEntity, entities);
            for (int i = 1; i <= shootData.NumberOfShoot; i++)
            {
                EntityCommandBuffer.SetComponent(entities[i - 1], new LocalTransform
                {
                    Position = localTransform.Position,
                    Scale = 1f,
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
    [BurstCompile, WithAll(typeof(ShootingSpinning))]
    public partial struct ShootingSpinningJob : IJobEntity
    {
        public float Time;
        public EntityCommandBuffer EntityCommandBuffer;

        public void Execute(in LocalTransform localTransform, ref ShootingSpinning shootData)
        {
            if (CooldownManager.IsDone(shootData.CooldownID, Time))
            {
                Entity entity = EntityCommandBuffer.Instantiate(shootData.ProjectilePrefabEntity);
                EntityCommandBuffer.AddComponent(entity, new LocalTransform
                {
                    Position = localTransform.Position,
                    Scale = 1f,
                    Rotation = Quaternion.identity
                });
                EntityCommandBuffer.AddComponent(entity, new Moving
                {
                    MoveSpeedValue = shootData.BulletMoveSpeed,
                    Direction = Quaternion.AngleAxis(
                        shootData.BaseAngle, new Vector3(
                            localTransform.Forward().x, localTransform.Forward().y, localTransform.Forward().z))
                    * new Vector3(localTransform.Right().x, localTransform.Right().y, localTransform.Right().z),
                });
                shootData.BaseAngle = (shootData.BaseAngle + shootData.AngleIncrease) % 360;
                CooldownManager.Start(shootData.CooldownID, shootData.FireRate, Time);
            }
        }
    }
}