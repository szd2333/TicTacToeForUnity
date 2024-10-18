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
        
        public TicTacToePlayerSettingViewModel(TicTacToePiecesType operateType)
        {
            this._operateType = operateType;
            ResetView();
        }

        private void ResetView()
        {
            
        }
    }
}