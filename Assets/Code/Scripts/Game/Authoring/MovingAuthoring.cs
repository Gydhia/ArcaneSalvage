using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class MovingAuthoring : MonoBehaviour
{
    public float MoveSpeedValue;
    private class Baker : Baker<MovingAuthoring>
    {
        public override void Bake(MovingAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Moving
            {
                MoveSpeedValue = authoring.MoveSpeedValue,
            });
        }
    }
}

public struct Moving : IComponentData
{
    public float MoveSpeedValue;
    public float3 Direction;
}

