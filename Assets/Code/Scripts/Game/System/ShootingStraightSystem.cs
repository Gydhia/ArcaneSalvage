using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Scripts.Game.Player;
using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class ShootingStraightSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<ShootingStraight>();
    }
    [BurstCompile]
    protected override void OnUpdate()
    {
        Vector3 playerPosition = SystemAPI.GetSingleton<DataSingleton>().PlayerPosition;

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
    }

    [BurstCompile]
    public partial struct ShootingStraightJob : IJobEntity
    {
        public Vector3 PlayerPosition;
        public float Time;
        public EntityCommandBuffer EntityCommandBuffer;
        public void Execute(in LocalTransform localTransform, in ShootingStraight shootData)
        {
            if (shootData.NumberOfShoot <=0)
                return;
            
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
                Vector3 direction = Quaternion.AngleAxis(
                                    i % 2 == 0
                                        ? shootData.AngleDifference * (i / 2)
                                        : -shootData.AngleDifference * ((i + 1) / 2),
                                    new Vector3(localTransform.Forward().x, localTransform.Forward().y,
                                        localTransform.Forward().z)) *
                                originMovementDirection;
                
                direction.Normalize();
                float angle = Vector2.SignedAngle(Vector2.right, direction);
                
                EntityCommandBuffer.SetComponent(entities[i - 1], new LocalTransform
                {
                    Position = localTransform.Position,
                    Scale = 1f,
                    Rotation = Quaternion.Euler(new Vector3(0, 0, angle))
                });
                EntityCommandBuffer.SetComponent(entities[i - 1], new Moving
                {
                    MoveSpeedValue = shootData.BulletMoveSpeed,
                    Direction = direction
                });
            }
        }

        private void ShootOdd(in LocalTransform localTransform, in ShootingStraight shootData, in float3 originMovementDirection)
        {
            NativeArray<Entity> entities = new NativeArray<Entity>(shootData.NumberOfShoot, Allocator.Temp);
            EntityCommandBuffer.Instantiate(shootData.ProjectilePrefabEntity, entities);
            for (int i = 1; i <= shootData.NumberOfShoot; i++)
            {
                int j = i - 1;
                Vector3 direction = Quaternion.AngleAxis(
                                        j % 2 == 0
                                            ? shootData.AngleDifference * (j / 2)
                                            : -shootData.AngleDifference * ((j + 1) / 2),
                                        new Vector3(localTransform.Forward().x, localTransform.Forward().y,
                                            localTransform.Forward().z)) *
                                    originMovementDirection;

                direction = (i == 1 ? originMovementDirection : direction );
                direction.Normalize();
                
                float angle = Vector2.SignedAngle(Vector2.right, direction);
                
                EntityCommandBuffer.SetComponent(entities[i - 1], new LocalTransform
                {
                    Position = localTransform.Position,
                    Scale = 1f,
                    Rotation = Quaternion.Euler(new Vector3(0, 0, angle))
                });
                EntityCommandBuffer.SetComponent(entities[i - 1], new Moving
                {
                    MoveSpeedValue = shootData.BulletMoveSpeed,
                    Direction = direction
                });
            }
        }
    }
}