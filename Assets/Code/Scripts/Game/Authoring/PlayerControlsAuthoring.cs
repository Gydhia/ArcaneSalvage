using Unity.Entities;
using UnityEngine;

namespace Assets.Code.Scripts.Game.Player
{
    public class PlayerControlsAuthoring : MonoBehaviour
    {
        public bool IsPhaseTwo = false;
        public float JoyStickDeadZone = .1f;
        public float MaxMagnitude = 1;

        public class Baker : Baker<PlayerControlsAuthoring>
        {
            public override void Bake(PlayerControlsAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new InputVariables
                {
                    IsPhaseTwo = authoring.IsPhaseTwo,
                    JoyStickDeadZone = authoring.JoyStickDeadZone,
                    MaxMagnitude = authoring.MaxMagnitude
                });
            }
        }
    }
    
    public struct InputVariables : IComponentData
    {
        public bool IsPhaseTwo;
        public float JoyStickDeadZone;
        public float MaxMagnitude;
    }
}