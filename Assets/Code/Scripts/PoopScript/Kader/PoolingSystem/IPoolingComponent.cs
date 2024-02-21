using Unity.Collections;
using Unity.Entities;

public struct IPoolingComponent : IComponentData
{
    public NativeArray<IPoolingEntityComponent> _gameObjects;
    //public Entity _gameObjects;
}
