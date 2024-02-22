using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class AnimationVisualsPrefabsAuthoring : MonoBehaviour
{
    [SerializeField] private GameObject Player;

    private class AnimationVisualsPrefabBaker : Baker<AnimationVisualsPrefabsAuthoring>
    {
        public override void Bake(AnimationVisualsPrefabsAuthoring authoring)
        {
            Entity playerPrefabEntity = GetEntity(TransformUsageFlags.None);

            AddComponentObject(playerPrefabEntity, new AnimationVisualsPrefabs
            {
                Player = authoring.Player
            });
        }
    }
}
