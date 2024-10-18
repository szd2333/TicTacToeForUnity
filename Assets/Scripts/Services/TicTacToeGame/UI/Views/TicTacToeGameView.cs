using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTT.TicTacToeGame
{
    public class TicTacToeGameView : ViewBase
    {

        public Transform piecesContainer;
        public Text tipText;
        
        public override Type ViewModelType { get => typeof(TicTacToeGameViewModel); }
        public override UIRootType RootType { get => UIRootType.FUNCTION; }
        
        protected override void BindValues()
        {
            TicTacToeGameViewModel viewModel = _GetTicTacToeGameViewModel();
            if (viewModel == null)
            {
                return;
            }
            BindValue(tipText, viewModel.tipTextProperty, "text");
            BindAllPieceView();
        }

        private void BindAllPieceView()
        {
            TicTacToeGameViewModel viewModel = _GetTicTacToeGameViewModel();
            if (viewModel == null)
            {
                return;
            }

            if (goutil.IsNil(piecesContainer))
            {
                return;
            }

            Type subViewType = typeof(TicTacToePiecesView);
            for (int i = 0; i < TicTacToeGameConstant.ChessPiecesCount; i++)
            {
                string pieceViewName = $"TicTacToePiecesView{i}";
                Transform piecesViewTrans = piecesContainer.Find(pieceViewName);
                if (goutil.IsNil(piecesViewTrans))
                {
                    Debug.LogError($"找不到棋子{i}");
                    continue;
                }

                TicTacToePiecesView subView = piecesViewTrans.GetComponent(subViewType) as TicTacToePiecesView;
                if (subView == null)
                {
                    Debug.LogError($"棋子{i}未挂载界面脚本");
                    continue;
                }

                TicTacToePiecesViewModel subViewModel = viewModel.GetPiecesViewModelById(i);
                if (subViewModel == null)
                {
                    Debug.LogError($"棋子{i}获取不到viewModel");
                    continue;
                }

                BindSubView(subView, subViewModel);
            }
        }

        private TicTacToeGameViewModel _GetTicTacToeGameViewModel()
        {
            TicTacToeGameViewModel viewModel = _viewModel as TicTacToeGameViewModel;
            return viewModel;
        }
    }
}