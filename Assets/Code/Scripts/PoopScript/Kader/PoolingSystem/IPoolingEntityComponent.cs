using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[InternalBufferCapacity(5)]
public struct IPoolingEntityComponent : IComponentData
{
    public int nbrOfEntity;
    public Entity entity;
}
