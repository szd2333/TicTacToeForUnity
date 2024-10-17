using UnityEngine;

namespace TTT.TicTacToeGame
{
    public class TicTacToeGameViewModel : ViewModelBase
    {
        private TicTacToePiecesViewModel[] piecesViewModels;

        public override void OnInit()
        {
            _InitAllPiecesViewModel();
        }

        #region 棋子子界面相关

        public TicTacToePiecesViewModel GetPiecesViewModelById(int id)
        {
            if (piecesViewModels == null || piecesViewModels.Length <= id)
            {
                return null;
            }
            return piecesViewModels[id];
        }

        private void _InitAllPiecesViewModel()
        {
            piecesViewModels = new TicTacToePiecesViewModel[TicTacToeGameConstant.ChessPiecesCount];
            for (int i = 0; i < TicTacToeGameConstant.ChessPiecesCount; i++)
            {
                var piecesViewModel = new TicTacToePiecesViewModel(i, _OnClickPiece);
                piecesViewModels[i] = piecesViewModel;
            }
        }

        #endregion

        private void _OnClickPiece(int id)
        {
            Debug.Log($"点击棋子{id}");
        }
        
    }
}