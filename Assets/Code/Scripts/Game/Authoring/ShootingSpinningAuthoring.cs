using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ShootingSpinningAuthoring : MonoBehaviour
{
    public float BulletDamage;
    public OwnerType OwnerType;
    public float FireRate;
    public GameObject ProjectilePrefab;
    public float BulletMoveSpeed;
    public float AngleIncrease;

    private class Baker : Baker<ShootingSpinningAuthoring>
    {
        public override void Bake(ShootingSpinningAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ShootingSpinning
            {
                BulletDamage = authoring.BulletDamage,
                OwnerType = authoring.OwnerType,
                FireRate = authoring.FireRate,
                OriginalFireRate = authoring.FireRate,
                ProjectilePrefabEntity = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.None),
                BulletMoveSpeed = authoring.BulletMoveSpeed,
                AngleIncrease = authoring.AngleIncrease,
                BaseAngle = 0.0f,
            });
        }
    }
}

public struct ShootingSpinning : IComponentData
{
    public float BulletDamage;
    public OwnerType OwnerType;
    public float FireRate;
    public float OriginalFireRate;
    public Entity ProjectilePrefabEntity;
    public float BulletMoveSpeed;
    public float AngleIncrease;
    public float BaseAngle;
}
