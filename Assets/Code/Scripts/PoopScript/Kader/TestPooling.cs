using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class TestPooling : MonoBehaviour
{
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Entity enemy = PoolingAuthoring.Instance.GetObject(PoolingType.Ennemy);
        Debug.Log(enemy);
        
        PoolingAuthoring.Instance.ReturnObject(enemy);
    }
}
