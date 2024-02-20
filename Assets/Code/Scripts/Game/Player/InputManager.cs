using Code.Scripts.Helper;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Scripts.Game.Player
{
    public class InputManager : Singleton<InputManager>
    {
        [SerializeField] private float joyStickDeadZone = 2;
        [SerializeField] private float maxMagnitude = 1;
        
        private InputActions _inputActions;
        
        private Vector2 _initTouchPos;
        private Vector2 _moveDirection;
        
        private bool _isTouching;
        private bool _isPhaseTwo;
        private bool _canMove;

        public Vector2 MoveDirection => _moveDirection;
        
        public bool IsTouching => _isTouching;
        public bool CanMove => _canMove;

        public bool IsPhaseTwo
        {
            get => _isPhaseTwo;
            set => _isPhaseTwo = value;
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
                    _moveDirection = Vector2.ClampMagnitude(GetTouchPos() - _initTouchPos, maxMagnitude);
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
