using UnityEngine;
 
 namespace Code.Scripts.Helper
{
    public class Singleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        private void Awake()
        {
            if(_instance != null && _instance.gameObject != gameObject)
            {
                Destroy(gameObject);
                return;
            }

            _instance = GetComponent<T>();
            DontDestroyOnLoad(_instance);
        }

        public static T Instance
        {
            get
            {
                if(_instance == null)
                    _instance = new GameObject(typeof(T).Name + " " + nameof(Singleton<T>)).AddComponent<T>();

                return _instance;
            }
            private set => _instance = value;
        }
        private static T _instance;

        private void OnDestroy()
        {
            if(_instance.gameObject == gameObject)
                _instance = null;
        }
    }
}