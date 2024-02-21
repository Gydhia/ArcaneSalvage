using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcanaSalvage
{
    public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;
        public static T Instance => _instance ??= Resources.Load<T>(typeof(T).Name);
    }
}