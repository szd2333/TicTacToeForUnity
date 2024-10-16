using UnityEngine;

namespace TTT.Test
{
    public class TestViewModel : ViewModelBase
    {
        public Observer numProperty = new Observer("");

        private int testNum = 0;

        public override void OnInit()
        {
            numProperty.value = testNum.ToString();
        }

        private void SetNum(int value)
        {
            testNum = value;
            numProperty.value = testNum.ToString();
        }

        public void _OnClick()
        {
            SetNum(testNum + 1);
        }

        public void _OnValueChange(float valueChange)
        {
            SetNum(Mathf.CeilToInt(valueChange * 100));
        }
    }
}