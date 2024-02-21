using System;
using System.Collections;
using System.Collections.Generic;
using Code.Scripts.Game.Player;
using UnityEngine;

public class AnimationShootRelay : MonoBehaviour
{
    private PlayerBehaviour _playerBehaviour;

    private void Start()
    {
        transform.parent.TryGetComponent(out _playerBehaviour);
    }

    public void SpawnProjectile()
    {
           _playerBehaviour.spawnProjectile.Invoke();
    }
}
