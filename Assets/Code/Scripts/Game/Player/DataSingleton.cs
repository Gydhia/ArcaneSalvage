using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Code.Scripts.Game.Player
{
    public struct DataSingleton : IComponentData
    {
        public float2 InitTouchPos;
        public float2 MoveDirection;
        public float3 PlayerPosition;
        public bool Touch;
        public bool CanMove;
        public int KeyNumber;
    }
}