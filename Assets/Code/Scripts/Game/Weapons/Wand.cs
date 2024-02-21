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
            spawnedFireBall = Instantiate(projectile, transform.position, Quaternion.identity);
            spawnedFireBall.GetComponent<Projectile>().speed = speed;
            spawnedFireBall.GetComponent<Projectile>().bulletLife = bulletLife;

            if (projectile != null)
            {
                Vector2 direction = target.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                spawnedFireBall.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            }
        }
    }
}
