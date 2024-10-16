using System;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public class PropertyBinder
    {
        private Dictionary<Component, Dictionary<Observer, Action>> _bindPropertyDict = new Dictionary<Component, Dictionary<Observer, Action>>();
        
        public void AddValueBinder(Component component, Observer observer, string arg)
        {
            if (!_CanValueBinder(component, observer, arg))
            {
                return;
            }

            if (!_bindPropertyDict.ContainsKey(component))
            {
                _bindPropertyDict[component] = new Dictionary<Observer, Action>();
            }
            Dictionary<Observer, Action> actionDict = _bindPropertyDict[component];
            
            //已存在则移除旧的
            if (actionDict.ContainsKey(observer))
            {
                _RemoveValueBinder(component, observer);
            }
            
            Action action = () =>
            {
                _ValueBinderHandler(component, observer, arg);
            };
            observer.OnValueChanged += action;
            actionDict[observer] = action;
        }
        
        public void RemoveAllValueBinder()
        {
            if (_bindPropertyDict == null)
            {
                return;
            }
            foreach (var bindPropertyPair in _bindPropertyDict)
            {
                Dictionary<Observer, Action> actionDict = bindPropertyPair.Value;
                foreach (var actionDictPair in actionDict)
                {
                    Observer observer = actionDictPair.Key;
                    Action action = actionDictPair.Value;
                    observer.OnValueChanged -= action;
                }
            }
            _bindPropertyDict.Clear();
        }

        private void _RemoveValueBinder(Component component, Observer observer)
        {
            if (goutil.IsNil(component) || observer == null)
            {
                return;
            }
            
            if (!_bindPropertyDict.ContainsKey(component))
            {
                return;
            }
            
            Dictionary<Observer, Action> actionDict = _bindPropertyDict[component];
            if (!actionDict.ContainsKey(observer))
            {
                return;
            }
            Action action = actionDict[observer];
            observer.OnValueChanged -= action;
        }

        private bool _CanValueBinder(Component component, Observer observer, string arg)
        {
            if (goutil.IsNil(component))
            {
                Debug.LogError($"绑定参数类型错误, component 不得为空");
                return false;
            }

            if (observer == null)
            {
                Debug.LogError($"绑定参数类型错误, observer 不得为空");
                return false;
            }
            
            if (string.IsNullOrEmpty(arg))
            {
                Debug.LogError($"绑定参数类型错误, arg 不得为空");
                return false;
            }
            
            var propInfo = DataBindUtil.GetCompPropInfo(component, arg);
            if (propInfo == null || propInfo.PropertyType != observer.valueType)
            {
                string propTypeName = propInfo == null ? "null" : propInfo.PropertyType.Name;
                Debug.LogError($"绑定参数类型错误, comp:{component.GetType()}, propTypeName:{arg}, compType:{propTypeName}, observer.valueType:{observer.valueType.Name}");
                return false;
            }
            return true;
        }

        private void _ValueBinderHandler(Component component, Observer observer, string arg)
        {
            if (goutil.IsNil(component) || observer == null || string.IsNullOrEmpty(arg))
            {
                return;
            }
            var propInfo = DataBindUtil.GetCompPropInfo(component, arg);

            if (propInfo == null || propInfo.PropertyType != observer.valueType)
            {
                string propTypeName = propInfo == null ? "null" : propInfo.PropertyType.Name;
                Debug.LogError($"绑定参数类型错误, comp:{component.GetType()}, propTypeName:{arg}, compType:{propTypeName}, observer.valueType:{observer.valueType.Name}");
                return;
            }
            propInfo.SetValue(component, observer.value);
        }
    }
}