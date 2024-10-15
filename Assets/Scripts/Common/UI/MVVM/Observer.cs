using System;

namespace MVVM
{
    public class Observer
    {
        private object _lastValue;
        private Type _valueType;
        
        //public delegate void ValueChangedHandler(Object newValue);

        public Action OnValueChanged;

        public Object value
        {
            get => _lastValue;
            set
            {
                if (_lastValue.GetType() != _valueType)
                {
                    return;
                }
                if (Equals(_lastValue, value))
                {
                    return;
                }
                _lastValue = value;
                ValueChanged();
            }
        }

        public Type valueType
        {
            get => _valueType;
        }
        
        public Observer(Object initValue)
        {
            _lastValue = initValue;
            _valueType = initValue.GetType();
        }

        public void ValueChanged()
        {
            if (OnValueChanged == null)
            {
                return;
            }
            OnValueChanged.Invoke();
        }
    }
}

