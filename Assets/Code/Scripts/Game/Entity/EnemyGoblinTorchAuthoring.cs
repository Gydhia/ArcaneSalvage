using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemyGoblinTorchAuthoring : MonoBehaviour
{
    
    private class Baker : Baker<ShootingSpinningAuthoring>
    {
        public override void Bake(ShootingSpinningAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyGoblinTorchData
            {

            });
        }
    }
}

public struct EnemyGoblinTorchData : IComponentData
{

}
