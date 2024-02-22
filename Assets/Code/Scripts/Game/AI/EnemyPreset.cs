using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/EnemyPreset")]
public class EnemyPreset : ScriptableObject
{
    public GameObject Prefab;
    public float Health;
    public float MoveSpeed;
    public float Damage;
}
