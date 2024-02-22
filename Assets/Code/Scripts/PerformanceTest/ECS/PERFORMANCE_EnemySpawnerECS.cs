using Assets.Code.Scripts.Game.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class PERFORMANCE_EnemySpawnerECS : MonoBehaviour
{
    [SerializeField] public List<Vector2> Pos;
    [SerializeField] public int Count;
    [SerializeField] public bool Spawn;
    [SerializeField] public GameObject Prefab;

    public class Baker : Baker<PERFORMANCE_EnemySpawnerECS>
    {
        public override void Bake(PERFORMANCE_EnemySpawnerECS authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponentObject(entity, new PERFORMANCE_EnemySpawnerData
            {
                Pos=authoring.Pos,
                Prefab=GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),
                Spawn=authoring.Spawn,
            });
        }
    }
}

public class PERFORMANCE_EnemySpawnerData : IComponentData
{
    [SerializeField] public List<Vector2> Pos;
    [SerializeField] public bool Spawn;
    [SerializeField] public Entity Prefab;


}
