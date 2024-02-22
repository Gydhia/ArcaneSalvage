using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ZombieAuthoring : MonoBehaviour
{
    public class BakerScript : Baker<PoolingAuthoring>
    {
        public override void Bake(PoolingAuthoring poolingManager)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new IZombieComponent()
            {
                key = 2
            });
        }
    }
}
