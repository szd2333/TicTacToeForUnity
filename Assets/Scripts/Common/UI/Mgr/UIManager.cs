using System;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class UIManager : SingletonMonoBehaviour<UIManager>
    {
        private Transform _uiRoot;
        private Dictionary<UIRootType, Transform> _uiRootDict = new Dictionary<UIRootType, Transform>();
        private Dictionary<string, ViewBase> _oepnFinishViewDict = new Dictionary<string, ViewBase>();
        private HashSet<string> _openingViewHashSet = new HashSet<string>();

        #region get 方法

        private static Transform UIRoot
        {
            get
            {
                if (!goutil.IsNil(Instance._uiRoot))
                {
                    return Instance._uiRoot;
                }
                if (goutil.IsNil(GameObj))
                {
                    return null;
                }
                Transform thisTrans = GameObj.transform;
                if (thisTrans == null)
                {
                    return null;
                }
                Instance._uiRoot = thisTrans.Find("UIROOT");
                return Instance._uiRoot;
            }
        }

        #endregion
        
        public static void Init()
        {
            UISetting.Init();
        }

        public static void Open(string viewName, object[] initParams = null)
        {
            if (IsOpen(viewName))
            {
                return;
            }
            string bundleName = UISetting.GetViewPrefabBundleName(viewName);
            Instance._openingViewHashSet.Add(viewName);
            Action<UnityEngine.Object, bool> loadCallback = (obj, isSuccess) =>
            {
                if (obj == null || !isSuccess)
                {
                    Close(viewName);
                    return;
                }
                GameObject prefab = obj as GameObject;
                _LoadViewFinsh(viewName, prefab, initParams);
            };
            ResourceManager.LoadAssetAsync(bundleName, viewName, typeof(GameObject), loadCallback);
        }

        public static void Close(string viewName)
        {
            if (!IsOpen(viewName))
            {
                return;
            }
            
        }

        public static bool IsOpen(string viewName)
        {
            if (Instance._oepnFinishViewDict.ContainsKey(viewName))
            {
                return true;
            }
            if (Instance._openingViewHashSet.Contains(viewName))
            {
                return true;
            }
            return false;
        }
        
        private static void _LoadViewFinsh(string viewName, GameObject prefab, object[] initParams)
        {
            Type viewType = UISetting.GetViewType(viewName);
            if (viewType == null)
            {
                Close(viewName);
                return;
            }
            GameObject viewGO = Instantiate(prefab);
            ViewBase viewBase = viewGO.GetComponent(viewType) as ViewBase;
            if (viewBase == null)
            {
                Destroy(viewGO);
                Close(viewName);
                return;
            }

            UIRootType rootType = viewBase.RootType;
            Transform uiRoot = _GetUIRootByType(rootType);
            if (goutil.IsNil(uiRoot))
            {
                Destroy(viewGO);
                Close(viewName);
                return;
            }

            var viewTrans = viewGO.transform;
            viewTrans.SetParent(uiRoot);
            viewTrans.localPosition = Vector3.zero;
            
            Type viewModelType = viewBase.ViewModelType;
            if (viewModelType == null)
            {
                Destroy(viewGO);
                Close(viewName);
                return;
            }

            Instance._openingViewHashSet.Remove(viewName);
            Instance._oepnFinishViewDict[viewName] = viewBase;
            
            object viewModelObj = Activator.CreateInstance(viewModelType);
            ViewModelBase viewModelBase = viewModelObj as ViewModelBase;
            viewBase.Open(initParams, viewModelBase);
        }

        private static Transform _GetUIRootByType(UIRootType type)
        {
            if (Instance._uiRootDict.ContainsKey(type))
            {
                return Instance._uiRootDict[type];
            }
            
            var uiRoot = UIRoot;
            if (uiRoot == null)
            {
                return null;
            }

            string rootName = Enum.GetName(typeof(UIRootType), type);
            var typeRoot = uiRoot.Find(rootName);
            return typeRoot;
        }
    }
}