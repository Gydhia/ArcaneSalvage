using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

[Serializable]
public class ShakeEffect
{
    public AnimationCurve Curve;
    public float _shakeIntensity;
    public float _shakeDuration;

}

public class CameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _virtualCamera;
    [SerializeField] ShakeEffect _shakeEffect;

    private float _timer;
    private CinemachineBasicMultiChannelPerlin _cbmcp;

    private void Awake()
    {
        _virtualCamera = GetComponent<CinemachineVirtualCamera>();
        
    }

    public void StartShake()
    {
        StopAllCoroutines();
        _cbmcp = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = _shakeEffect._shakeIntensity;

        StartCoroutine(ShakeAnimation(_shakeEffect));
    }

    public void StartShake(ShakeEffect shake)
    {

        _cbmcp = _virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _cbmcp.m_AmplitudeGain = _shakeEffect._shakeIntensity;

        StartCoroutine(ShakeAnimation(shake));
    }


    public IEnumerator ShakeAnimation(ShakeEffect shake)
    {
        float timeElapsed = 0;
        while(timeElapsed/shake._shakeDuration < 1)
        {
            _cbmcp.m_AmplitudeGain = Mathf.Lerp(shake._shakeIntensity, 0, timeElapsed / shake._shakeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;

        }
    }

    public IEnumerator ChangeOrthoSizeCoroutine(float newOrthoSize, float duration)
    {
        float elapsedTime = 0;
        float startValue = _virtualCamera.m_Lens.OrthographicSize;
        while (elapsedTime / duration < 1)
        {
            _virtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(startValue, newOrthoSize, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) 
        {
            Debug.Log("space");
            StartShake();
        }
    }

}
