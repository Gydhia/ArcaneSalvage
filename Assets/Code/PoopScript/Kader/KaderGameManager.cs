using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

public class KaderGameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TMP_InputField SpawningNumber;
    public GameObject entitieGO;

    [Header("EntitiesUtilities")] 
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Mesh _mesh;
    [SerializeField] private Material _material;

    private EntityArchetype entityArchetype;
    
    public int nbrOfEntities;
    
    public class BakerScript : Baker<KaderGameManager>
    {
        private static GameObject entityGO;
        public override void Bake(KaderGameManager kader)
        {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new GameManagerComponent()
            {
                nbrEntity = kader.nbrOfEntities,
                EntityECS = GetEntity(kader.entitieGO, TransformUsageFlags.Dynamic)
            });
        }
    }

    public void ValidSpawningNumber()
    {
        Debug.Log(SpawningNumber.text);
        if(SpawningNumber.text != "")
        {
            nbrOfEntities = int.Parse(SpawningNumber.text);
        }
    }

    public void SpawnGoEntities()
    {
        EntitiesGO.ClearEntities();
        for (int i = 0; i < nbrOfEntities; i++)
        {
            GameObject go = Instantiate(entitieGO);
            go.transform.localScale = new Vector3(0.2f, 0.2f, transform.localScale.z);
        } 
    }
}



