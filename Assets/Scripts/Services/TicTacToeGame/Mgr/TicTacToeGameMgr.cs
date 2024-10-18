using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

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
        //玩家控制器
        private Dictionary<TicTacToePiecesType, PlayerController> _playerControllerDict;
        //延迟触发器
        private Timer _delayInvokeTimer;


        #region public接口

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
            int id = TicTacToeGameUtil.GetIdByRowAndColumn(row, column);
            TicTacToeGameService.OnOperatePieceEvent.Invoke(id);
            FinishRound();
        }
        
        public static void TryOperatePieceById(int id, TicTacToePiecesType operatePieceType)
        {
            int row, column;
            TicTacToeGameUtil.GetRowAndColumnById(id, out row, out column);
            TryOperatePiece(row, column, operatePieceType);
        }

        #endregion

        private static void InitData()
        {
            InitOperateData();
            InitBoardData();
            InitPlayerControllers();
        }

        private static void DisposeData()
        {
            DisposePlayerControllers();
        }

        #region 流程相关

        private static void Start()
        {
            InitData();
            if (UIManager.IsOpen("TicTacToeSettingView"))
            {
                UIManager.Close("TicTacToeSettingView");
            }
            if(!UIManager.IsOpen("TicTacToeGameView"))
            {
                UIManager.Open("TicTacToeGameView");
            }
            TicTacToeGameService.OnGameStartEvent.Invoke();
            Debug.Log("开始");
            SetDelayInvokeTimer(1, RoundStart);
        }

        private static void NextRound()
        {
            SwitchCurOperatePiecesType();
            SetDelayInvokeTimer(1, RoundStart);
        }

        private static void RoundStart()
        {
            SetIsCurOperational(true);
            Debug.Log("回合开始");
            TicTacToeGameService.OnRoundStartEvent.Invoke(GetCurOperatePiecesType());
        }
        
        private static void FinishRound()
        {
            SetIsCurOperational(false);
            TicTacToePiecesType winType = TicTacToePiecesType.Empty;
            if (ExistsWinPiecesType(out winType))
            {
                FinishGame(winType);
                return;
            }

            if (!ExistsEmptyPieces())
            {
                FinishGame(TicTacToePiecesType.Empty);
                return;
            }
            Debug.Log("回合结束");
            NextRound();
        }
        
        private static void FinishGame(TicTacToePiecesType winType)
        {
            switch (winType)
            {
                case TicTacToePiecesType.Empty :
                    Debug.Log("游戏结束, 平局");
                    break;
                case TicTacToePiecesType.O :
                    Debug.Log("游戏结束, O获胜");
                    break;
                case TicTacToePiecesType.X :
                    Debug.Log("游戏结束, X获胜");
                    break;
            }
            DisposeData();
            SetDelayInvokeTimer(1, OpenSettingView);
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
        
        public static TicTacToePiecesType GetPiecesType(int row, int column)
        {
            if (Instance._ticTacToeBoardData == null)
            {
                return default;
            }
            return Instance._ticTacToeBoardData.GetPiecesType(row, column);
        }
        
        public static TicTacToePiecesType GetPiecesTypeById(int id)
        {
            int row, column;
            TicTacToeGameUtil.GetRowAndColumnById(id, out row, out column);
            return GetPiecesType(row, column);
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
        
        private static bool ExistsEmptyPieces()
        {
            if (Instance._ticTacToeBoardData == null)
            {
                return false;
            }

            return Instance._ticTacToeBoardData.ExistsEmptyPieces();
        }
        
        public static bool IsEmptyPiece(int row, int column)
        {
            if (Instance._ticTacToeBoardData == null)
            {
                return false;
            }
            return Instance._ticTacToeBoardData.IsEmptyPiece(row, column);
        }

        public static TicTacToeBoardData GetBoardDataCopy()
        {
            return BoardDataPoolMgr.CopyBoardData(Instance._ticTacToeBoardData);
        }
        
        #endregion

        #region 操作信息相关

        private static void InitOperateData()
        {
            Instance._curOperatePiecesType = TicTacToePiecesType.Empty;
            Instance._isCurOperational = false;
            InitCurOperatePiecesType();
        }

        private static void InitCurOperatePiecesType()
        {
            //随机先手
            int randomNum = Random.Range(0, 100);
            Instance._curOperatePiecesType = randomNum > 50 ? TicTacToePiecesType.O : TicTacToePiecesType.X;
            Debug.Log($"{Instance._curOperatePiecesType}先手, randomNum:{randomNum}");
        }

        private static void SwitchCurOperatePiecesType()
        {
            Instance._curOperatePiecesType = TicTacToeGameUtil.SwitchPiecesType(Instance._curOperatePiecesType);
        }

        private static TicTacToePiecesType GetCurOperatePiecesType()
        {
            return Instance._curOperatePiecesType;
        }

        private static bool CanOperatePiece(int row , int column, TicTacToePiecesType operatePieceType)
        {
            if (!GetIsCurOperational())
            {
                Debug.Log("不在可操作阶段");
                return false;
            }

            var curOperatePiecesType = GetCurOperatePiecesType(); 
            if (operatePieceType == TicTacToePiecesType.Empty || curOperatePiecesType != operatePieceType)
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
            ResetAllPlayerCtrlOperationalState();
        }

        public static bool GetIsCurOperational()
        {
            return Instance._isCurOperational;
        }
        
        #endregion

        #region 玩家控制器相关

        private static void InitPlayerControllers()
        {
            //可以根据模式设置不同的玩家控制器
            Instance._playerControllerDict = new Dictionary<TicTacToePiecesType, PlayerController>()
            {
                { TicTacToePiecesType.O, new PlayerController(TicTacToePiecesType.O, TicTacToeGameSaveMgr.GetOperateControllerType(TicTacToePiecesType.O)) },
                { TicTacToePiecesType.X, new PlayerController(TicTacToePiecesType.X, TicTacToeGameSaveMgr.GetOperateControllerType(TicTacToePiecesType.X))}
            };
        }

        private static void DisposePlayerControllers()
        {
            if (Instance._playerControllerDict == null)
            {
                return;
            }
            foreach (var keyValuePair in Instance._playerControllerDict)
            {
                PlayerController playerCtrl = keyValuePair.Value;
                playerCtrl.Dispose();
            }
            Instance._playerControllerDict.Clear();
        }

        private static void ResetAllPlayerCtrlOperationalState()
        {
            if (Instance._playerControllerDict == null)
            {
                return;
            }
            
            var curOperational = GetIsCurOperational();
            var curOperatePiecesType = GetCurOperatePiecesType();
            foreach (var keyValuePair in Instance._playerControllerDict)
            {
                PlayerController playerCtrl = keyValuePair.Value;
                var playerPiecesType = playerCtrl.GetOperatePiecesType();
                playerCtrl.SetIsCurOperational(curOperational && curOperatePiecesType == playerPiecesType);
            }
        }

        #endregion

        #region Timer相关

        private static void SetDelayInvokeTimer(float seconds, Action callback)
        {
            ClearDelayInvokeTimer();
            var timer = new Timer(seconds);
            Instance._delayInvokeTimer = timer;
            timer.onEnd += callback;
        }

        private static void ClearDelayInvokeTimer()
        {
            if (Instance._delayInvokeTimer == null)
            {
                return;
            }

            Timer timer = Instance._delayInvokeTimer;
            Instance._delayInvokeTimer = null;
            timer.Stop();
        }

        #endregion

        public static void OpenSettingView()
        {
            if(!UIManager.IsOpen("TicTacToeSettingView"))
            {
                UIManager.Open("TicTacToeSettingView");
            }
        }
        
    }
}