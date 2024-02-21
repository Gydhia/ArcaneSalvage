using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConstantSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> _itemPrefab;
    [SerializeField] BoxCollider2D _spawnCollider;

    [SerializeField] float _minScale;
    [SerializeField] float _maxScale;

    [SerializeField] Vector3 _minSpeed;
    [SerializeField] Vector3 _maxSpeed;

    [SerializeField] float _minSpawnDelay;
    [SerializeField] float _maxSpawnDelay;

    private void Start()
    {
        StartCoroutine(SpawnItem());
    }

    public IEnumerator SpawnItem()
    {
        Vector3 randomPoint = GetRandomPointInCollider(_spawnCollider);
        GameObject instantiated = Instantiate(_itemPrefab[Random.Range(0, _itemPrefab.Count-1)], randomPoint, Quaternion.identity);

        ItemMoving item = instantiated.GetComponent<ItemMoving>();
        item.transform.localScale = Vector3.one * Random.Range(_minScale, _maxScale);
        Vector3 slidingSpeed = new Vector3(Random.Range(_minSpeed.x, _maxSpeed.x), Random.Range(_minSpeed.y, _maxSpeed.y), Random.Range(_minSpeed.z, _maxSpeed.z));
        item._speed = slidingSpeed;

        float nextDelay = Random.Range(_minSpawnDelay, _maxSpawnDelay);
        yield return new WaitForSeconds(nextDelay);
        StartCoroutine(SpawnItem());
    }

    Vector3 GetRandomPointInCollider(Collider2D collider)
    {
        Vector3 randomPoint = new Vector3(
            Random.Range(collider.bounds.min.x, collider.bounds.max.x),
            Random.Range(collider.bounds.min.y, collider.bounds.max.y),
            Random.Range(collider.bounds.min.z, collider.bounds.max.z)
        );
        return randomPoint;
    }
}
