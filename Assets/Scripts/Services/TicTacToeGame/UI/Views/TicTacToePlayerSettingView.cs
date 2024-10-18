using System;
using UnityEngine;
using UnityEngine.UI;

namespace TTT.TicTacToeGame
{
    public class TicTacToePlayerSettingView : ViewBase
    {
        public Button manModeSelectBtn;
        public GameObject manModeSelectGo;
        public Button machineModeSelectBtn;
        public GameObject machineModeSelectGo;
        
        public bool manModeSelectGoActive
        {
            set => goutil.SetActive(manModeSelectGo, value);
        }
        
        public bool machineModeSelectGoActive
        {
            set => goutil.SetActive(machineModeSelectGo, value);
        }
        
        public override Type ViewModelType { get => typeof(TicTacToePlayerSettingView); }
        public override UIRootType RootType { get; }

        protected override void BindValues()
        {
            TicTacToePlayerSettingViewModel viewModel = _viewModel as TicTacToePlayerSettingViewModel;
            if (viewModel == null)
            {
                return;
            }
            BindValue(this, viewModel.manModeSelectGoProperty, "manModeSelectGoActive");
            BindValue(this, viewModel.machineModeSelectGoProperty, "machineModeSelectGoActive");
        }
        
        protected override void BindEvents()
        {
            BindEvent(manModeSelectBtn, "onClick", "_OnClickManModeSelectBtn");
            BindEvent(machineModeSelectBtn, "onClick", "_OnClickMachineModeSelectBtn");
        }
    }
}