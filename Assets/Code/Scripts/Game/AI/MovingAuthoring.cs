using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

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
                MoveSpeedValue = authoring.MoveSpeedValue
            });
        }
    }
}

public struct Moving : IComponentData
{
    public float MoveSpeedValue;
}

