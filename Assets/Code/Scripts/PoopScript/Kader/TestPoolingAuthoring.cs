using Unity.Entities;
using UnityEngine;

public class TestPoolingAuthoring : MonoBehaviour
{
    public class BakerScript : Baker<TestPoolingAuthoring>
    {
        public override void Bake(TestPoolingAuthoring poolingManager)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new TestPoolingComponent()
            {
                key = 2
            });
        }
    }
}

public struct TestPoolingComponent : IComponentData
{
    public int key;
}


public partial class TestSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //Debug.Log("caca");
        Debug.Log(PoolingAuthoring.Instance.GetObject(PoolingType.Ennemy));
    }
}