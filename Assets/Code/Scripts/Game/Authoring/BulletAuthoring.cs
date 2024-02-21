using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public float Damage;
    private void Start()
    {
        Debug.Log("MonoBehaviour Bullet start");
    }
    private class Baker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Bullet
            {
                Damage = authoring.Damage,
            });
        }
    }
}

public struct Bullet : IComponentData
{
    public float Damage;
}
