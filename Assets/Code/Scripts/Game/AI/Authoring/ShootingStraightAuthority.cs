using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ShootingStraightAuthority : MonoBehaviour
{
    public float FireRange;
    public float FireRate;
    public GameObject ProjectilePrefab;
    public float BulletMoveSpeed;
    public int NumberOfShoot;
    public float AngleDifference;

    private int _cooldownID = CooldownManager.NewId;

    private class Baker: Baker<ShootingStraightAuthority> 
    {
        public override void Bake(ShootingStraightAuthority authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ShootingStraight
            {
                FireRange = authoring.FireRange,
                FireRate = authoring.FireRate,
                ProjectilePrefabEntity = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
                BulletMoveSpeed = authoring.BulletMoveSpeed,
                AngleDifference = authoring.AngleDifference,
                NumberOfShoot = authoring.NumberOfShoot,
                CooldownID = authoring._cooldownID
            });
        }
    }
}

public struct ShootingStraight : IComponentData
{
    public float FireRange;
    public float FireRate;
    public Entity ProjectilePrefabEntity;
    public float BulletMoveSpeed;
    public int NumberOfShoot;
    public float AngleDifference;
    public int CooldownID;
}