using System;
using UnityEngine;

namespace TTT.TicTacToeGame
{
    public class TicTacToeGameSaveMgr : SingletonBase<TicTacToeGameSaveMgr>
    {
        private OperateControllerType _xPiecesControllerType = OperateControllerType.UIClick;
        private OperateControllerType _operateControllerType = OperateControllerType.UIClick;
        
        private static readonly string XPiecesControllerTypePrefsKey = "XPiecesControllerTypePrefsKey";
        private static readonly string OPiecesControllerTypePrefsKey = "OPiecesControllerTypePrefsKey";

        public static void Init()
        {
            Instance._xPiecesControllerType = _GetOperateControllerType(XPiecesControllerTypePrefsKey, OperateControllerType.UIClick);
            Instance._operateControllerType = _GetOperateControllerType(OPiecesControllerTypePrefsKey, OperateControllerType.AIMiniMax);
        }

        public static OperateControllerType GetOperateControllerType(TicTacToePiecesType piecesType)
        {
            var isX = piecesType == TicTacToePiecesType.X;
            return isX ? Instance._xPiecesControllerType : Instance._operateControllerType;
        }

        public static void SetOperateControllerType(TicTacToePiecesType piecesType, OperateControllerType ctrlType)
        {
            var curCtrlType = GetOperateControllerType(piecesType);
            if (curCtrlType == ctrlType)
            {
                return;
            }
            var isX = piecesType == TicTacToePiecesType.X;
            if (isX)
            {
                Instance._xPiecesControllerType = ctrlType;
            }
            else
            {
                Instance._operateControllerType = ctrlType;
            }
            string prefsKey = isX ? XPiecesControllerTypePrefsKey : OPiecesControllerTypePrefsKey;
            _SetOperateControllerType(prefsKey, ctrlType);
            TicTacToeGameService.OnPlayerSettingChangeEvent.Invoke(piecesType);
        }

        private static OperateControllerType _GetOperateControllerType(string prefsKey, OperateControllerType defaultType)
        {
            int typeInt = PlayerPrefs.GetInt(prefsKey, Convert.ToInt32(defaultType));
            if (!Enum.IsDefined(typeof(OperateControllerType), typeInt))
            {
                return defaultType;
            }
            OperateControllerType yourEnum = (OperateControllerType)Enum.Parse(typeof(OperateControllerType), typeInt.ToString());
            return yourEnum;
        }
        
        private static void _SetOperateControllerType(string prefsKey, OperateControllerType ctrlType)
        {
            PlayerPrefs.SetInt(prefsKey, Convert.ToInt32(ctrlType));
        }

    }
}