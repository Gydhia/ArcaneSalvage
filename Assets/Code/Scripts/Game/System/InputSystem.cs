using System.Linq;
using System.Threading.Tasks;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Code.Scripts.Game.Player
{
    public partial class InputSystem : SystemBase
    {
        private InputActions _inputActions;

        private DataSingleton _inputComponent;
        
        protected override void OnCreate()
        {
            if (!SystemAPI.TryGetSingleton<DataSingleton>(out DataSingleton _))
            {
                EntityManager.CreateEntity(typeof(DataSingleton));
            }

            _inputActions = new InputActions();
            _inputActions.Enable();

            _inputComponent = new DataSingleton();
            
            _inputActions.MovePointer.Touch.started += async context =>
            {
                _inputComponent.Touch = true;
                await InputWait();
                _inputComponent.InitTouchPos = _inputActions.MovePointer.Position.ReadValue<Vector2>();
            };

            _inputActions.MovePointer.Touch.canceled += ctx =>
            {
                _inputComponent.Touch = false;
            };
            
        }

        private async Task InputWait()
        {
            await Task.Delay(10);
        }

        protected override void OnUpdate()
        {
            InputVariables inputVariables = new InputVariables();
            foreach (var inputVar in SystemAPI.Query<RefRO<InputVariables>>())
            {
                inputVariables = inputVar.ValueRO;
            }
     
            SystemAPI.TryGetSingletonEntity<InputVariables>(out var playerEntity);
            var localToWorld = SystemAPI.GetComponent<LocalToWorld>(playerEntity);
            
            _inputComponent.PlayerPosition = localToWorld.Position;
            
            if (_inputComponent.Touch)
            {
                _inputComponent.CanMove = (GetTouchPos() - Convert(_inputComponent.InitTouchPos)).magnitude > inputVariables.JoyStickDeadZone;
                if (_inputComponent.CanMove)
                {
                    if (inputVariables.IsPhaseTwo)
                    {
                        Vector2 phaseTwoVector = Vector2.ClampMagnitude(GetTouchPos() - Convert(_inputComponent.InitTouchPos), inputVariables.MaxMagnitude);
                        phaseTwoVector.y = 0;
                        _inputComponent.MoveDirection = phaseTwoVector.normalized;
                    }
                    else
                    {
                        _inputComponent.MoveDirection = Vector2.ClampMagnitude(GetTouchPos() - Convert(_inputComponent.InitTouchPos), inputVariables.MaxMagnitude);
                    }
                    
                }
            }
            else
            {
                _inputComponent.CanMove = false;
            }
            
            SystemAPI.SetSingleton(_inputComponent);
        }
        
        private Vector2 GetTouchPos()
        {
            return _inputActions.MovePointer.Position.ReadValue<Vector2>();
        }

        private Vector2 Convert(float2 f)
        {
            return new Vector2(f.x, f.y);
        }

        protected override void OnDestroy()
        {
            _inputActions.Disable();
        }
    }
}