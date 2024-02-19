using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class MoveEnemySystem : SystemBase
{
    private Transform _playerTransform;
    public void OnCreate(ref SystemState state)
    {
        
        state.RequireForUpdate<Moving>();
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    [BurstCompile]
    protected override void OnUpdate()
    {
        //foreach ((RefRW<LocalTransform> localTransform, RefRO<Moving> movementData)
        //    in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Moving>>())
        //{
        //    float3 Direction = math.normalizesafe(
        //        new float3(_playerTransform.position.x - localTransform.ValueRO.Position.x,
        //        _playerTransform.position.y - localTransform.ValueRO.Position.y, 0.0f));
        //    if (math.lengthsq(localTransform.ValueRO.Position - Direction) >= 0.0001f)
        //    {
        //        localTransform.ValueRW.Position += (Direction * movementData.ValueRO.MoveSpeedValue * SystemAPI.Time.DeltaTime);
        //    }
        //}
        MovingObjectJob movingObjectJob = new MovingObjectJob
        {
            deltaTime = SystemAPI.Time.DeltaTime,
            playerPosition = _playerTransform.position,
        };
        movingObjectJob.ScheduleParallel();
    }

    [BurstCompile]
    public partial struct MovingObjectJob: IJobEntity
    {
        public float deltaTime;
        public Vector3 playerPosition;
        public void Execute(ref LocalTransform localTransform, in Moving movementData)
        {
            float3 Direction = math.normalizesafe(
                new float3(playerPosition.x - localTransform.Position.x,
                playerPosition.y - localTransform.Position.y, 0.0f));
            if (math.lengthsq(localTransform.Position - Direction) >= 0.0001f)
            {
                localTransform.Position += (Direction * movementData.MoveSpeedValue * deltaTime);
            }
        }
    }
}
