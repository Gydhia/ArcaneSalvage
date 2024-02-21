using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    [SerializeField] private float _floatingHeight;
    [SerializeField] private float _floatingSpeed;

    private void Start()
    {
        transform.DOMoveY(transform.position.y + _floatingHeight, _floatingSpeed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

}
