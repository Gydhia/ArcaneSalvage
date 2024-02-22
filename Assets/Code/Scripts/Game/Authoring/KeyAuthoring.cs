using Unity.Entities;
using UnityEngine;

namespace Code.Scripts.Game.Authoring
{
    public class KeyAuthoring : MonoBehaviour
    {
        private int numberOfKeys = 0;
        private class Baker : Baker<KeyAuthoring>
        {
            public override void Bake(KeyAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Key
                {
                    numberOfKeys = authoring.numberOfKeys
                });
            }
        }
    }

    public struct Key : IComponentData
    {
        public int numberOfKeys;
    }
}