using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public float Lifetime;
    
    private class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Bullet());
        }
    }
}

public struct Bullet : IComponentData
{
}
