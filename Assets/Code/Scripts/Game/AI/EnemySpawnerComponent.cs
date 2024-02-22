using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct EnemySpawnerComponent : IComponentData
{
    public float SpawnCooldown;
    public float MinDistanceFromPlayer;
    public float SpawnRadius;
}
