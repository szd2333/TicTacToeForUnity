using UnityEngine.Events;

namespace TTT.TicTacToeGame
{
    public class TicTacToeGameService : SingletonBase<TicTacToeGameService>, IService
    {

        private UnityEvent m_OnGameStartEvent;
        private UnityEvent<int> m_OnOperatePieceEvent;

        //游戏开始事件
        public static UnityEvent OnGameStartEvent { get => Instance.m_OnGameStartEvent; }
        //操作棋子事件
        public static UnityEvent<int> OnOperatePieceEvent { get => Instance.m_OnOperatePieceEvent; }

        public void OnInit()
        {
            m_OnGameStartEvent = new UnityEvent();
            m_OnOperatePieceEvent = new UnityEvent<int>();
        }

        public static void StartGame()
        {
            TicTacToeGameMgr.StartGame();
        }
    }
}