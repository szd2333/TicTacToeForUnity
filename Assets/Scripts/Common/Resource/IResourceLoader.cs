using System;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public interface IResourceLoader
    {
        public void LoadAssetAsync(string assetBundleName, string assetName, Type type, Action<UnityEngine.Object, bool> func);
    }
}