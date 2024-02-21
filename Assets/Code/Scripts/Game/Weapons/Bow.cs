using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon
{
    public GameObject projectile;
    private GameObject _spawnedBullet;
    private Transform _target;

    public void Fire(Transform target)
    {
        if (projectile)
        {
            Vector3 directionDiff = new Vector3(0, 1, 0); // Ajustement pour replacer les objets instantiés au bon endroit, et l'angle de visée en fonction

            Vector3 posSpawn = transform.position + directionDiff;
            _spawnedBullet = Instantiate(projectile, posSpawn, Quaternion.identity);
            _spawnedBullet.GetComponent<Projectile>().speed = speed;
            _spawnedBullet.GetComponent<Projectile>().bulletLife = bulletLife;

            if (projectile != null)
            {
                Vector2 direction = (target.position - directionDiff) - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                _spawnedBullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            }
        }

    }

    public override void ChoosingFire(Transform target)
    {
        if (!projectile)
            return;

        _target = target;

        switch (attackType)
        {
            case AttackType.Angle:
                FireMoreArrow();
                break;
            case AttackType.Piercing:
                FireAngle();
                break;
            case AttackType.MoreArrow:
                FireExploding();
                break;
            case AttackType.Exploding:
                FirePiercing();
                break;
        }
    }

    public void FireMoreArrow()
    {
        _spawnedBullet = Instantiate(projectile, transform.position, Quaternion.identity);
        _spawnedBullet.GetComponent<Projectile>().speed = speed;
        _spawnedBullet.GetComponent<Projectile>().bulletLife = bulletLife;

        if (projectile != null)
        {
            Vector2 direction = _target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _spawnedBullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
    }

    public void FireAngle()
    {
        _spawnedBullet = Instantiate(projectile, transform.position, Quaternion.identity);
        _spawnedBullet.GetComponent<Projectile>().speed = speed;
        _spawnedBullet.GetComponent<Projectile>().bulletLife = bulletLife;

        if (projectile != null)
        {
            Vector2 direction = _target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _spawnedBullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
    }

    public void FireExploding()
    {
        _spawnedBullet = Instantiate(projectile, transform.position, Quaternion.identity);
        _spawnedBullet.GetComponent<Projectile>().speed = speed;
        _spawnedBullet.GetComponent<Projectile>().bulletLife = bulletLife;

        if (projectile != null)
        {
            Vector2 direction = _target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _spawnedBullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
    }

    public void FirePiercing()
    {
        _spawnedBullet = Instantiate(projectile, transform.position, Quaternion.identity);
        _spawnedBullet.GetComponent<Projectile>().speed = speed;
        _spawnedBullet.GetComponent<Projectile>().bulletLife = bulletLife;

        if (projectile != null)
        {
            Vector2 direction = _target.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _spawnedBullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        }
    }
}
