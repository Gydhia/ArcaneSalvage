using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public float firingRate = 1f;
    public float damage = 1f;
    public float speed = 1f;
    public float bulletLife = 1f;

    public abstract void Fire(Transform target);
}
