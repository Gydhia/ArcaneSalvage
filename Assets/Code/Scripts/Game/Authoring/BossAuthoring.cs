using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BossAuthoring : MonoBehaviour
{
    private class Baker : Baker<BossAuthoring>
    {
        public override void Bake(BossAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Boss());
        }
    }
}
public struct Boss : IComponentData
{

}
