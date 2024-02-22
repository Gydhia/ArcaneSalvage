using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial class PoolingSystem : SystemBase
{
    NativeArray<NativeArray<Entity>> allEntities;
    
    private bool isCreatingPool; 
    protected override void OnCreate()
    {
        isCreatingPool = false;
        allEntities = new NativeArray<NativeArray<Entity>>(1, Allocator.Persistent);
    }

    protected override void OnUpdate()
    {
        
        if (!isCreatingPool)
        {
            foreach (var entityPooling in SystemAPI.Query<RefRW<IPoolingEntityComponent>>())
            {
                CreatePool(entityPooling.ValueRW.entity, entityPooling.ValueRW.nbrOfEntity);
            
                /*PoolingComponentJob newPoolingComponentJob = new PoolingComponentJob()
                {
                    entity = theEntity,
                    nbrOfEntity = theNbrOfEnity,
                    entities = newEntities
                };
    
                newPoolingComponentJob.Schedule();*/

                Entity entityTestOui = GetEntityPool(entityPooling.ValueRW.entity);
                
                //EntityManager.SetSharedComponentData(entityTestOui, new Visible { Value = 0 });
                //remove the position
                EntityManager.RemoveComponent(entityTestOui, typeof(LocalToWorld));
                EntityManager.CompleteAllTrackedJobs();
                
                //EntityManager.SetComponentEnabled(entityTestOui, typeof(IPoolingComponentGeneric), false);
                //Debug.Log("GROS FILS DE PUTE DE TA GRAND MERE QUI SUCE TOUTE LES BITES DE LA TERRE !!!");
                
                /*for (int i = 0; i < allCompoenent.Length; i++)
                {
                    //Debug.Log(allCompoenent[i]);
                    

                    //EntityManager.SetComponentEnabled(entityTestOui, allCompoenent[i], false);
                }*/
                //Debug.Log(entityTestOui);

                
                //EntityManager.SetComponentEnabled<>(entityTestOui, false);
                //EntityManager.SetEnabled(entityTestOui, true);
                //Debug.Log(entityTestOui);
            }

            isCreatingPool = true;
        }
        
        
    }

    public void CreatePool(Entity entity, int nbrEntity)
    {
        NativeArray<Entity> newEntities = new NativeArray<Entity>(nbrEntity, Allocator.Persistent);
        //EntityManager.SetComponentEnabled(entity, false);
        //EntityManager.SetEnabled(entity, false);
        EntityManager.Instantiate(entity, newEntities);
        allEntities[0] = newEntities;

    }
    
    public void Pool()
    {
        
    }

    public Entity GetEntityPool(Entity entity)
    {
        Entity aEntity = allEntities[0][0];
        
        return aEntity;
    }
    
    [BurstCompile, WithAll(typeof(IPoolingComponent))]
    public partial struct PoolingComponentJob : IJobEntity
    {
        public Entity entity;
        public int nbrOfEntity;

        public NativeArray<Entity> entities;
        public void Execute(in IPoolingEntityComponent poolingComponent)
        {
            Debug.Log(entities);
        }
    }
    
}

