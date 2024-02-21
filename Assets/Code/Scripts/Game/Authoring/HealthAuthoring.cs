using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class HealthAuthoring : MonoBehaviour
{
    public float MaxHealth;
    public bool DieOnDeath;
    [Tooltip("If Lifetime is negative, this entity won't get destroyed by time")]public float Lifetime = -1.0f;

    public class Baker : Baker<HealthAuthoring>
    {
        public override void Bake(HealthAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Health
            {
                MaxHealth = authoring.MaxHealth,
                CurrentHealth = authoring.MaxHealth,
                DieOnDeath = authoring.DieOnDeath,
                Lifetime = authoring.Lifetime,
            });
        }
    }
}

public struct Health : IComponentData
{
    public float MaxHealth;
    public float CurrentHealth;
    public bool DieOnDeath;
    public float Lifetime;
}
