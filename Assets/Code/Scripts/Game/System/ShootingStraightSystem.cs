using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Code.Scripts.Game.Player;
using Code.Scripts.Game.Authoring;
using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using ProjectDawn.Navigation;

[BurstCompile]
[UpdateAfter(typeof(PlayerTargetSystem))]
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

        EntityCommandBuffer entityCommandBufferEnemyJob = new EntityCommandBuffer(Allocator.TempJob);

        ShootingStraightEnemyJob shootingEnemyJob = new ShootingStraightEnemyJob
        {
            PlayerPosition = playerPosition,
            EntityCommandBuffer = entityCommandBufferEnemyJob,
            DeltaTime = (float)SystemAPI.Time.DeltaTime,
        };
        shootingEnemyJob.Schedule();

        EntityCommandBuffer entityCommandBufferStraightPlayerJob = new EntityCommandBuffer(Allocator.TempJob);

        ShootingStraightPlayerJob shootingPlayerJob = new ShootingStraightPlayerJob
        {
            EntityCommandBuffer = entityCommandBufferStraightPlayerJob,
            DeltaTime = (float)SystemAPI.Time.DeltaTime,
            entityExist = EntityManager.Exists(SystemAPI.GetSingleton<PlayerTarget>().enemy),
        };
        shootingPlayerJob.Schedule();

        EntityCommandBuffer entityCommandBufferPlayerJobStraight = new EntityCommandBuffer(Allocator.TempJob);

        ShootingStraightForReal shootingstraightForReal = new ShootingStraightForReal
        {
            EntityCommandBuffer = entityCommandBufferPlayerJobStraight,
            DeltaTime = (float)SystemAPI.Time.DeltaTime,
        };
        shootingstraightForReal.Schedule();
        Dependency.Complete();

        entityCommandBufferStraightPlayerJob.Playback(EntityManager);
        entityCommandBufferStraightPlayerJob.Dispose();
        entityCommandBufferEnemyJob.Playback(EntityManager);
        entityCommandBufferEnemyJob.Dispose();
        entityCommandBufferPlayerJobStraight.Playback(EntityManager);
        entityCommandBufferPlayerJobStraight.Dispose();
    }

    [BurstCompile, WithAll(typeof(Enemy))]
    public partial struct ShootingStraightEnemyJob : IJobEntity
    {
        public Vector3 PlayerPosition;
        public float DeltaTime;
        public EntityCommandBuffer EntityCommandBuffer;
        public void Execute(in LocalTransform localTransform, ref ShootingStraight shootData)
        {
            shootData.FireRate -= DeltaTime;
            if (shootData.NumberOfShoot <= 0 || shootData.FireRate >= 0.0f)
                return;

            float x = PlayerPosition.x - localTransform.Position.x;
            float y = PlayerPosition.y - localTransform.Position.y;
            if (Math.Sqrt(x * x + y * y) <= shootData.FireRange)
            {
                float3 movementDirection = math.normalizesafe(new float3(x, y, 0.0f));
                if (shootData.NumberOfShoot % 2 == 0)
                    ShootEven(localTransform, shootData, movementDirection);
                else
                    ShootOdd(localTransform, shootData, movementDirection);

                shootData.FireRate = shootData.OriginalFireRate;
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
                EntityCommandBuffer.AddComponent(entities[i - 1], new Bullet
                {
                    Damage = shootData.BulletDamage,
                    OwnerType = shootData.OwnerType,
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

                direction = (i == 1 ? originMovementDirection : direction);
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
                EntityCommandBuffer.AddComponent(entities[i - 1], new Bullet
                {
                    Damage = shootData.BulletDamage,
                    OwnerType = shootData.OwnerType,
                });
            }
        }
    }

    [BurstCompile, WithNone(typeof(Enemy), typeof(BoatPlayer)), WithAll(typeof(ShootingStraight), typeof(PlayerTarget))]
    public partial struct ShootingStraightPlayerJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer EntityCommandBuffer;
        public bool entityExist;

        public void Execute(in LocalTransform localTransform, ref ShootingStraight shootData, in PlayerTarget playerTarget)
        {
            shootData.FireRate -= DeltaTime;
            if (shootData.NumberOfShoot <= 0 || shootData.FireRate > 0.0f || !entityExist)
                return;
            float x = playerTarget.enemyPosition.x - localTransform.Position.x;
            float y = playerTarget.enemyPosition.y - localTransform.Position.y;
            if (Math.Sqrt(x * x + y * y) <= shootData.FireRange)
            {
                float3 movementDirection = math.normalizesafe(new float3(x, y, 0.0f));
                if (shootData.NumberOfShoot % 2 == 0)
                    ShootEven(localTransform, shootData, movementDirection);
                else
                    ShootOdd(localTransform, shootData, movementDirection);

                shootData.FireRate = shootData.OriginalFireRate;
            }
        }
        private void ShootEven(in LocalTransform localTransform, in ShootingStraight shootData, in float3 originMovementDirection)
        {
            NativeArray<Entity> entities = new NativeArray<Entity>(shootData.NumberOfShoot, Allocator.Temp);
            EntityCommandBuffer.Instantiate(shootData.ProjectilePrefabEntity, entities);
            for (int i = 1; i <= shootData.NumberOfShoot; i++)
            {
                Vector3 direction = Quaternion.AngleAxis(i % 2 == 0
                                        ? shootData.AngleDifference * (i / 2)
                                        : -shootData.AngleDifference * ((i + 1) / 2),
                                        (Vector3)localTransform.Forward())
                                * originMovementDirection;

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
                EntityCommandBuffer.AddComponent(entities[i - 1], new Bullet
                {
                    Damage = shootData.BulletDamage,
                    OwnerType = shootData.OwnerType,
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
                Vector3 direction = Quaternion.AngleAxis(j % 2 == 0
                                            ? shootData.AngleDifference * (j / 2)
                                            : -shootData.AngleDifference * ((j + 1) / 2),
                                            (Vector3)localTransform.Forward())
                                    * originMovementDirection;

                direction = (i == 1 ? originMovementDirection : direction);
                direction.Normalize();

                float angle = Vector2.SignedAngle(Vector2.right, direction);

                EntityCommandBuffer.SetComponent(entities[j], new LocalTransform
                {
                    Position = localTransform.Position,
                    Scale = 1f,
                    Rotation = Quaternion.Euler(new Vector3(0, 0, angle))
                });
                EntityCommandBuffer.SetComponent(entities[j], new Moving
                {
                    MoveSpeedValue = shootData.BulletMoveSpeed,
                    Direction = direction
                });
                EntityCommandBuffer.AddComponent(entities[j], new Bullet
                {
                    Damage = shootData.BulletDamage,
                    OwnerType = shootData.OwnerType,
                });
            }
        }
    }

    [BurstCompile, WithAll(typeof(ShootingStraight), typeof(BoatPlayer)), WithNone(typeof(Enemy))]
    public partial struct ShootingStraightForReal : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer EntityCommandBuffer;
        public void Execute(in LocalTransform localTransform, ref ShootingStraight shootData)
        {
            shootData.FireRate -= DeltaTime;
            if (shootData.NumberOfShoot <= 0 || shootData.FireRate > 0.0f)
                return;

            float3 movementDirection = localTransform.Up();
            if (shootData.NumberOfShoot % 2 == 0)
                ShootEven(localTransform, shootData, movementDirection);
            else
                ShootOdd(localTransform, shootData, movementDirection);

            shootData.FireRate = shootData.OriginalFireRate;
        }

        private void ShootEven(in LocalTransform localTransform, in ShootingStraight shootData, in float3 originMovementDirection)
        {
            NativeArray<Entity> entities = new NativeArray<Entity>(shootData.NumberOfShoot, Allocator.Temp);
            EntityCommandBuffer.Instantiate(shootData.ProjectilePrefabEntity, entities);
            for (int i = 1; i <= shootData.NumberOfShoot; i++)
            {
                Vector3 direction = Quaternion.AngleAxis(i % 2 == 0
                                        ? shootData.AngleDifference * (i / 2)
                                        : -shootData.AngleDifference * ((i + 1) / 2),
                                        (Vector3)localTransform.Forward())
                                * originMovementDirection;

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
                EntityCommandBuffer.AddComponent(entities[i - 1], new Bullet
                {
                    Damage = shootData.BulletDamage,
                    OwnerType = shootData.OwnerType,
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
                Vector3 direction = Quaternion.AngleAxis(j % 2 == 0
                                            ? shootData.AngleDifference * (j / 2)
                                            : -shootData.AngleDifference * ((j + 1) / 2),
                                            (Vector3)localTransform.Forward())
                                    * originMovementDirection;

                direction = (i == 1 ? originMovementDirection : direction);
                direction.Normalize();

                float angle = Vector2.SignedAngle(Vector2.right, direction);

                EntityCommandBuffer.SetComponent(entities[j], new LocalTransform
                {
                    Position = localTransform.Position,
                    Scale = 1f,
                    Rotation = Quaternion.Euler(new Vector3(0, 0, angle))
                });
                EntityCommandBuffer.SetComponent(entities[j], new Moving
                {
                    MoveSpeedValue = shootData.BulletMoveSpeed,
                    Direction = direction
                });
                EntityCommandBuffer.AddComponent(entities[j], new Bullet
                {
                    Damage = shootData.BulletDamage,
                    OwnerType = shootData.OwnerType,
                });
            }
        }
    }
}