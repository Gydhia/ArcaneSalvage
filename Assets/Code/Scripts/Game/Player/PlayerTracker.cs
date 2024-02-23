using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Scripts.Game.Player
{
    public class PlayerTracker : MonoBehaviour
    {
        private EntityManager em;
        private Entity entity;
        private void Start()
        {
            em = World.DefaultGameObjectInjectionWorld.EntityManager;
            entity = em.CreateEntity();
            em.SetName(entity, name);
            em.AddComponentObject(entity, this);
            
        }

        private void OnDestroy()
        {
            em.DestroyEntity(entity);
        }
    }
     
    [UpdateInGroup(typeof(TransformSystemGroup), OrderLast = true)]
    public partial class PlayerTrackerSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            if (!SystemAPI.TryGetSingletonEntity<InputVariables>(out var playerEntity))
                return;
            
            var localToWorld = SystemAPI.GetComponent<LocalToWorld>(playerEntity);
            
            Entities.ForEach((PlayerTracker tracker) =>
            {
                tracker.transform.SetPositionAndRotation(localToWorld.Position, localToWorld.Rotation);
            }).WithoutBurst().Run();
        }
    }
}