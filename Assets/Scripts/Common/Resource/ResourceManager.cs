using System;
using UnityEngine;
using Object = System.Object;

namespace TTT
{
    public class ResourceManager : SingletonBase<ResourceManager>
    {
        private IResourceLoader _resourceLoad;

        private static IResourceLoader ResourceLoad
        {
            get
            {
                if (Instance._resourceLoad == null)
                {
#if UNITY_EDITOR
                    //临时处理，只实现Editor下资源加载
                    Instance._resourceLoad = EditorResourceLoadManager.Instance;
#endif
                }
                return Instance._resourceLoad;
            }
        }

        public static void LoadAssetAsync(string assetBundleName, string assetName, Type type, Action<UnityEngine.Object, bool> func)
        {
            ResourceLoad.LoadAssetAsync(assetBundleName, assetName, type, func);
        }
    }
}