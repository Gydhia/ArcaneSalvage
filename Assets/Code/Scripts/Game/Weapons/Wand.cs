using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wand : Weapon
{
    private GameObject spawnedFireBall;
    public GameObject projectile;

    public override void Fire(Transform target)
    {
        if (projectile)
        {
            Vector3 directionDiff = new Vector3(0, 1, 0); // Ajustement pour replacer les objets instantiés au bon endroit, et l'angle de visée en fonction

            Vector3 posSpawn = transform.position + directionDiff;
            spawnedFireBall = Instantiate(projectile, posSpawn, Quaternion.identity);
            spawnedFireBall.GetComponent<Projectile>().speed = speed;
            spawnedFireBall.GetComponent<Projectile>().bulletLife = bulletLife;

            if (projectile != null)
            {
                Vector2 direction = (target.position - directionDiff) - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                spawnedFireBall.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            }
        }
    }
}
