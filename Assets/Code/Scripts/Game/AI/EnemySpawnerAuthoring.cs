using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class EnemySpawnerAuthoring : MonoBehaviour
{
    public float SpawnCooldown = 1;
    public float MinDistanceFromPlayer = 2f;
    public float SpawnRadius = 10f;
    
    public List<EnemyPreset> EnemyPresets;

    public class EnemySpawnerBaker : Baker<EnemySpawnerAuthoring>
    {
        public override void Bake(EnemySpawnerAuthoring authoring)
        {
            Entity enemySpawnerAuthoring = GetEntity(TransformUsageFlags.None);
            
            AddComponent(enemySpawnerAuthoring, new EnemySpawnerComponent
            {
                SpawnCooldown = authoring.SpawnCooldown,
                MinDistanceFromPlayer = authoring.MinDistanceFromPlayer,
                SpawnRadius = authoring.SpawnRadius
            });

            List<EnemyData> EnemyDatas = new List<EnemyData>();

            foreach (var enemyPreset in authoring.EnemyPresets)
            {
                EnemyDatas.Add( new EnemyData
                {
                    Damage = enemyPreset.Damage,
                    Health = enemyPreset.Health,
                    MoveSpeed = enemyPreset.MoveSpeed,
                    Prefab = GetEntity(enemyPreset.Prefab, TransformUsageFlags.None),
                });
            }
            
            AddComponentObject(enemySpawnerAuthoring, new EnemyDataContainer {Enemies = EnemyDatas});
        }
    }
}
