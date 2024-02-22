using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGoblinTorchAnimationBehavior : MonoBehaviour
{

    public void UpdateVelocity(Vector3 velocity)
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
