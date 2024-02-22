using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.AI;


namespace Assets.Code.Scripts.Game.Player
{

    public partial class EnemySpawnerSystem : SystemBase
    {
        private EnemySpawnerComponent m_enemySpawnerComponent;
        private EnemyDataContainer m_enemyDataContainerComponent;
        private Entity m_enemySpawnerEntity;
        private float m_nextSpawnTime;

        protected void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<DataSingleton>();

        }

        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingletonEntity<EnemySpawnerComponent>(out m_enemySpawnerEntity))
                return;

            m_enemySpawnerComponent = EntityManager.GetComponentData<EnemySpawnerComponent>(m_enemySpawnerEntity);
            m_enemyDataContainerComponent = EntityManager.GetComponentData<EnemyDataContainer>(m_enemySpawnerEntity);

            if (SystemAPI.Time.ElapsedTime > m_nextSpawnTime)
            {
                SpawnEnemy();
            }
        }

        private void SpawnEnemy()
        {
            System.Random rdm = new System.Random();
            
            EnemyData data = m_enemyDataContainerComponent.Enemies[rdm.Next(0, m_enemyDataContainerComponent.Enemies.Count)];

            var playerPosition = SystemAPI.GetSingleton<DataSingleton>().PlayerPosition;
            
            var angle = rdm.Next(0, 360);
            var spawnDirection = Quaternion.Euler(0, 0, angle) * Vector3.right;
            var spawnPosition = playerPosition + new float3(spawnDirection) * m_enemySpawnerComponent.SpawnRadius;

            if (NavMesh.SamplePosition(spawnPosition, out NavMeshHit hit, 0.1f, NavMesh.AllAreas))
            {
                Debug.Log("Spawned Enemy");
                Entity newEnemy = EntityManager.Instantiate(data.Prefab);
                EntityManager.SetComponentData(newEnemy, new LocalTransform
                {   
                    Position = hit.position,
                    Rotation = quaternion.identity,
                    Scale = 1
                });
            }
            else
            {
                Debug.Log("COUDLNT Spawned Enemy");

            }
            
            m_nextSpawnTime = (float)SystemAPI.Time.ElapsedTime + m_enemySpawnerComponent.SpawnCooldown;
        }        
    }
}