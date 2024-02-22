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
    private int _cooldownID = CooldownManager.NewId;

    private class Baker : Baker<ShootCardinalAuthoring> 
    {
        public override void Bake(ShootCardinalAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ShootingCardinal
            {
                FireRate = authoring.FireRate,
                ProjectilePrefabEntity = GetEntity(authoring.ProjectilePrefab, TransformUsageFlags.None),
                BulletMoveSpeed = authoring.BulletMoveSpeed,
                ShootingDirection = authoring.Direction,
                CooldownID = authoring._cooldownID
            });
        }
    }
}

public struct ShootingCardinal : IComponentData
{
    public float FireRate;
    public Entity ProjectilePrefabEntity;
    public float BulletMoveSpeed;
    public ShootingDirection ShootingDirection;
    public int CooldownID;

}
