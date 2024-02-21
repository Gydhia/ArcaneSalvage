using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct InputComponent : IComponentData
{
    public bool pressing1;
    public bool pressing2;
    public bool pressing3;
}
