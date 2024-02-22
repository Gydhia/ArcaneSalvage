using System.Collections;
using System.Collections.Generic;
using Unity.Transforms;
using UnityEngine;

public class GameObjectPerformanceSpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _prefabToSpawn;

    public void Spawn(List<Vector2> spawnPoint)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            // Détruire chaque enfant
            Destroy(transform.GetChild(i).gameObject);
        }
        foreach (var item in spawnPoint)
        {
            Instantiate(_prefabToSpawn, item, Quaternion.identity, transform);
        }
    }
}
