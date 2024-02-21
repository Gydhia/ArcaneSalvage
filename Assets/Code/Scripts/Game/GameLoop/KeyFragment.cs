using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class KeyFragment : MonoBehaviour
{
    public delegate void KeyFragmentFound(KeyFragment keyFragment);
    public KeyFragmentFound KeyFragmentFoundDelegate;

    bool isCollected = false;
    bool isAnimationCompleted = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            isCollected = true;
            StartCoroutine(StartAnimation());
        }
    }

    private IEnumerator StartAnimation()
    {
       StartCoroutine(Animate());
        while (isAnimationCompleted)
        {
            yield return new WaitForSeconds(0.1f);
        }
        KeyFragmentFoundDelegate.Invoke(this);
    }

    private IEnumerator Animate()
    {
        yield return null;
    }
}
