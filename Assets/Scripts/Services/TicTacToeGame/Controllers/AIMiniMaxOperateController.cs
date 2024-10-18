using System;
using System.Collections.Generic;

namespace TTT.TicTacToeGame
{
    public class AIMiniMaxOperateController : OperateControllerBase
    {
        public override void StartOperate()
        {
            _TryOperate();
        }

        private void _TryOperate()
        {
            var pos = GetBestOperatePos();
            int id = TicTacToeGameUtil.GetIdByRowAndColumn(pos.row, pos.column);
            if (this._operateCallback == null)
            {
                return;
            }
            this._operateCallback.Invoke(id);
        }

        private TicTacToePiecePosition GetBestOperatePos()
        {
            var boardData = TicTacToeGameMgr.GetBoardDataCopy();
            int score;
            TicTacToePiecePosition pos = GetBestPosition(boardData, _playerCtrl.GetOperatePiecesType(), 0, out score);
            BoardDataPoolMgr.ReleaseBoardData(boardData);
            return pos;
        }
        
        public TicTacToePiecePosition GetBestPosition(TicTacToeBoardData boardData, TicTacToePiecesType operateType, int depth, out int score)
        {
            if (IsFinish(boardData, depth, out score))
            {
                return default;
            }
            depth++;

            TicTacToeBoardData newBoardData = BoardDataPoolMgr.GetBoardData();
            TicTacToePiecesType nextOperateType = TicTacToeGameUtil.SwitchPiecesType(operateType);

            bool isCurOperate = IsCurOperateType(operateType);
            TicTacToePiecePosition resultPos = new TicTacToePiecePosition(0,0);
            int resultScore = isCurOperate ? int.MinValue : int.MaxValue;
            for (int row = 0; row < TicTacToeGameConstant.ChessPiecesRowCount; row++)
            {
                for (int column = 0; column < TicTacToeGameConstant.ChessPiecesColumnCount; column++)
                {
                    if (!boardData.IsEmptyPiece(row, column))
                    {
                        continue;
                    }
                    newBoardData.CopyData(boardData);
                    newBoardData.SetPiecesType(row, column, operateType);
                    int newPosScore;
                    TicTacToePiecePosition newPosition = GetBestPosition(newBoardData, nextOperateType, depth, out newPosScore);
                    if (isCurOperate)
                    {
                        if (newPosScore >= resultScore)
                        {
                            resultPos.row = row;
                            resultPos.column = column;
                            resultScore = newPosScore;
                        }
                    }
                    else
                    {
                        if (newPosScore <= resultScore)
                        {
                            resultPos.row = row;
                            resultPos.column = column;
                            resultScore = newPosScore;
                        }
                    }
                }
            }
            BoardDataPoolMgr.ReleaseBoardData(newBoardData);
            return resultPos;
        }

        private bool IsFinish(TicTacToeBoardData boardData, int depth, out int score)
        {
            if (boardData == null)
            {
                score = 0;
                return true;
            }

            TicTacToePiecesType winType = TicTacToePiecesType.Empty;

            if (boardData.ExistsWinPiecesType(out winType))
            {
                bool isWin = IsCurOperateType(winType);
                score = isWin ? 10 : -10;
                score = score - (isWin ? depth : -depth);
                return true;
            }

            if (!boardData.ExistsEmptyPieces())
            {
                score = 0;
                return true;
            }

            score = default;
            return false;
        }

        private bool IsCurOperateType(TicTacToePiecesType operateType)
        {
            var curOperateType = _playerCtrl.GetOperatePiecesType();
            return curOperateType == operateType;
        }
    }
}