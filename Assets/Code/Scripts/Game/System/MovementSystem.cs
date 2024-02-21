using Assets.Code.Scripts.Game.Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Transforms;
using UnityEngine;

public partial struct MovementSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<InputComponent>();
        state.RequireForUpdate<Moving>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        MovingEnemyJob movingEnemyJob = new MovingEnemyJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            PlayerPosition = SystemAPI.GetSingleton<InputComponent>().PlayerPosition,
        };
        movingEnemyJob.Schedule();

        MovingBulletJob movingBulletJob = new MovingBulletJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
        };
        movingBulletJob.Schedule();
        
        MovingPlayerJob movingPlayerJob = new MovingPlayerJob
        {

            InputComponent = SystemAPI.GetSingleton<InputComponent>()
            
        };

        movingPlayerJob.Schedule();

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
            localTransform.Position += (Direction * movementData.MoveSpeedValue * DeltaTime);
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

    [WithAll(typeof(Moving), typeof(InputVariables))]
    public partial struct MovingPlayerJob : IJobEntity
    {
        public InputComponent InputComponent; 
        public void Execute(ref PhysicsVelocity physicsVelocity, in Moving moveData)
        {
            if (InputComponent.CanMove)
            {
                Vector2 moveDir = InputComponent.MoveDirection;
                Vector2 direction = moveDir * moveData.MoveSpeedValue;
                physicsVelocity.Linear = new float3(direction.x, direction.y, 0);
            }
            else
            {
                physicsVelocity.Linear = float3.zero;
            }
        }
    }
}
