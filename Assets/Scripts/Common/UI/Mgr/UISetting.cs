using System;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class UISetting : SingletonBase<UISetting>
    {
        private Dictionary<string, UISettingInfo> viewSettingInfoDict = new Dictionary<string, UISettingInfo>();

        public static void Init()
        {
            _RegisterAllViews();
        }

        /// <summary>
        ///  尝试获取界面设置信息
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="result"></param>
        /// <returns>是否获取成功</returns>
        public static bool TryGetViewSettingInfo(string viewName, out UISettingInfo result)
        {
            if (string.IsNullOrEmpty(viewName) || !Instance.viewSettingInfoDict.ContainsKey(viewName))
            {
                result = default;
                return false;
            }
            result = Instance.viewSettingInfoDict[viewName];
            return true;
        }
        
        public static Type GetViewType(string viewName)
        {
            UISettingInfo result;
            if (!TryGetViewSettingInfo(viewName, out result))
            {
                return null;
            }
            return result.viewType;
        }
        
        public static string GetViewPrefabBundleName(string viewName)
        {
            UISettingInfo result;
            if (!TryGetViewSettingInfo(viewName, out result))
            {
                return default;
            }
            return result.prefabBundleName;
        }
        
        public static bool IsFullScreenView(string viewName)
        {
            UISettingInfo result;
            if (!TryGetViewSettingInfo(viewName, out result))
            {
                return false;
            }
            return result.isFullScreenView;
        }

        private static void _RegisterAllViews()
        {
            _RegisterView(typeof(Test.TestView), "prefabs/services/test/ui");
            _RegisterView(typeof(TicTacToeGame.TicTacToeGameView), "prefabs/services/tictactoegame/ui", true);
        }

        private static void _RegisterView(Type viewType, string prefabBundleName, bool isFullScreenView = false)
        {
            if (viewType == null || !viewType.IsSubclassOf(typeof(ViewBase)))
            {
                Debug.LogError("注册界面类型错误");
                return;
            }

            string viewName = viewType.Name;
            if (Instance.viewSettingInfoDict.ContainsKey(viewName))
            {
                Debug.LogError($"注册界面名重复, viewName:{viewName}");
                return;
            }
            Instance.viewSettingInfoDict[viewName] = new UISettingInfo(viewType, prefabBundleName, isFullScreenView);
        }
    }
}