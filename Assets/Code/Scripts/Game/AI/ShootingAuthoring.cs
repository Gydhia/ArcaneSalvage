using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class ShootingAuthoring : MonoBehaviour
{
    public List<Transform> Shooters = new List<Transform>();
    public float FireRange;
    public float FireRate;
    public GameObject ProjectilePrefab;
    public float BulletMoveSpeed;
    public int NumberOfShoot;
    public float AngleDifference;
    private int _cooldownID = CooldownManager.NewId;

    private class Baker: Baker<ShootingAuthoring> 
    {
        public override void Bake(ShootingAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Shooting
            {
                FireRange = authoring.FireRange,
                FireRate = authoring.FireRate,
                ProjectilePrefabEntity = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.Dynamic),
                BulletMoveSpeed = authoring.BulletMoveSpeed,
                NumberOfShoot = authoring.NumberOfShoot,
                AngleDifference = authoring.AngleDifference,
                CooldownID = authoring._cooldownID
            });
        }
    }
}

public struct Shooting : IComponentData
{
    public float FireRange;
    public float FireRate;
    public Entity ProjectilePrefabEntity;
    public float BulletMoveSpeed;
    public int NumberOfShoot;
    public float AngleDifference;
    public int CooldownID;
}