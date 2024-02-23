using Code.Scripts.Game.Player;
using Code.Scripts.Helper;
using System.Collections.Generic;
using ArcanaSalvage;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    // Player reference
    public PlayerBehaviour PlayerRef { get; private set; }
    public SceneController SceneControllerRef { get; private set; }
    public InputManager InputManagerRef { get; private set; }
    public CameraController CameraControllerRef { get; private set; }
    public UISceneTransitionManager UISceneTransitionManagerRef { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        PlayerData.CurrentPlayerData = new PlayerData();
    }
    
    // Gameloop
    private void Start()
    {
        SetupGameLoop();
        PlayerRef = FindObjectOfType<PlayerBehaviour>();
        SceneControllerRef = FindObjectOfType<SceneController>();
        CameraControllerRef = FindObjectOfType<CameraController>();
        UISceneTransitionManagerRef = FindObjectOfType<UISceneTransitionManager>();
        InputManagerRef = FindObjectOfType<InputManager>();
        
        PlayerData.CurrentPlayerData.GetHealthOverride();
    }

    #region GameLoop
    [Header("Phase 1")]
    [SerializeField] private List<KeyFragment> _keyFragments;
    [SerializeField] private PhasePortal _phasePortal;
    private KeyFragment _keyFragment = null;

    [Header("Phase 2")]
    [SerializeField] private GameObject Boss;

    private void SetupGameLoop()
    {
        if(AddListenerToKeys())
        {
            SetKeyFragmentActive(_keyFragments[0]);
            _phasePortal.gameObject.SetActive(false);
        }
        
    }

    private void OpenPortal()
    {
        _phasePortal.gameObject.SetActive(true);
    }

    private bool AddListenerToKeys()
    {
        if(_keyFragments.Count == 0) { return false; }

        foreach (var keyFragment in _keyFragments)
        {
            keyFragment.KeyFragmentFoundDelegate += KeyFound;
            keyFragment.gameObject.SetActive(false);
        }
        return true;
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
        SceneControllerRef.LoadScene("Alexis-SubDev");
        GetNewSceneComponent();
        GameManager.Instance.InputManagerRef.IsActive = true;
        InputManagerRef.IsPhaseTwo = true;
        StartCoroutine(UISceneTransitionManagerRef.TransitionFadeOut(0.2f));
    }

    private void GetNewSceneComponent()
    {
        PlayerRef = FindObjectOfType<PlayerBehaviour>();
        CameraControllerRef = FindObjectOfType<CameraController>();
        UISceneTransitionManagerRef = FindObjectOfType<UISceneTransitionManager>();
    }

    #endregion
}
