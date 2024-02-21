using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    private GameObject spawnedBullet;
    public GameObject projectile;

    public override void Fire(Transform target)
    {
        if (projectile)
        {
            spawnedBullet = Instantiate(projectile, transform.position, Quaternion.identity);
            spawnedBullet.GetComponent<Projectile>().speed = speed;
            spawnedBullet.GetComponent<Projectile>().bulletLife = bulletLife;

            if (projectile != null)
            {
                Vector2 direction = target.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                spawnedBullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            }
        }

    }
}
