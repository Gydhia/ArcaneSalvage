using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Scripts.Game.Player
{
    //[RequireComponent(typeof(Rigidbody), typeof(BoxCollider), typeof(SpriteRenderer))]
    public class PlayerBehaviour : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private bool _phaseOne = true;
        
        private Rigidbody _rigidbody;
        private InputManager _inputManager;

        private Animator playerAnimator;
        private Animator playerWeaponAnimator;

        public UnityEvent spawnProjectile;

        private void Start()
        {
            TryGetComponent(out playerAnimator);
            if(transform.childCount > 0) 
            { 
                transform.GetChild(0).TryGetComponent(out playerWeaponAnimator);
            }
            TryGetComponent(out _rigidbody);

            
            _inputManager = InputManager.Instance;
        }

        private void Update()
        {
            //Debug.Log($"Touch : {_inputManager.IsTouching} | CanMove : {_inputManager.CanMove}'{InputManager.Instance.CanMove} | MoveDirection : {_inputManager.MoveDirection.ToString()}");
            if(_phaseOne)
            {
                if (_rigidbody.velocity.magnitude > 0.01f)
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
                _rigidbody.velocity = moveDir * moveSpeed;
                if(_phaseOne)
                {
                    if (moveDir.x > 0)
                    {
                        transform.DORotate(Vector3.zero, 0.2f, RotateMode.Fast);
                        
                    }

                    if (moveDir.x < 0)
                    {
                        transform.DORotate(Vector3.down * 180, 0.2f, RotateMode.Fast);
                    }

                }
            }
            else
            {
                _rigidbody.velocity = Vector2.zero;
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
