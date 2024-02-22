using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using static ShootingStraightSystem;

public partial class ShootingSpinningSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate< ShootingSpinning>();
    }

    protected override void OnUpdate()
    {
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

    [BurstCompile]
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
                    Rotation = Quaternion.AngleAxis((shootData.BaseAngle + shootData.AngleIncrease) % 360, Vector3.forward)
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
