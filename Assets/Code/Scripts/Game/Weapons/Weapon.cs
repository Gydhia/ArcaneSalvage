using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum AttackType { Angle, Exploding, MoreArrow, Piercing }
    public AttackType attackType;

    public float firingRate = 0.5f;
    public float damage = 1f;
    public float speed = 5f;
    public float bulletLife = 5f;

    public abstract void ChoosingFire(Transform target);
}
