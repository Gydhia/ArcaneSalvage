using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SocialPlatforms;
using Unity.Transforms;
using Assets.Code.Scripts.Game.Player;
using Unity.Physics;
using static UnityEngine.RuleTile.TilingRuleOutput;
using static MovementSystem;

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

        foreach (var (transform, playerComponent, velocity, entity) in SystemAPI.Query<LocalTransform, RefRW<InputVariables>, Moving>().WithEntityAccess())
        {
            if (!entityManager.HasComponent<VisualsReferenceComponent>(entity))
            {
                GameObject playerVisuals = Object.Instantiate(animationVisualPrefabs.Player);
                ECB.AddComponent(entity, new VisualsReferenceComponent { gameObject = playerVisuals });
            }
            else
            {
                VisualsReferenceComponent playerVisualsReference = entityManager.GetComponentData<VisualsReferenceComponent>(entity);

                playerVisualsReference.gameObject.transform.position = transform.Position;

                PlayerAnimationBehavior animation = playerVisualsReference.gameObject.GetComponent<PlayerAnimationBehavior>();
                
                animation.ToggleIdle(!SystemAPI.GetSingleton<InputComponent>().CanMove);
                animation.UpdateVelocity(velocity.Direction, playerComponent.ValueRO.IsPhaseTwo);
        }

        ECB.Playback(entityManager);
        ECB.Dispose();
    }

    //[WithAll(typeof(Moving), typeof(InputVariables))]
    //public partial struct AnimateJob : IJobEntity
    //{
    //    public InputComponent inputComponent;
    //    public EntityCommandBuffer ECB;
    //    public AnimationVisualsPrefabs animationVisualPrefabs;
    //    public void Execute(ref LocalTransform localTransform, Entity entity,  RefRW<InputVariables> inputVariables, in Moving moveData, VisualsReferenceComponent visualsReference)
    //    {
    //        if(visualsReference == null)
    //        {
    //            GameObject playerVisuals = Object.Instantiate(animationVisualPrefabs.Player);
    //            ECB.AddComponent(entity, new VisualsReferenceComponent { gameObject = playerVisuals });
    //        }

    //        visualsReference.gameObject.transform.position = localTransform.Position;

    //        PlayerAnimationBehavior animation = visualsReference.gameObject.GetComponent<PlayerAnimationBehavior>();
    //        animation.ToggleIdle(inputComponent.CanMove);
    //        animation.UpdateVelocity(moveData.Direction, inputVariables.ValueRO.IsPhaseTwo);
    //    }
    //}
}
