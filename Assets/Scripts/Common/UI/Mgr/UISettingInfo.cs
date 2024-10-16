using System;

namespace TTT
{
    public struct UISettingInfo
    {
        public Type viewType;
        public string prefabBundleName;
        public bool isFullScreenView;

        public UISettingInfo(Type viewType, string prefabBundleName, bool isFullScreenView = false)
        {
            this.viewType = viewType;
            this.prefabBundleName = prefabBundleName;
            this.isFullScreenView = isFullScreenView;
        }
    }
}