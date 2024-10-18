using UnityEngine.Events;

namespace TTT.TicTacToeGame
{
    public class TicTacToeGameService : SingletonBase<TicTacToeGameService>, IService
    {

        private UnityEvent m_OnGameStartEvent;
        private UnityEvent<TicTacToePiecesType> m_OnRoundStartEvent;
        private UnityEvent<int> m_OnOperatePieceEvent;
        private UnityEvent<int> m_OnPieceUIClickEvent;

        //游戏开始事件
        public static UnityEvent OnGameStartEvent { get => Instance.m_OnGameStartEvent; }
        //回合开始事件
        public static UnityEvent<TicTacToePiecesType> OnRoundStartEvent { get => Instance.m_OnRoundStartEvent; }
        //操作棋子事件
        public static UnityEvent<int> OnOperatePieceEvent { get => Instance.m_OnOperatePieceEvent; }
        //点击棋子UI事件
        public static UnityEvent<int> OnPieceUIClickEvent { get => Instance.m_OnPieceUIClickEvent; }

        public void OnInit()
        {
            m_OnGameStartEvent = new UnityEvent();
            Instance.m_OnRoundStartEvent = new UnityEvent<TicTacToePiecesType>();
            m_OnOperatePieceEvent = new UnityEvent<int>();
            m_OnPieceUIClickEvent = new UnityEvent<int>();
        }

        public static void StartGame()
        {
            TicTacToeGameMgr.StartGame();
        }

        public static void NotifyPieceUIClickEvent(int id)
        {
            OnPieceUIClickEvent.Invoke(id);
        }
    }
}