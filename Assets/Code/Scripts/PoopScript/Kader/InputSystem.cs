using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public partial class InputKaderSystem : SystemBase
{
    private Playermove controls;
    
    protected override void OnCreate()
    {
        if (!SystemAPI.TryGetSingleton<InputComponent_Old>(out InputComponent_Old input))
        {
            EntityManager.CreateEntity(typeof(InputComponent_Old));
        }

        controls = new Playermove();
        controls.Enable();
    }

    protected override void OnUpdate()
    {
        bool isPressing1 = controls.ActionMap.SpawnGO.ReadValue<float>() == 1 ? true : false;
        bool isPressing2 = controls.ActionMap.SpawnECS.ReadValue<float>() == 1 ? true : false;
        bool isPressing3 = controls.ActionMap.SpawnECSWithPooling.ReadValue<float>() == 1 ? true : false;
        
        SystemAPI.SetSingleton(new InputComponent_Old{pressing1 = isPressing1, pressing2 = isPressing2, pressing3 = isPressing3});
    }
}
