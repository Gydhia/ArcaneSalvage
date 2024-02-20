using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Game.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D), typeof(SpriteRenderer))]
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private bool _phaseOne = true;
        
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRendererWeapon;
        private InputManager _inputManager;

        private Animator playerAnimator;
        private Animator playerWeaponAnimator;

        public UnityEvent spawnProjectile;

        private void Start()
        {
            TryGetComponent(out playerAnimator);
            if(transform.childCount > 0) 
            { 
                transform.GetChild(0).TryGetComponent(out _spriteRendererWeapon);
                transform.GetChild(0).TryGetComponent(out playerWeaponAnimator);
            }
            TryGetComponent(out _rigidbody2D);

            
            _inputManager = InputManager.Instance;
        }

        private void Update()
        {
            //Debug.Log($"Touch : {_inputManager.IsTouching} | CanMove : {_inputManager.CanMove}'{InputManager.Instance.CanMove} | MoveDirection : {_inputManager.MoveDirection.ToString()}");
            if(_phaseOne)
            {
                if (_rigidbody2D.velocity.magnitude > 0.01f)
                {
                    playerAnimator.SetBool("Idle",false);
                }
                else
                {
                    playerAnimator.SetBool("Idle",true);
                }
            }
            
            if (InputManager.Instance.CanMove)
            {
                Vector2 moveDir = _inputManager.MoveDirection;
                _rigidbody2D.velocity = moveDir * moveSpeed;
                if(_phaseOne)
                {
                    if (moveDir.x > 0)
                    {
                        transform.DORotate(Vector3.zero, 0.2f, RotateMode.Fast);
                        _spriteRendererWeapon.sortingOrder = 1;
                    }

                    if (moveDir.x < 0)
                    {
                        transform.DORotate(Vector3.down * 180, 0.2f, RotateMode.Fast);
                        _spriteRendererWeapon.sortingOrder = -1;
                    }

                }
            }
            else
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
        }
        void OnDestroy()
        {
            DOTween.Kill(transform);
            DOTween.Kill(gameObject);
            foreach (Transform child in transform)
            {
                DOTween.Kill(child);
            }
        }
    }
}
