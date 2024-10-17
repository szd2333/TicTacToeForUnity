using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTT.TicTacToeGame
{
    public class TicTacToePiecesView : ViewBase
    {
        public GameObject xPieceGo;
        public GameObject oPieceGo;
        public Button clickBtn;

        public bool xPieceGoActive
        {
            set => goutil.SetActive(xPieceGo, value);
        }
        
        public bool oPieceGoActive
        {
            set => goutil.SetActive(oPieceGo, value);
        }
        
        public override Type ViewModelType { get => typeof(TicTacToePiecesViewModel); }
        public override UIRootType RootType { get; }

        protected override void BindValues()
        {
            TicTacToePiecesViewModel viewModel = _viewModel as TicTacToePiecesViewModel;
            if (viewModel == null)
            {
                return;
            }
            BindValue(this, viewModel.showXPieceGoProperty, "xPieceGoActive");
            BindValue(this, viewModel.showOPieceGoProperty, "oPieceGoActive");
        }

        protected override void BindEvents()
        {
            BindEvent(clickBtn, "onClick", "_OnClick");
        }
    }
}