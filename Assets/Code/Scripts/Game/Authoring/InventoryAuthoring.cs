using System.Security.Cryptography;
using Unity.Entities;
using UnityEngine;

namespace Code.Scripts.Game.Authoring
{
    public class InventoryAuthoring : MonoBehaviour
    {
        private int _keyNumber = 0;
        
        public class Baker : Baker<InventoryAuthoring>
        {
            public override void Bake(InventoryAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Inventory
                {
                    keyNumber = authoring._keyNumber
                });
            }
        }
    }

    public struct Inventory : IComponentData
    {
        public int keyNumber;
    }
}