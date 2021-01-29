using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Managers
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
#if UNITY_EDITOR
                    //fix Error on stop Play mode
                    if (!EditorApplication.isPlayingOrWillChangePlaymode)
                        return null;
#endif
                    var obj = new GameObject($"[{typeof(T).Name}]");
                    Debug.Log("Singleton instantiating " + typeof(T).Name);
                    _instance = obj.AddComponent<T>();
                }

                return _instance;
            }
        }

        public void Awake()
        {
            if (_instance == null)
            {
                Debug.Log("Singleton Awake " + typeof(T).Name);
                _instance = GetComponent<T>();
            }
            else
            {
                Debug.Log("Singleton Destroing Duplicate " + typeof(T).Name);
                Destroy(gameObject);
            }
        }
    }
}