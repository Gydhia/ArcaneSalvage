using System;
using Unity.Entities.UniversalDelegates;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Scripts.Game.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private InputManager _inputManager;

        private void Start()
        {
            TryGetComponent(out _spriteRenderer);
            TryGetComponent(out _rigidbody2D);
            _inputManager = InputManager.Instance;
        }

        private void Update()
        {
            //Debug.Log($"Touch : {_inputManager.IsTouching} | CanMove : {_inputManager.CanMove}'{InputManager.Instance.CanMove} | MoveDirection : {_inputManager.MoveDirection.ToString()}");

            if (InputManager.Instance.CanMove)
            {
                Debug.Log($"Moving | {InputManager.Instance.CanMove}");
                Vector2 moveDir = _inputManager.MoveDirection;
                _rigidbody2D.velocity = moveDir * moveSpeed;
                if (moveDir.x > 0)
                {
                    _spriteRenderer.flipX = false;
                }

                if (moveDir.x < 0)
                {
                    _spriteRenderer.flipX = true;
                }
            }
            else
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
        }
    }
}
