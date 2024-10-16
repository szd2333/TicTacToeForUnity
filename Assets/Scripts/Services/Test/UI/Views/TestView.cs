using System;
using UnityEngine.UI;

namespace TTT.Test
{
    public class TestView : ViewBase
    {
        public Text showText;
        public Button testBtn;
        public Slider testSlider;

        public override Type ViewModelType { get => typeof(TestViewModel); }

        public override UIRootType RootType { get => UIRootType.FUNCTION; }

        protected override void BindValues()
        {
            TestViewModel viewModel = _viewModel as TestViewModel;
            if (viewModel == null)
            {
                return;
            }
            BindValue(showText, viewModel.numProperty, "text");
        }

        protected override void BindEvents()
        {
            BindEvent(testBtn, "onClick", "_OnClick");
            BindEvent(testSlider, "onValueChanged", "_OnValueChange");
        }
    }
}