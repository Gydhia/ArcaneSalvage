using Code.Scripts.Game.Player;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class KeyFragment : MonoBehaviour
{
    public delegate void KeyFragmentFound(KeyFragment keyFragment);
    public KeyFragmentFound KeyFragmentFoundDelegate;
    [SerializeField] private GameObject _particlePrefab;
    [SerializeField] private GameObject _keyLight;
    [SerializeField] private float _followDuration;
    [SerializeField] private float _followSpeed;
    [SerializeField] private Vector3 _offsetFollow;
    [SerializeField] private Vector3 _offsetMovement;

    bool isCollected = false;
    bool isAnimationCompleted = false;

    private void Start()
    {
        DOTween.SetTweensCapacity(2000, 100);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isCollected = true;
            StartCoroutine(StartAnimation(collision.GetComponent<PlayerBehaviour>()));
        }
    }

    private IEnumerator StartAnimation(PlayerBehaviour player)
    {
       StartCoroutine(Animate(player));
        while (!isAnimationCompleted)
        {
            yield return new WaitForSeconds(0.1f);
        }
        KeyFragmentFoundDelegate.Invoke(this);
    }

    private IEnumerator Animate(PlayerBehaviour player)
    {
        DOTween.KillAll();
        transform.DOMove(transform.position + _offsetMovement, 0.3f).SetEase(Ease.Linear);
        SpriteRenderer keyLight = _keyLight.GetComponent<SpriteRenderer>();
        keyLight.DOColor(new Color(keyLight.color.r, keyLight.color.g, keyLight.color.b, 0.2f), 0.3f).SetEase(Ease.InOutBack);

        yield return new WaitForSeconds(0.3f);
        DOTween.KillAll();
        float timeRemain;
        timeRemain = _followDuration;
        while (timeRemain > 0f)
        {

            transform.DOMove(player.transform.position + _offsetFollow, _followSpeed).SetEase(Ease.Linear);
            timeRemain -= Time.deltaTime;
            yield return null;
        }
        DOTween.KillAll();

        Instantiate(_particlePrefab, transform.position, Quaternion.identity);
        isAnimationCompleted = true;
        yield return null;
    }
}
