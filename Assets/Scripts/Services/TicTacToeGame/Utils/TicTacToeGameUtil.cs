using UnityEngine;

namespace TTT.TicTacToeGame
{
    public static class TicTacToeGameUtil
    {
        public static bool GetRowAndColumnById(int id, out int row, out int column)
        {
            if (id < 0 || id >= TicTacToeGameConstant.ChessPiecesCount)
            {
                Debug.LogError($"传入Id超出范围了, id:{id}");
                row = default;
                column = default;
                return false;
            }
            row = id / TicTacToeGameConstant.ChessPiecesColumnCount;
            column = id % TicTacToeGameConstant.ChessPiecesColumnCount;
            return true;
        }
        
        public static int GetIdByRowAndColumn(int row, int column)
        {
            if (row < 0 || column < 0 || row >= TicTacToeGameConstant.ChessPiecesRowCount ||
                column >= TicTacToeGameConstant.ChessPiecesColumnCount)
            {
                Debug.LogError($"传入行列数超出范围, row:{row}, column:{column}");
                return default;
            }
            return row * TicTacToeGameConstant.ChessPiecesColumnCount + column;
        }
    }
}