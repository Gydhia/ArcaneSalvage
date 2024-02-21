using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct GameManagerComponent : IComponentData
{
    public int nbrEntity;
    public Entity EntityECS;
}

