using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneTransitionManager : MonoBehaviour
{
    [SerializeField] private Image _transitionPanel;

    public IEnumerator TransitionFadeIn(float duration)
    {
        float elapsedTime = 0;
        float alpha = 0;
        while (elapsedTime / duration < 1)
        {

            alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            _transitionPanel.color = new Color(_transitionPanel.color.r, _transitionPanel.color.g, _transitionPanel.color.b, alpha);
            yield return null;
        }
    }

    public IEnumerator TransitionFadeOut(float duration) 
    {
        float elapsedTime = 0;
        float alpha = 0;
        while (elapsedTime / duration < 1)
        {
            alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            _transitionPanel.color = new Color(_transitionPanel.color.r, _transitionPanel.color.g, _transitionPanel.color.b, alpha);
            yield return null;
        }
        _transitionPanel.color = new Color(_transitionPanel.color.r, _transitionPanel.color.g, _transitionPanel.color.b, 0);

    }

}
