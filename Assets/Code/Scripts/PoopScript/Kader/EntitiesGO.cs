using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public class EntitiesGO : MonoBehaviour
{
    // Start is called before the first frame update
    private float moveSpeed;
    private static List<EntitiesGO> allEntities = new List<EntitiesGO>();
    void Start()
    {
        allEntities.Add(this);
        transform.position = new Vector3(Random.Range(8, -8), Random.Range(5, -5), transform.position.z);
        moveSpeed = Random.Range(1f,2f);
    }

    // Update is called once per frame
    void Update()
    {
        float y = transform.position.y;
        y += moveSpeed * Time.deltaTime;
        if (y > 5f)
        {
            moveSpeed = -math.abs(moveSpeed);
        }else if(y < -5f)
        {
            moveSpeed = +math.abs(moveSpeed);
        }
        
        transform.position = new Vector3(transform.position.x, y, transform.position.z);

        //transform.position.y += 1f * Time.deltaTime;
    }

    public static void ClearEntities()
    {
        foreach (EntitiesGO entitiesGo in allEntities)
        {
            Destroy(entitiesGo.gameObject);
        }
        
        allEntities.Clear();
    }
}
