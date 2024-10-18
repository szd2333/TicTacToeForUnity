namespace TTT.TicTacToeGame
{
    public class TicTacToePlayerSettingViewModel : ViewModelBase
    {
        public Observer manModeSelectGoProperty = new Observer(false);
        public Observer machineModeSelectGoProperty = new Observer(false);

        private TicTacToePiecesType _operateType = TicTacToePiecesType.Empty;
        
        public override void OnInit()
        {
            manModeSelectGoProperty.value = false;
            machineModeSelectGoProperty.value = false;
        }
        
        public override void OnActive()
        {
            TicTacToeGameService.OnPlayerSettingChangeEvent.AddListener(_OnPlayerSettingChangeEvent);
            ResetView();
        }

        public override void OnDispose()
        {
            TicTacToeGameService.OnPlayerSettingChangeEvent.RemoveListener(_OnPlayerSettingChangeEvent);
        }
        
        public TicTacToePlayerSettingViewModel(TicTacToePiecesType operateType)
        {
            this._operateType = operateType;
        }

        private void ResetView()
        {
            var ctrlType = TicTacToeGameSaveMgr.GetOperateControllerType(_operateType);
            manModeSelectGoProperty.value = ctrlType == OperateControllerType.UIClick;
            machineModeSelectGoProperty.value = ctrlType == OperateControllerType.AIMiniMax;
        }

        public void _OnClickManModeSelectBtn()
        {
            TicTacToeGameSaveMgr.SetOperateControllerType(_operateType, OperateControllerType.UIClick);
        }
        
        public void _OnClickMachineModeSelectBtn()
        {
            TicTacToeGameSaveMgr.SetOperateControllerType(_operateType, OperateControllerType.AIMiniMax);
        }

        private void _OnPlayerSettingChangeEvent(TicTacToePiecesType operateType)
        {
            if (this._operateType != operateType)
            {
                return;
            }
            ResetView();
        }
    }
}