using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Weapons/Weapon")]
public class Weapon : ScriptableObject
{
    public string Name;

    public float firingRate = 0.5f;
    public float FireRange = 5f;
    public int NumberOfShoots = 1;
    public float damage = 1f;
    public float speed = 5f;
    public float bulletLife = 5f;
    public GameObject ProjectilePrefab;
}
