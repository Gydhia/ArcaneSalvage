using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public enum ShootingDirection
{
    CARDINAL,
    INTERCARDINAL,
    BOTH,
};
public class ShootCardinalAuthoring : MonoBehaviour
{
    public float FireRate;
    public GameObject ProjectilePrefab;
    public float BulletMoveSpeed;
    public ShootingDirection Direction;

    private class Baker : Baker<ShootCardinalAuthoring> 
    {
        public override void Bake(ShootCardinalAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ShootingCardinal
            {
                FireRate = authoring.FireRate,
                OriginalFireRate = authoring.FireRate,
                ProjectilePrefabEntity = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.None),
                BulletMoveSpeed = authoring.BulletMoveSpeed,
                ShootingDirection = authoring.Direction,
            });
        }
    }
}

public struct ShootingCardinal : IComponentData
{
    public float FireRate;
    public float OriginalFireRate;
    public Entity ProjectilePrefabEntity;
    public float BulletMoveSpeed;
    public ShootingDirection ShootingDirection;
}
