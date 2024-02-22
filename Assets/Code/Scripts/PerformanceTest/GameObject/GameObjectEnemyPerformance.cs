using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectEnemyPerformance : MonoBehaviour
{
    [SerializeField] float _fireRate;
    [SerializeField] float _rotationSpeed;
    [SerializeField] GameObject _bulletPrefab;

    private float timeElapsed = 0;

    private GameObject _rotate;

    private void Start()
    {
        _rotate = new GameObject("Name");
        _rotate.transform.parent = transform;
        StartCoroutine(MovementCoroutine());
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        _rotate.transform.Rotate(new Vector3(0, 0, _rotationSpeed * Time.deltaTime));
        if(_fireRate < timeElapsed)
        {
            Fire();
        }
        
    }

    public IEnumerator MovementCoroutine()
    {
        transform.DOMoveY(transform.position.y + 2f, 2f);
        yield return new WaitForSeconds(2f);
        transform.DOMoveY(transform.position.y - 2f, 2f);
        yield return new WaitForSeconds(2f);

        StartCoroutine(MovementCoroutine());

    }

    public void Fire()
    {
        timeElapsed = 0;
        GameObject bullet = Instantiate(_bulletPrefab, transform.position, _rotate.transform.rotation);
        Destroy(bullet, 3f);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }
}
