using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PlayerTargetAuthoring : MonoBehaviour
{
    private class Baker : Baker<PlayerTargetAuthoring> 
    {
        public override void Bake(PlayerTargetAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerTarget
            {
                DistanceToClosestEnemy = 10000000.0f
            });
        }
    }
}

public struct PlayerTarget : IComponentData
{
    public float DistanceToClosestEnemy;
    public Vector3 enemyPosition;
    public Entity enemy;

}
