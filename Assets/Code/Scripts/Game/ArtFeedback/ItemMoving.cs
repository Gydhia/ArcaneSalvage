using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMoving : MonoBehaviour
{
    [SerializeField] public Vector3 _speed;

    public void Update()
    {
        transform.position += _speed * Time.deltaTime;
    }
}
