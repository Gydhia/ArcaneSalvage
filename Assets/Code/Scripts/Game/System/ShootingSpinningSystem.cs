using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial class ShootingSpinningSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate< ShootingSpinning>();
    }

    protected override void OnUpdate()
    {
        EntityCommandBuffer entityCommandBufferSpinningJob = new EntityCommandBuffer(Allocator.TempJob);

        EntityQuery query = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform>().WithAll<ShootingSpinning>().Build(this);

        ShootingSpinningJob shootingSpinningJob = new ShootingSpinningJob
        {
            EntityCommandBuffer = entityCommandBufferSpinningJob,
            DeltaTime = (float)SystemAPI.Time.DeltaTime,
        };
        shootingSpinningJob.Schedule(query);
        Dependency.Complete();
        entityCommandBufferSpinningJob.Playback(EntityManager);
        entityCommandBufferSpinningJob.Dispose();
        query.Dispose();
    }

    [BurstCompile, WithAll(typeof(ShootingSpinning))]
    public partial struct ShootingSpinningJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer EntityCommandBuffer;

        public void Execute(in LocalTransform localTransform, ref ShootingSpinning shootData)
        {
            shootData.FireRate -= DeltaTime;
            if (shootData.FireRate > 0.0f)
                return;
            Entity entity = EntityCommandBuffer.Instantiate(shootData.ProjectilePrefabEntity);
            EntityCommandBuffer.AddComponent(entity, new LocalTransform
            {
                Position = localTransform.Position,
                Scale = 1f,
                Rotation = Quaternion.AngleAxis((shootData.BaseAngle + shootData.AngleIncrease) % 360, Vector3.forward)
            });
            EntityCommandBuffer.AddComponent(entity, new Moving
            {
                MoveSpeedValue = shootData.BulletMoveSpeed,
                Direction = Quaternion.AngleAxis(
                    shootData.BaseAngle, (Vector3) localTransform.Forward())
                * (Vector3) localTransform.Right()
            });
            shootData.BaseAngle = (shootData.BaseAngle + shootData.AngleIncrease) % 360;
            shootData.FireRate = shootData.OriginalFireRate;

            EntityCommandBuffer.AddComponent(entity, new Bullet
            {
                Damage = shootData.BulletDamage,
                OwnerType = shootData.OwnerType,
            });
        }
    }
}
