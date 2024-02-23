

using Assets.Code.Scripts.Game.Player;
using Code.Scripts.Game.Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scripts.Game.System
{
    [BurstCompile]
    [UpdateInGroup(typeof(FixedStepSimulationSystemGroup))]
    [UpdateAfter(typeof(PhysicsSystemGroup))]
    
    public partial struct TriggerSystem : ISystem
    {
        
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<Inventory>();
            state.RequireForUpdate<SimulationSingleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer entityCommandBufferKeyDestroy = new EntityCommandBuffer(Allocator.TempJob);
            NativeArray<Inventory> nativeArrayData = new NativeArray<Inventory>(1, Allocator.TempJob);
            
            nativeArrayData[0] = SystemAPI.GetSingleton<Inventory>();
            
            TriggerJob triggerJob = new TriggerJob
            {
                KeyGroup = SystemAPI.GetComponentLookup<Key>(),
                PortalGroup = SystemAPI.GetComponentLookup<Portal>(),
                PlayerGroup = SystemAPI.GetComponentLookup<InputVariables>(),
                dataSingleton = nativeArrayData,
                destroyBuffer = entityCommandBufferKeyDestroy
            
            };
            JobHandle jobHandle = triggerJob.Schedule(SystemAPI.GetSingleton<SimulationSingleton>(), state.Dependency);
            jobHandle.Complete();
            
            entityCommandBufferKeyDestroy.Playback(state.EntityManager);
            entityCommandBufferKeyDestroy.Dispose();
            
            SystemAPI.SetSingleton(nativeArrayData[0]);
            nativeArrayData.Dispose();
        }
        [BurstCompile]
        public partial struct TriggerJob : ITriggerEventsJob
        {
            [ReadOnly] public ComponentLookup<Key> KeyGroup;
            [ReadOnly] public ComponentLookup<Portal> PortalGroup;
            [ReadOnly] public ComponentLookup<InputVariables> PlayerGroup;
            public NativeArray<Inventory> dataSingleton;
            public EntityCommandBuffer destroyBuffer;
            
            
            public void Execute(TriggerEvent triggerEvent)
            {
                Entity entityA = triggerEvent.EntityA;
                Entity entityB = triggerEvent.EntityB;

                (bool, Entity) KeyCheck = TriggerBulletSystem.FindEntityWithComponent(entityA, entityB, KeyGroup);
                (bool, Entity) PortalCheck = TriggerBulletSystem.FindEntityWithComponent(entityA, entityB, PortalGroup);
                (bool, Entity) PlayerCheck = TriggerBulletSystem.FindEntityWithComponent(entityA, entityB, PlayerGroup);

                Inventory dataSingletonCopy = dataSingleton[0];

                
                if (PlayerCheck.Item1 && KeyCheck.Item1)
                {
                    dataSingletonCopy.keyNumber++;
                    destroyBuffer.DestroyEntity(KeyCheck.Item2);
                    
                    dataSingleton[0] = dataSingletonCopy;
                }
                
                if (PlayerCheck.Item1 && PortalCheck.Item1)
                {
                    if (dataSingletonCopy.keyNumber == 4)
                    {
                        //DO SHIT LIKE CHANGE SCENE OR END GAME
                    }
                    Debug.Log("Portal " + (dataSingletonCopy.keyNumber == 4));
                }
                
            }
        }
    }
}