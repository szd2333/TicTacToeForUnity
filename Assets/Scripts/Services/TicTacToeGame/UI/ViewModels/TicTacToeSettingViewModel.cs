namespace TTT.TicTacToeGame
{
    public class TicTacToeSettingViewModel : ViewModelBase
    {
        public TicTacToePlayerSettingViewModel xSubView;
        public TicTacToePlayerSettingViewModel oSubView;
        
        public override void OnInit()
        {
            xSubView = new TicTacToePlayerSettingViewModel(TicTacToePiecesType.X);
            oSubView = new TicTacToePlayerSettingViewModel(TicTacToePiecesType.O);
        }

        public void _OnClickStartGame()
        {
            TicTacToeGameMgr.StartGame();
        }
    }
}