using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Code.Scripts.Game.Player
{
    public class PlayerTracker : MonoBehaviour
    {
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
     
            Entities.ForEach((PlayerTracker tracker) =>
            {
                tracker.transform.SetPositionAndRotation(localToWorld.Position, localToWorld.Rotation);
            }).WithoutBurst().Run();
        }
    }
}