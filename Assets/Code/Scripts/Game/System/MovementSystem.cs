using Assets.Code.Scripts.Game.Player;
using ProjectDawn.Navigation;
using ProjectDawn.Navigation.Hybrid;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Serialization;

public partial struct MovementSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DataSingleton>();
        state.RequireForUpdate<Moving>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        SetDestinationJob setDestinationJob = new SetDestinationJob
        {
            inputComponent = SystemAPI.GetSingleton<DataSingleton>(),
        };
        setDestinationJob.Schedule();
        
        
        MovingBulletJob movingBulletJob = new MovingBulletJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
        };
        movingBulletJob.Schedule();
        
        MovingPlayerJob movingPlayerJob = new MovingPlayerJob
        {
            dataSingleton = SystemAPI.GetSingleton<DataSingleton>()
        };
        movingPlayerJob.Schedule();
        
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

    [BurstCompile, WithAll(typeof(Moving), typeof(AgentBody), typeof(InputVariables))]
    public partial struct MovingPlayerJob : IJobEntity
    {
        public DataSingleton dataSingleton;
        public void Execute(ref AgentBody agentBody, ref Moving moveData)
        {
            if (dataSingleton.CanMove)
            {
                Vector2 moveDir = dataSingleton.MoveDirection;
                Vector3 direction = moveDir * moveData.MoveSpeedValue;
                Vector3 playerDestination= dataSingleton.PlayerPosition + (float3)direction;
                agentBody.SetDestination(playerDestination);
                moveData.Direction = agentBody.Velocity;
            }
            else
            {
                agentBody.IsStopped = true;
            }
        }
    }
    
    [BurstCompile, WithAll(typeof(AgentBody), typeof(Moving), typeof(Enemy))]
    public partial struct SetDestinationJob : IJobEntity
    {
        public DataSingleton inputComponent;

        public void Execute(ref AgentBody agentBody, ref Moving moveData)
        {
            agentBody.SetDestination(inputComponent.PlayerPosition);
            moveData.Direction = agentBody.Velocity;
        }
    }
}
