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
        [SerializeField] private bool _isActive = true;
        
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

        public bool IsActive
        {
            get => _isActive;
            set => _isActive = value;
        }

        private void OnEnable()
        {
            _inputActions = new InputActions();
            _inputActions.Enable();
        }

        private void OnDisable()
        {
            if(_inputActions != null)
            {
                _inputActions.Disable();
            }
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
                Debug.Log("test");
            };
        }
        
        void Update()
        {
            if(!_isActive) 
            {
                _moveDirection = Vector2.zero;
                return; 
            }
            if (_isTouching)
            {
                if(isPhaseTwo)
                {
                    Vector2 pos = GetTouchPos();
                    _canMove = pos != Vector2.zero;

                    if(pos.x < Screen.width /2)
                    {
                        _moveDirection = new Vector2(-maxMagnitude, 0);
                    }
                    else
                    {
                        _moveDirection = new Vector2(maxMagnitude, 0);
                    }
                    return;
                }
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

        public void SetActive(bool value)
        {

        }
    }
}
