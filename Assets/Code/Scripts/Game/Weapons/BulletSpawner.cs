using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BulletSpawner : MonoBehaviour
{
    enum SpawnerType { Straight, Spin }
    enum GameModePhase { Phase1, Phase2 }


    [Header("Bullet Attributes")]
    public GameObject bullet;
    public float bulletLife = 1f;
    public float speed = 1f;
    public GameObject target;


    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private GameModePhase gameModePhase;
    [SerializeField] private float firingRate = 1f;


    private GameObject spawnedBullet;
    private float timer = 0f;


    void Start() { }

    void Update()
    {
        timer += Time.deltaTime;
        if (spawnerType == SpawnerType.Spin) transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z + 1f);
        if (timer >= firingRate)
        {
            if (gameModePhase == GameModePhase.Phase1)
            {
                FindClosestEnemy();
                Fire();
            }
            else
            {
                // code phase 2
            }
            timer = 0;
        }
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        target = null;
        float minDistance = Mathf.Infinity;
        Vector3 playerPosition = transform.position;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(playerPosition, enemy.transform.position);
            if (distance < minDistance)
            {
                target = enemy;
                minDistance = distance;
            }
        }
    }

    private void Fire()
    {
        if (bullet)
        {
            spawnedBullet = Instantiate(bullet, transform.position, Quaternion.identity);
            spawnedBullet.GetComponent<Projectile>().speed = speed;
            spawnedBullet.GetComponent<Projectile>().bulletLife = bulletLife;

            Vector3 direction = target.transform.position - transform.position; // Direction vers la cible
            Quaternion rotation = Quaternion.LookRotation(direction); // Calcul de la rotation nécessaire
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10); // Applique la rotation avec interpolation
        }
    }
}
