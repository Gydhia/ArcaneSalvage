using System;
using Code.Scripts.Helper;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Code.Scripts.Game.Player
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private float joyStickDeadZone = 2;
        [SerializeField] private float maxMagnitude = 1;
        [SerializeField] private bool isPhaseTwo;
        
        private InputActions _inputActions;
        
        private Vector2 _initTouchPos;
        private Vector2 _moveDirection;
        
        private bool _isTouching;
        private bool _canMove;

        public Vector2 MoveDirection => _moveDirection;
        
        public bool IsTouching => _isTouching;
        public bool CanMove => _canMove;

        public Action<Vector2> OnTouchStart;

        public bool IsPhaseTwo
        {
            get => isPhaseTwo;
            set => isPhaseTwo = value;
        }

        private void OnEnable()
        {
            _inputActions = new InputActions();
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }
        
        void Start()
        {
            _inputActions.MovePointer.Touch.started += context =>
            {
                _isTouching = true;
                _initTouchPos = _inputActions.MovePointer.Position.ReadValue<Vector2>();
                
                OnTouchStart?.Invoke(_initTouchPos);
            };

            _inputActions.MovePointer.Touch.canceled += ctx =>
            {
                _isTouching = false; 
                
            };
        }
        
        void Update()
        {
            if (_isTouching)
            {
                _canMove = (GetTouchPos() - _initTouchPos).magnitude > joyStickDeadZone;
                if (_canMove)
                {
                    if (isPhaseTwo)
                    {
                        Vector2 phaseTwoVector = Vector2.ClampMagnitude(GetTouchPos() - _initTouchPos, maxMagnitude);
                        phaseTwoVector.y = 0;
                        _moveDirection = phaseTwoVector.normalized;
                    }
                    else
                    {
                        _moveDirection = Vector2.ClampMagnitude(GetTouchPos() - _initTouchPos, maxMagnitude);
                    }
                    
                }
            }
            else
            {
                _canMove = false;
            }
        }

        private Vector2 GetTouchPos()
        {
            return _inputActions.MovePointer.Position.ReadValue<Vector2>();
        }
    }
}
