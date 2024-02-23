//using System.Collections;
//using NaughtyAttributes;
//using Unity.Entities;
//using Unity.Scenes;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.SceneManagement;
//using Hash128 = Unity.Entities.Hash128;

//public class SubSceneLoader : MonoBehaviour
//{
//    [SerializeField] private SubScene _subScene;

//    private Hash128 subSceneID;
    
//    // Start is called before the first frame update
//    void Start()
//    {
//        subSceneID = _subScene.SceneGUID;
//        SceneSystem.LoadSceneAsync(World.DefaultGameObjectInjectionWorld.Unmanaged, subSceneID);
//    }

//    [Button("UnloadScene")]
//    public void UnloadScene()
//    {
//        SceneSystem.UnloadScene(World.DefaultGameObjectInjectionWorld.Unmanaged, subSceneID);

//        StartCoroutine(CheckForUnload(ChangeScene));

//    }

//    private IEnumerator CheckForUnload(UnityAction callback)
//    {
//        while (_subScene.IsLoaded)
//        {
//            yield return new WaitForEndOfFrame();
//        }
        
//        callback?.Invoke();
//    }

//    private void ChangeScene()
//    {
//        SceneManager.LoadScene(0);
//    }
//}
