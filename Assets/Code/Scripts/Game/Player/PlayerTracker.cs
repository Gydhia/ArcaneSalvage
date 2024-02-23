using System;
using Code.Scripts.Helper;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code.Scripts.Game.Player
{
    public class PlayerTracker : Singleton<PlayerTracker>
    {
        protected override void Awake()
        {
            DontDestroyOnLoad = false;
            base.Awake();
        }

        private void Start()
        {
            var em = World.DefaultGameObjectInjectionWorld.EntityManager;
            var entity = em.CreateEntity();
            em.SetName(entity, name);
            em.AddComponentObject(entity, this);
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

            PlayerTracker.Instance.transform.SetPositionAndRotation(localToWorld.Position, localToWorld.Rotation);
            
            // Entities.ForEach((PlayerTracker tracker) =>
            // {
            //     if (tracker != null)
            //     {
            //         tracker.transform.SetPositionAndRotation(localToWorld.Position, localToWorld.Rotation);
            //     }
            // }).WithoutBurst().Run();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
        }
    }
}