using Assets.Code.Scripts.Game.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PERFORMANCE_EnemySpawnerECSSystem : ISystem
{
    public void OnUpdate(ref SystemState state)
    {

        EntityCommandBuffer ECB = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);
        foreach (var (spawner, entity) in SystemAPI.Query<PERFORMANCE_EnemySpawnerData>().WithEntityAccess())
        {
            if (spawner.Spawn)
            {
                foreach (var item in spawner.Pos)
                {
                    Debug.Log(item);
                    Entity spawned = state.EntityManager.Instantiate(spawner.Prefab);

                    LocalTransform transformData = state.EntityManager.GetComponentData<LocalTransform>(spawned);

                    // Modifier la position de l'entité
                    transformData.Position = new float3(item.x, item.y, 0);

                    // Appliquer les modifications au composant de transformation
                    state.EntityManager.SetComponentData<LocalTransform>(spawned, transformData);

                }
            }
            spawner.Spawn = false;
        }
        ECB.Dispose();

    }
}
