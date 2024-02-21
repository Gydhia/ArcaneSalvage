using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SocialPlatforms;
using Unity.Transforms;

public partial struct PlayerAnimationSystem : ISystem
{
    private EntityManager entityManager;

    private void OnUpdate(ref SystemState state)
    {
        if (!SystemAPI.ManagedAPI.TryGetSingleton(out AnimationVisualsPrefabs animationVisualPrefabs))
        {
            return;
        }

        foreach (var (transform, playerComponent, entity) in SystemAPI.Query<LocalTransform, PlayerControlsAuthoring>().WithEntityAccess())
        {
            if (!entityManager.HasComponent<VisualsReferenceComponent>(entity))
            {

            }

        }
        entityManager = state.EntityManager;
    }
}
