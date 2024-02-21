using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SocialPlatforms;
using Unity.Transforms;
using Assets.Code.Scripts.Game.Player;
using Unity.Physics;

public partial struct PlayerAnimationSystem : ISystem
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

        foreach (var (transform, playerComponent, velocity, entity) in SystemAPI.Query<LocalTransform, RefRW<InputVariables>, PhysicsVelocity>().WithEntityAccess())
        {
            if (!entityManager.HasComponent<VisualsReferenceComponent>(entity))
            {
                GameObject playerVisuals = Object.Instantiate(animationVisualPrefabs.Player);
                ECB.AddComponent(entity, new VisualsReferenceComponent { gameObject = playerVisuals});
            }
            else
            {
                VisualsReferenceComponent playerVisualsReference = entityManager.GetComponentData<VisualsReferenceComponent>(entity);

                playerVisualsReference.gameObject.transform.position = transform.Position;
                //playerVisualsReference.gameObject.transform.rotation = transform.Rotation;

                //playerVisualsReference.gameObject.GetComponent<Animator>().SetBool("Idle", !SystemAPI.GetSingleton<InputComponent>().CanMove);
                PlayerAnimationBehavior animation = playerVisualsReference.gameObject.GetComponent<PlayerAnimationBehavior>();
                animation.ToggleIdle(!SystemAPI.GetSingleton<InputComponent>().CanMove);
                animation.UpdateVelocity(velocity.Linear, playerComponent.ValueRO.IsPhaseTwo);
                
                
            }

        }
        ECB.Playback(entityManager);
        ECB.Dispose();
    }
}
