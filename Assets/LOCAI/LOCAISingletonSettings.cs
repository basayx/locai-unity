using UnityEditor;
using UnityEngine;

namespace LOCAI
{
    public abstract class LOCAISingletonSettings<T> : ScriptableObject where T : LOCAISingletonSettings<T>
    {
        private static volatile T _instance;

        private static object LockObj;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (LockObj ??= new object())
                {
                    if (_instance != null)
                        return _instance;

                    _instance = (T)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets($"t: {typeof(T)}")[0]), typeof(T));
                }

                return _instance;
            }
        }
    }
}