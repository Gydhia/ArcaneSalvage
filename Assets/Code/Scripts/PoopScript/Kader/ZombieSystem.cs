using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEngine;
using Component = System.ComponentModel.Component;

public partial class ZombieSystem : SystemBase
{
    
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        foreach (var entityPooling in SystemAPI.Query<RefRW<IZombieComponent>>())
        {
                
        }
    }

    [BurstCompile, WithAll(typeof(IPoolingComponent))]
    public partial struct PoolingComponentJob : IJobEntity
    {
        public void Execute(in IZombieComponent poolingComponent)
        {
            
        }
    }
    
}

