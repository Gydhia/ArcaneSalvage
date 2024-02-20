using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

public partial struct GameManagerSystem : ISystem
{
    private EntityManager entityManager;
    
    private Entity inputEntity;
    private Entity gameManagerEntity;
    private Entity inputTextEntity;

    private InputComponent inputComponent;
    private GameManagerComponent gameManagerComponent;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameManagerComponent>();
    }

    public void OnUpdate(ref SystemState state)
    {
        entityManager = state.EntityManager;
        
        inputEntity = SystemAPI.GetSingletonEntity<InputComponent>();
        gameManagerEntity = SystemAPI.GetSingletonEntity<GameManagerComponent>();

        
        
        inputComponent = entityManager.GetComponentData<InputComponent>(inputEntity);
        gameManagerComponent = entityManager.GetComponentData<GameManagerComponent>(gameManagerEntity);
        //inputTextEntity = entityManager.GetComponentObject<TMP_InputField>(gameManagerEntity);
        
        SpawnGO(ref state);
        SpawnECS(ref state);
        SpawnECSWithPooling(ref state);
    }

    private void SpawnGO(ref SystemState state)
    {
        
    }

    private float nextPressTime;
    private void SpawnECS(ref SystemState state)
    {
        if (inputComponent.pressing2 && nextPressTime < SystemAPI.Time.ElapsedTime)
        {
            NativeArray<Entity> entityArray = new NativeArray<Entity>(gameManagerComponent.nbrEntity, Allocator.Temp);
            entityManager.Instantiate(gameManagerComponent.EntityECS, entityArray);
            nextPressTime = (float) SystemAPI.Time.ElapsedTime + 1f;         
        }
    }
    
    private void SpawnECSWithPooling(ref SystemState state)
    {
        
    }
}
