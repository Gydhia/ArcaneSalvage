using Code.Scripts.Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : MonoBehaviour
{
    public float ProjectileDamage;
    public float ProjectileLifetime;

    private void Start()
    {
        Debug.Log("MonoBehaviour Projectile Enemy start");
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerBehaviour playerBehaviour))
        {
            if (other.TryGetComponent(out HealthComponent healthComponent))
            {
                Debug.Log("Hit Player");
                healthComponent.TakeDamage(ProjectileDamage);
            }
        }
    }
}
