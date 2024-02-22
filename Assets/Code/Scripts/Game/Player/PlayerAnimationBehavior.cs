
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Physics;
using UnityEngine;

public class PlayerAnimationBehavior : MonoBehaviour
{
    public bool IsIdle = false;

    [SerializeField] private List<Animator> childAnimator;

    public void ToggleIdle(bool isIdle)
    {
        IsIdle = isIdle;
        GetComponent<Animator>().SetBool("Idle", isIdle);

        foreach (var item in childAnimator)
        {
            item.SetBool("Idle", isIdle);
        }
    }

    public void UpdateVelocity(Vector3 velocity, bool isPhaseTwo)
    {
        if (!isPhaseTwo)
        {
            if (velocity.x > 0)
            {
                transform.DORotate(Vector3.zero, 0.2f, RotateMode.Fast);
            }

            if (velocity.x < 0)
            {
                transform.DORotate(Vector3.down * 180, 0.2f, RotateMode.Fast);
            }

        }
    }
}
