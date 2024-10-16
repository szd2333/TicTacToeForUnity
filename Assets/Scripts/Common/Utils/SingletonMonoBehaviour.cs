using UnityEngine;

namespace TTT
{
    /// <summary>
    /// 单例创建器
    /// </summary>
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static GameObject gObj = null;
        private static T _instance;
        private static readonly object syslock = new object();
        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                
                lock (syslock)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        if (gObj == null)
                        {
                            gObj = new GameObject(typeof(T).ToString());
                            DontDestroyOnLoad(gObj);
                        }
                        _instance = gObj.AddComponent<T>();
                    }
                    else
                    {
                        gObj = _instance.gameObject;
                        DontDestroyOnLoad(gObj);
                    }
                    return _instance;
                }
            }
        }

        public static GameObject GameObj
        {
            get => gObj;
        }

    }
}

