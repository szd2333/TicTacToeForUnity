using System;
using UnityEngine.UI;

namespace TTT.TicTacToeGame
{
    public class TicTacToeSettingView : ViewBase
    {
        public Button startGameBtn;
        public TicTacToePlayerSettingView xPlayerSettingView;
        public TicTacToePlayerSettingView oPlayerSettingView;
        public override Type ViewModelType { get => typeof(TicTacToeSettingViewModel); }
        public override UIRootType RootType { get => UIRootType.POPUP; }

        protected override void BindValues()
        {
            TicTacToeSettingViewModel viewModel = _viewModel as TicTacToeSettingViewModel;
            if (viewModel == null)
            {
                return;
            }
            BindSubView(xPlayerSettingView, viewModel.xSubView);
            BindSubView(oPlayerSettingView, viewModel.oSubView);
        }

        protected override void BindEvents()
        {
            BindEvent(startGameBtn, "onClick", "_OnClickStartGame");
        }
    }
}