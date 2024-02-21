using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ShootingSpinningAuthoring : MonoBehaviour
{
    public float FireRate;
    public GameObject ProjectilePrefab;
    public float BulletMoveSpeed;
    public float AngleIncrease;
    private int _cooldownID = CooldownManager.NewId;
    private class Baker : Baker<ShootingSpinningAuthoring>
    {
        public override void Bake(ShootingSpinningAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ShootingSpinning
            {
                FireRate = authoring.FireRate,
                ProjectilePrefabEntity = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.None),
                BulletMoveSpeed = authoring.BulletMoveSpeed,
                AngleIncrease = authoring.AngleIncrease,
                BaseAngle = 0.0f,
                CooldownID = authoring._cooldownID
            });
        }
    }
}

public struct ShootingSpinning : IComponentData
{
    public float FireRate;
    public Entity ProjectilePrefabEntity;
    public float BulletMoveSpeed;
    public float AngleIncrease;
    public float BaseAngle;
    public int CooldownID;
}
