using UnityEngine;

namespace TTT.TicTacToeGame
{
    public class TicTacToeGameMgr : SingletonBase<TicTacToeGameMgr>
    {
        //棋盘数据
        private TicTacToeBoardData _ticTacToeBoardData;
        //当前操作棋盘类型
        private TicTacToePiecesType _curOperatePiecesType = TicTacToePiecesType.Empty;
        //当前是否可操作
        private bool _isCurOperational = false;
 
        public static void StartGame()
        {
            Start();
        }

        public static void TryOperatePiece(int row , int column, TicTacToePiecesType operatePieceType)
        {
            if (!CanOperatePiece(row, column, operatePieceType))
            {
                return;
            }
            SetPiecesType(row, column, operatePieceType);
            FinishRound();
        }

        private static void InitData()
        {
            InitOperateData();
            InitBoardData();
        }

        #region 流程相关

        private static void Start()
        {
            InitData();
            if(!UIManager.IsOpen("TicTacToeGameView"))
            {
                UIManager.Open("TicTacToeGameView");
            }
            Debug.Log("开始");
            NextRound();
        }

        private static void NextRound()
        {
            ResetCurOperatePiecesType();
            RoundStart();
        }

        private static void RoundStart()
        {
            SetIsCurOperational(true);
            Debug.Log("回合开始");
        }
        
        private static void FinishRound()
        {
            SetIsCurOperational(false);
            TicTacToePiecesType winType = TicTacToePiecesType.Empty;
            if (ExistsWinPiecesType(out winType))
            {
                FinishGame();
            }
            else
            {
                NextRound();
            }
            Debug.Log("回合结束");
        }
        
        private static void FinishGame()
        {
            Debug.Log("游戏结束");
            Start();
        }

        #endregion

        #region 棋盘相关

        private static void InitBoardData()
        {
            if (Instance._ticTacToeBoardData == null)
            {
                Instance._ticTacToeBoardData = new TicTacToeBoardData();
            }
            Instance._ticTacToeBoardData.InitData();;
        }

        private static void SetPiecesType(int row, int column, TicTacToePiecesType pieceType)
        {
            if (Instance._ticTacToeBoardData == null)
            {
                return;
            }
            Instance._ticTacToeBoardData.SetPiecesType(row, column, pieceType);
        }
        
        private static TicTacToePiecesType GetPiecesType(int row, int column)
        {
            if (Instance._ticTacToeBoardData == null)
            {
                return default;
            }
            return Instance._ticTacToeBoardData.GetPiecesType(row, column);
        }

        private static bool ExistsWinPiecesType(out TicTacToePiecesType winType)
        {
            if (Instance._ticTacToeBoardData == null)
            {
                winType = TicTacToePiecesType.Empty;
                return false;
            }

            return Instance._ticTacToeBoardData.ExistsWinPiecesType(out winType);
        }
        
        #endregion

        #region 操作信息相关

        private static void InitOperateData()
        {
            Instance._curOperatePiecesType = TicTacToePiecesType.Empty;
            Instance._isCurOperational = false;
        }

        private static void ResetCurOperatePiecesType()
        {
            switch (Instance._curOperatePiecesType)
            {
                case TicTacToePiecesType.Empty :
                    Instance._curOperatePiecesType = TicTacToePiecesType.X;
                    break;
                case TicTacToePiecesType.O :
                    Instance._curOperatePiecesType = TicTacToePiecesType.X;
                    break;
                case TicTacToePiecesType.X :
                    Instance._curOperatePiecesType = TicTacToePiecesType.O;
                    break;
            }
        }

        private static bool CanOperatePiece(int row , int column, TicTacToePiecesType operatePieceType)
        {
            if (!GetIsCurOperational())
            {
                Debug.Log("不在可操作阶段");
                return false;
            }

            if (operatePieceType == TicTacToePiecesType.Empty || Instance._curOperatePiecesType != operatePieceType)
            {
                Debug.Log("不在可操作阶段");
                return false;
            }

            var curPiecesType = GetPiecesType(row, column);
            if (curPiecesType != TicTacToePiecesType.Empty)
            {
                Debug.Log("当前位置已存在棋子");
                return false;
            }
            return true;
        }

        private static void SetIsCurOperational(bool isCurOperational)
        {
            Instance._isCurOperational = isCurOperational;
        }

        public static bool GetIsCurOperational()
        {
            return Instance._isCurOperational;
        }
        
        #endregion
    }
}