using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EnemyDataContainer : IComponentData
{
    public List<EnemyData> Enemies;
}

public struct EnemyData
{
    public Entity Prefab;
    public float Health;
    public float MoveSpeed;
    public float Damage;
}
