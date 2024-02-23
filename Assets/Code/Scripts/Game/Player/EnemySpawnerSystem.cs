using System;
using System.Collections;
using ArcanaSalvage.UI;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;
using Random = Unity.Mathematics.Random;

namespace Assets.Code.Scripts.Game.Player
{
    public partial class EnemySpawnerSystem : SystemBase
    {
        private EnemySpawnerComponent m_enemySpawnerComponent;
        private EnemyDataContainer m_enemyDataContainerComponent;
        private Entity m_enemySpawnerEntity;
        private float m_nextSpawnTime;
        private int m_nbEnemiesToSpawn;

        private Random m_random;
        private NativeList<float3> m_enemyPositions;

        private bool m_spawnEnemies;
        
        protected override void OnCreate()
        {
            m_random = Random.CreateFromIndex((uint)m_enemySpawnerComponent.GetHashCode());
            m_nbEnemiesToSpawn = 10;
        }
        
        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingletonEntity<EnemySpawnerComponent>(out m_enemySpawnerEntity))
                return;
            
            m_enemySpawnerComponent = EntityManager.GetComponentData<EnemySpawnerComponent>(m_enemySpawnerEntity);
            m_enemyDataContainerComponent = EntityManager.GetComponentData<EnemyDataContainer>(m_enemySpawnerEntity);
            
            if (SystemAPI.Time.ElapsedTime > m_nextSpawnTime)
            {
                GameUIManager.Instance.StartCoroutine(
                    CalculateSpawnPoints(SpawnEnemies));
                
                m_nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + m_enemySpawnerComponent.SpawnCooldown;
            }
        }

        private IEnumerator CalculateSpawnPoints(Action callback)
        {
            m_enemyPositions = new NativeList<float3>(Allocator.Persistent);
        
            for (int i = 0; i < m_nbEnemiesToSpawn; i++)
            {
                if(!SystemAPI.TryGetSingleton(out DataSingleton dataSingleton))
                    continue;
            
                var angle = m_random.NextFloat(0f, 360f);
                var spawnDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;
                var spawnPosition = dataSingleton.PlayerPosition + new float3(spawnDirection) * m_enemySpawnerComponent.SpawnRadius;

                if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
                {
                    m_enemyPositions.Add(hit.position);
                }

                yield return null;
            }
            callback?.Invoke();
        }

        public void SpawnEnemies()
        {
                EntityCommandBuffer entityCommandBufferStraightJob = new EntityCommandBuffer(Allocator.TempJob);
            EnemyData eData = m_enemyDataContainerComponent.Enemies[m_random.NextInt(0, m_enemyDataContainerComponent.Enemies.Count)];
                
            SpawnEnemyJob spawnEnemyJob = new SpawnEnemyJob
            {
                EntityCommandBuffer = entityCommandBufferStraightJob,
                EnemyData = eData,
                EnemyPositions = m_enemyPositions
            };
            JobHandle spawnHandle = spawnEnemyJob.Schedule(Dependency);
            spawnHandle.Complete();
            
            entityCommandBufferStraightJob.Playback(EntityManager);
            entityCommandBufferStraightJob.Dispose();
            m_enemyPositions.Dispose();
        }
        
        [BurstCompile, WithAll(typeof(EnemySpawnerComponent))]
        public partial struct SpawnEnemyJob : IJobEntity
        {
            public EntityCommandBuffer EntityCommandBuffer;
            public EnemyData EnemyData;
            public NativeList<float3> EnemyPositions;
            
            public void Execute()
            {
                NativeArray<Entity> entities = 
                    new NativeArray<Entity>(EnemyPositions.Length, Allocator.Temp);
                EntityCommandBuffer.Instantiate(EnemyData.Prefab, entities);
                
                for (int i = 0; i < EnemyPositions.Length; i++)
                {
                    EntityCommandBuffer.SetComponent(entities[i], new LocalTransform
                    {   
                        Position = EnemyPositions[i],
                        Rotation = quaternion.identity,
                        Scale = 1
                    });
                }
            }
        }
    }
}