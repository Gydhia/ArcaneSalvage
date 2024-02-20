using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct RotatingSquareSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<RotateSpeed>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        /*foreach ((RefRW<LocalTransform> localTransform, RefRO<RotateSpeed> rotateSpeed)
            in SystemAPI.Query<RefRW<LocalTransform>, RefRO<RotateSpeed>>())
        {
            localTransform.ValueRW = localTransform.ValueRO.RotateZ(rotateSpeed.ValueRO.value * SystemAPI.Time.DeltaTime);
        }*/

        RotatingSquareJob rotatingSquareJob = new RotatingSquareJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };

        rotatingSquareJob.Schedule();
    }

    [BurstCompile]
    public partial struct RotatingSquareJob : IJobEntity
    {
        public float deltaTime;

        // ref = RW / in = RO
        public void Execute(ref LocalTransform localTransform, in RotateSpeed rotateSpeed)
        {
            localTransform = localTransform.RotateZ(rotateSpeed.value * deltaTime);
        }
    }
}
