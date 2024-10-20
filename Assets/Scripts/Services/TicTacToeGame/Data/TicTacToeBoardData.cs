using System.Collections.Generic;
using UnityEngine;

namespace TTT.TicTacToeGame
{
    public class TicTacToeBoardData
    {
        private TicTacToePiecesType[][] _ticTacToeBoard;
        
        public void InitData()
        {
            if (_ticTacToeBoard == null)
            {
                _ticTacToeBoard = new TicTacToePiecesType[TicTacToeGameConstant.ChessPiecesRowCount][];
            }

            for (int row = 0; row < TicTacToeGameConstant.ChessPiecesRowCount; row++)
            {
                TicTacToePiecesType[] rowBord = _ticTacToeBoard[row];
                if (rowBord == null)
                {
                    rowBord = new TicTacToePiecesType[TicTacToeGameConstant.ChessPiecesColumnCount];
                    _ticTacToeBoard[row] = rowBord;
                }
                for (int column = 0; column < TicTacToeGameConstant.ChessPiecesColumnCount; column++)
                {
                    _ticTacToeBoard[row][column] = TicTacToePiecesType.Empty;
                }
            }
        }

        public void CopyData(TicTacToeBoardData originBoardData)
        {
            if (originBoardData == null)
            {
                return;
            }
            for (int row = 0; row < TicTacToeGameConstant.ChessPiecesRowCount; row++)
            {
                for (int column = 0; column < TicTacToeGameConstant.ChessPiecesColumnCount; column++)
                {
                    _ticTacToeBoard[row][column] = originBoardData.GetPiecesType(row, column);
                }
            }
        }

        #region 棋子相关
        
        public TicTacToePiecesType GetPiecesType(int row, int column)
        {
            if (row < 0 || column < 0 || row >= TicTacToeGameConstant.ChessPiecesRowCount ||
                column >= TicTacToeGameConstant.ChessPiecesColumnCount)
            {
                Debug.LogError($"传入行列数超出范围, row:{row}, column:{column}");
                return default;
            }
            return _ticTacToeBoard[row][column];
        }
        
        public void SetPiecesType(int row, int column, TicTacToePiecesType piecesType)
        {
            if (row < 0 || column < 0 || row >= TicTacToeGameConstant.ChessPiecesRowCount ||
                column >= TicTacToeGameConstant.ChessPiecesColumnCount)
            {
                Debug.LogError($"传入行列数超出范围, row:{row}, column:{column}");
                return;
            }
            _ticTacToeBoard[row][column] = piecesType;
        }

        public bool ExistsEmptyPieces()
        {
            for (int row = 0; row < TicTacToeGameConstant.ChessPiecesRowCount; row++)
            {
                for (int column = 0; column < TicTacToeGameConstant.ChessPiecesColumnCount; column++)
                {
                    var piecesType = GetPiecesType(row, column);
                    if (piecesType == TicTacToePiecesType.Empty)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        public bool IsEmptyPiece(int row, int column)
        {
            return GetPiecesType(row, column) == TicTacToePiecesType.Empty;
        }

        #endregion

        #region 胜利判定
        
        public bool ExistsWinPiecesType(out TicTacToePiecesType winType)
        {
            //行检查
            for (int row = 0; row < TicTacToeGameConstant.ChessPiecesRowCount; row++)
            {
                if (ExistsWinPiecesInRow(row, out winType))
                {
                    return true;
                }
            }
            
            //列检查
            for (int column = 0; column < TicTacToeGameConstant.ChessPiecesColumnCount; column++)
            {
                if (ExistsWinPiecesInColumn(column, out winType))
                {
                    return true;
                }
            }
            
            //对角线检查
            if (ExistsWinPiecesInDiagonal(true, out winType))
            {
                return true;
            }
            
            if (ExistsWinPiecesInDiagonal(false, out winType))
            {
                return true;
            }

            return false;
        }

        public bool ExistsWinPiecesInRow(int row, out TicTacToePiecesType winType)
        {
            var firstType = GetPiecesType(row, 0);
            for (int column = 0; column < TicTacToeGameConstant.ChessPiecesColumnCount; column++)
            {
                var piecesType = GetPiecesType(row, column);
                if (piecesType == TicTacToePiecesType.Empty || piecesType != firstType)
                {
                    winType = TicTacToePiecesType.Empty;
                    return false;
                }
            }
            winType = firstType;
            return true;
        }
        
        public bool ExistsWinPiecesInColumn(int column, out TicTacToePiecesType winType)
        {
            var firstType = GetPiecesType(0, column);
            for (int row = 0; row < TicTacToeGameConstant.ChessPiecesRowCount; row++)
            {
                var piecesType = GetPiecesType(row, column);
                if (piecesType == TicTacToePiecesType.Empty || piecesType != firstType)
                {
                    winType = TicTacToePiecesType.Empty;
                    return false;
                }
            }
            winType = firstType;
            return true;
        }
        
        /// <summary>
        /// 对角线是否存在胜利棋子
        /// </summary>
        /// <param name="isPrincipal">是否为主对角线(左上到右下)</param>
        /// <param name="winType">胜利棋子类型</param>
        /// <returns></returns>
        public bool ExistsWinPiecesInDiagonal(bool isPrincipal, out TicTacToePiecesType winType)
        {
            var startRow = isPrincipal ? 0 : TicTacToeGameConstant.ChessPiecesRowCount - 1;
            var rowMoveDirection = isPrincipal ? 1 : -1; //行计算方向
            var firstType = GetPiecesType(startRow, 0);
            var row = startRow;
            
            for (int column = 0; column < TicTacToeGameConstant.ChessPiecesColumnCount; column++)
            {
                var piecesType = GetPiecesType(row, column);
                if (piecesType == TicTacToePiecesType.Empty || piecesType != firstType)
                {
                    winType = TicTacToePiecesType.Empty;
                    return false;
                }
                row += rowMoveDirection;
            }
            winType = firstType;
            return true;
        }

        #endregion
        
        
    }
}