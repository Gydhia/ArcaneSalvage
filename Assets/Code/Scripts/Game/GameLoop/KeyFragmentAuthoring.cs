using Assets.Code.Scripts.Game.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class KeyFragmentAuthoring : MonoBehaviour
{
    public int Id;
    public bool Collected;
    public class KeyFragmentBaker : Baker<KeyFragmentAuthoring>
    {
        public override void Bake(KeyFragmentAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new KeyFragmentData
            {
                Id = authoring.Id,
                Collected = authoring.Collected,
            });
        }
    }
}

public struct KeyFragmentData : IComponentData
{
    public int Id;
    public bool Collected;
}
