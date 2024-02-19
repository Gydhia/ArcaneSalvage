using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Rendering;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TMP_InputField SpawningNumber;
    [SerializeField] private GameObject entitieGO;

    [Header("EntitiesUtilities")] 
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;
    
    
    private int nbrOfEntities;

    public void ValidSpawningNumber()
    {
        if(SpawningNumber.text != "")
        {
            nbrOfEntities = int.Parse(SpawningNumber.text);
        }
    }

    public void SpawnGoEntities()
    {
        for (int i = 0; i < nbrOfEntities; i++)
        {
            GameObject go = Instantiate(entitieGO);
            go.AddComponent<SpriteRenderer>();
            SpriteRenderer SP = go.GetComponent<SpriteRenderer>();
            SP.sprite = _material.GetTexture();
        } 
    }
    
    public void SpawnECSEntities()
    {
        
    }
    
    public void SpawnECSEntitiesWithPooling()
    {
        
    }    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
