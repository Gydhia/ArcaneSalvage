using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class BoatPlayerAuthoring : MonoBehaviour
{
    private class Baker : Baker<BoatPlayerAuthoring>
    {
        public override void Bake(BoatPlayerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new BoatPlayer());
        }
    }
}

public struct BoatPlayer : IComponentData
{

}
