using Unity.Entities;
using UnityEngine;

namespace Code.Scripts.Game.Authoring
{
    public class PortalAuthoring : MonoBehaviour
    {
        private class Baker : Baker<PortalAuthoring>
        {
            public override void Bake(PortalAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Portal());
            }
        }
    }

    public struct Portal : IComponentData
    {
    }
}