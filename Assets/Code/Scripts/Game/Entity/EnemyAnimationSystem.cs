
using Assets.Code.Scripts.Game.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public partial struct EnemyAnimationSystem : ISystem
{
    private EntityManager entityManager;

    private void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.ManagedAPI.TryGetSingleton(out AnimationVisualsPrefabs animationVisualPrefabs))
        {
            return;
        }

        entityManager = state.EntityManager;
        EntityCommandBuffer ECB = new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (transform, goblinTorch, moving, entity) in SystemAPI.Query<LocalTransform, EnemyGoblinTorchData, Moving>().WithEntityAccess())
        {
            if (!entityManager.HasComponent<VisualsReferenceComponent>(entity))
            {
                GameObject animationVisuals  = Object.Instantiate(animationVisualPrefabs.TorchGoblin);
                ECB.AddComponent(entity, new VisualsReferenceComponent { gameObject = animationVisuals });
            }
            else
            {
                VisualsReferenceComponent enemyVisualsReference = entityManager.GetComponentData<VisualsReferenceComponent>(entity);

                enemyVisualsReference.gameObject.transform.position = transform.Position;

                EnemyGoblinTorchAnimationBehavior animation = enemyVisualsReference.gameObject.GetComponent<EnemyGoblinTorchAnimationBehavior>();
                animation.UpdateVelocity(moving.Direction);
            }
        }
        ECB.Playback(entityManager);
        ECB.Dispose();
    }
}