#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace TTT
{
    public class EditorResourceLoadManager : SingletonBase<EditorResourceLoadManager>, IResourceLoader
    {

        public void LoadAssetAsync(string assetBundleName, string assetName, Type type, Action<UObject, bool> func)
        {
            if (func == null)
            {
                return;
            }

            string[] assetPaths =
                AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName(assetBundleName, assetName);
            if (assetPaths.Length <= 0)
            {
                func.Invoke(null, false);
                return;
            }
            var asset = AssetDatabase.LoadAssetAtPath(assetPaths[0], type);
            bool isSuccess = asset != null;
            func.Invoke(asset, isSuccess);
        }
    }
}
#endif