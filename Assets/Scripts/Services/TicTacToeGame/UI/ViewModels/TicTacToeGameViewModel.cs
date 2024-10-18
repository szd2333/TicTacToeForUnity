using UnityEngine;

namespace TTT.TicTacToeGame
{
    public class TicTacToeGameViewModel : ViewModelBase
    {
        private TicTacToePiecesViewModel[] piecesViewModels;
        public Observer tipTextProperty = new Observer("");

        public override void OnInit()
        {
            _InitAllPiecesViewModel();
        }

        public override void OnActive()
        {
            TicTacToeGameService.OnGameStartEvent.AddListener(_OnGameStartEvent);
            TicTacToeGameService.OnRoundStartEvent.AddListener(_OnRoundStartEvent);
            TicTacToeGameService.OnGameFinishEvent.AddListener(_OnGameFinishEvent);
            TicTacToeGameService.OnOperatePieceEvent.AddListener(_OnOperatePieceEvent);
        }

        public override void OnDispose()
        {
            TicTacToeGameService.OnGameStartEvent.RemoveListener(_OnGameStartEvent);
            TicTacToeGameService.OnRoundStartEvent.RemoveListener(_OnRoundStartEvent);
            TicTacToeGameService.OnGameFinishEvent.RemoveListener(_OnGameFinishEvent);
            TicTacToeGameService.OnOperatePieceEvent.RemoveListener(_OnOperatePieceEvent);
        }

        private void _ResetView()
        {
            _ResetAllPiecesViewModel();
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

        private void _ResetAllPiecesViewModel()
        {
            if (piecesViewModels == null)
            {
                return;
            }

            foreach (var subViewModel in piecesViewModels)
            {
                subViewModel.ResetView();
            }
        }

        private void _ResetPiecesViewModel(int id)
        {
            var subViewModel = GetPiecesViewModelById(id);
            if (subViewModel == null)
            {
                return;
            }
            subViewModel.ResetView();
        }

        #endregion

        private void _OnClickPiece(int id)
        {
            TicTacToeGameService.NotifyPieceUIClickEvent(id);
        }

        private void _OnGameStartEvent()
        {
            _ResetView();
        }

        private void _OnOperatePieceEvent(int id)
        {
            _ResetPiecesViewModel(id);
        }

        private void _OnRoundStartEvent(TicTacToePiecesType curOperatePiecesType)
        {
            string tip = curOperatePiecesType == TicTacToePiecesType.O ? "棋子O行动" : "棋子X行动";
            tipTextProperty.value = tip;
        }

        private void _OnGameFinishEvent(TicTacToePiecesType winType)
        {
            string tip = "";
            switch (winType)
            {
                case TicTacToePiecesType.Empty :
                    tip = "游戏结束, 平局";
                    break;
                case TicTacToePiecesType.O :
                    tip = "游戏结束, 棋子O获胜";
                    break;
                case TicTacToePiecesType.X :
                    tip = "游戏结束, 棋子X获胜";
                    break;
            }
            tipTextProperty.value = tip;
        }
    }
}