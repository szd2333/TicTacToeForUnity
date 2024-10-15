using System;
using MVVM;
using UnityEngine;
using UnityEngine.UI;

namespace TTT.Test
{
    public class TestView : ViewBase<TestViewModel>
    {
        public Text showText;
        public Button testBtn;
        public Slider testSlider;

        void Start()
        {
            TestViewModel viewModel = new TestViewModel();
            Open(viewModel);
        }

        protected override void BindValues()
        {
            BindValue(showText, _viewModel.numProperty, "text");
        }

        protected override void BindEvents()
        {
            BindEvent(testBtn, "onClick", "_OnClick");
            BindEvent(testSlider, "onValueChanged", "_OnValueChange");
        }
    }
}