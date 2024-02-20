using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct MovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        MovingEnemyJob movingEnemyJob = new MovingEnemyJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
        };
        movingEnemyJob.Schedule();

        MovingBulletJob movingBulletJob = new MovingBulletJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
        };
        movingBulletJob.Schedule();
    }

    [BurstCompile, WithAll(typeof(Moving), typeof(Enemy))]
    public partial struct MovingEnemyJob : IJobEntity
    {
        public float DeltaTime;
        public Vector3 PlayerPosition;
        public void Execute(ref LocalTransform localTransform, in Moving movementData)
        {
            float x = PlayerPosition.x - localTransform.Position.x;
            float y = PlayerPosition.y - localTransform.Position.y;
            float3 Direction = math.normalizesafe(
                new float3(PlayerPosition.x - localTransform.Position.x,
                PlayerPosition.y - localTransform.Position.y, 0.0f));
            if (math.lengthsq(localTransform.Position - Direction) >= 0.0001f)
            {
                localTransform.Position += (Direction * movementData.MoveSpeedValue * DeltaTime);
            }
        }
    }

    [BurstCompile, WithAll(typeof(Moving), typeof(Bullet))]
    public partial struct MovingBulletJob : IJobEntity
    {
        public float DeltaTime;
        
        public void Execute(ref LocalTransform localTransform, in Moving movementData)
        {
            localTransform.Position += (movementData.Direction * movementData.MoveSpeedValue * DeltaTime);
        }
    }
}
