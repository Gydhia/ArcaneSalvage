using Code.Scripts.Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class PhasePortal : MonoBehaviour
{
    [SerializeField] private float _blackFadeDuration;
    [SerializeField] private float _cameraTransitionDuration;
    bool isEntered = false;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isEntered)
        {
            isEntered = true;
            StartCoroutine(StartEndAnimationCoroutine());
        }
    }
    private IEnumerator StartEndAnimationCoroutine()
    {
        if(_blackFadeDuration >_cameraTransitionDuration)
        {
            _blackFadeDuration = _cameraTransitionDuration;
        }
        StartCoroutine(GameManager.Instance.CameraControllerRef.ChangeOrthoSizeCoroutine(1.0f, _cameraTransitionDuration));
        GameManager.Instance.InputManagerRef.IsActive = false;
        yield return new WaitForSeconds(_cameraTransitionDuration - _blackFadeDuration);
        yield return StartCoroutine(GameManager.Instance.UISceneTransitionManagerRef.TransitionFadeIn(_blackFadeDuration));
        GameManager.Instance.PlayerEnterPortal();


    }
}
