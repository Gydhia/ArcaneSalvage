using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Projectile : MonoBehaviour
{

    public float bulletLife = 1f;  // Defines how long before the bullet is destroyed
    public float damages = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.activeInHierarchy)
            return;

        if (other.gameObject.CompareTag("Ennemy"))
        {
            other.GetComponent<HealthComponent>().TakeDamage(damages);
        }
        
    }
}
