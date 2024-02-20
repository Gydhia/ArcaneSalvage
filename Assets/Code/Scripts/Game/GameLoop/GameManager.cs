using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    // Player reference



    // Gameloop
    private void Start()
    {
        // Get Player Ref;

        SetupGameLoop();
    }

    #region GameLoop
    [SerializeField] private List<KeyFragment> _keyFragments;
    [SerializeField] private PhasePortal _phasePortal;

    private KeyFragment _keyFragment = null;

    private void SetupGameLoop()
    {
        AddListenerToKeys();
        SetKeyFragmentActive(_keyFragments[0]);
        _phasePortal.gameObject.SetActive(false);
    }

    private void OpenPortal()
    {
        _phasePortal.gameObject.SetActive(true);
    }

    private void AddListenerToKeys()
    {
        foreach (var keyFragment in _keyFragments)
        {
            keyFragment.KeyFragmentFoundDelegate += KeyFound;
            keyFragment.gameObject.SetActive(false);
        }
    }

    private void SetKeyFragmentActive(KeyFragment keyFragment)
    {
        if(_keyFragment != null)
        {
            SetKeyFragmentInactive();
        }
        _keyFragment = keyFragment;
        _keyFragment.gameObject.SetActive(true);
    }
    private void SetKeyFragmentInactive()
    {
        _keyFragment.gameObject.SetActive(false);
        _keyFragment = null;
    }


    private void KeyFound(KeyFragment keyFragment)
    {
        if(_keyFragments.Contains(keyFragment))
        {
            int index = _keyFragments.IndexOf(keyFragment);
            if(index == _keyFragments.Count - 1)
            {
                SetKeyFragmentInactive();
                OpenPortal();
            }
            else
            {
                SetKeyFragmentActive(_keyFragments[index + 1]);
            }
        }
    }

    public void PlayerEnterPortal()
    {
        // Load scene
    }

    #endregion
}
