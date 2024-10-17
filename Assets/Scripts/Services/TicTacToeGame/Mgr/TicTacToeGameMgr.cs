namespace TTT.TicTacToeGame
{
    public class TicTacToeGameMgr : SingletonBase<TicTacToeGameMgr>
    {
        public static void StartGame()
        {
            UIManager.Open("TicTacToeGameView");
        }
    }
}