using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.UniversalDelegates;
using Unity.Transforms;
using UnityEngine;

public partial struct ShootingCardinalSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ShootingCardinal>();
    }

    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBufferCardinalJob = new EntityCommandBuffer(Allocator.TempJob);

        ShootingCardinalJob shootingCardinalJob = new ShootingCardinalJob
        {
            Time = (float)SystemAPI.Time.ElapsedTime,
            EntityCommandBuffer = entityCommandBufferCardinalJob,
        };
        shootingCardinalJob.Schedule();
        state.Dependency.Complete();
        entityCommandBufferCardinalJob.Playback(state.EntityManager);
        entityCommandBufferCardinalJob.Dispose();


    }

    public partial struct ShootingCardinalJob : IJobEntity
    {
        public float Time;
        public EntityCommandBuffer EntityCommandBuffer;

        public void Execute(in LocalTransform localTransform, in ShootingCardinal shootData)
        {
            if (CooldownManager.IsDone(shootData.CooldownID, Time))
            {
                int numberOfBullet = shootData.ShootingDirection == ShootingDirection.BOTH ? 8 : 4;
                NativeArray<Entity> entities = 
                    new NativeArray<Entity>(numberOfBullet, Allocator.Temp);
                EntityCommandBuffer.Instantiate(shootData.ProjectilePrefabEntity, entities);
                for (int i = 0; i < numberOfBullet; i++)
                {
                    EntityCommandBuffer.AddComponent(entities[i], new LocalTransform
                    {
                        Position = localTransform.Position,
                        Scale = 1f,
                        Rotation = Quaternion.identity
                    });
                    switch (shootData.ShootingDirection)
                    {
                        case ShootingDirection.CARDINAL:
                        case ShootingDirection.BOTH:
                            EntityCommandBuffer.AddComponent(entities[i], new Moving
                            {
                                MoveSpeedValue = shootData.BulletMoveSpeed,
                                Direction = Quaternion.AngleAxis((360.0f / numberOfBullet) * i,
                                (Vector3)localTransform.Forward()) *
                                (Vector3)localTransform.Right()
                            });
                            break;
                        case ShootingDirection.INTERCARDINAL:
                            EntityCommandBuffer.AddComponent(entities[i], new Moving
                            {
                                MoveSpeedValue = shootData.BulletMoveSpeed,
                                Direction = Quaternion.AngleAxis(45.0f + ((360.0f / numberOfBullet) * i),
                                (Vector3)localTransform.Forward()) *
                                (Vector3)localTransform.Right()
                            });
                            break;
                        default:
                            break;
                    }
                }
                CooldownManager.Start(shootData.CooldownID, shootData.FireRate, Time);
            }
        }
    }
}