using System;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace TTT
{
    public class PropertyBinder
    {
        private Dictionary<UObject, Dictionary<Observer, Action>> _bindPropertyDict = new Dictionary<UObject, Dictionary<Observer, Action>>();
        
        public void AddValueBinder(UObject bindObj, Observer observer, string arg)
        {
            if (!_CanValueBinder(bindObj, observer, arg))
            {
                return;
            }

            if (!_bindPropertyDict.ContainsKey(bindObj))
            {
                _bindPropertyDict[bindObj] = new Dictionary<Observer, Action>();
            }
            Dictionary<Observer, Action> actionDict = _bindPropertyDict[bindObj];
            
            //已存在则移除旧的
            if (actionDict.ContainsKey(observer))
            {
                _RemoveValueBinder(bindObj, observer);
            }
            
            Action action = () =>
            {
                _ValueBinderHandler(bindObj, observer, arg);
            };
            
            action.Invoke();
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

        private void _RemoveValueBinder(UObject bindObj, Observer observer)
        {
            if (goutil.IsNil(bindObj) || observer == null)
            {
                return;
            }
            
            if (!_bindPropertyDict.ContainsKey(bindObj))
            {
                return;
            }
            
            Dictionary<Observer, Action> actionDict = _bindPropertyDict[bindObj];
            if (!actionDict.ContainsKey(observer))
            {
                return;
            }
            Action action = actionDict[observer];
            observer.OnValueChanged -= action;
        }

        private bool _CanValueBinder(UObject bindObj, Observer observer, string arg)
        {
            if (goutil.IsNil(bindObj))
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
            
            var propInfo = DataBindUtil.GetUObjPropInfo(bindObj, arg);
            if (propInfo == null || propInfo.PropertyType != observer.valueType)
            {
                string propTypeName = propInfo == null ? "null" : propInfo.PropertyType.Name;
                Debug.LogError($"绑定参数类型错误, comp:{bindObj.GetType()}, propTypeName:{arg}, compType:{propTypeName}, observer.valueType:{observer.valueType.Name}");
                return false;
            }
            return true;
        }

        private void _ValueBinderHandler(UObject bindObj, Observer observer, string arg)
        {
            if (goutil.IsNil(bindObj) || observer == null || string.IsNullOrEmpty(arg))
            {
                return;
            }
            var propInfo = DataBindUtil.GetUObjPropInfo(bindObj, arg);

            if (propInfo == null || propInfo.PropertyType != observer.valueType)
            {
                string propTypeName = propInfo == null ? "null" : propInfo.PropertyType.Name;
                Debug.LogError($"绑定参数类型错误, comp:{bindObj.GetType()}, propTypeName:{arg}, compType:{propTypeName}, observer.valueType:{observer.valueType.Name}");
                return;
            }

            try
            {
                propInfo.SetValue(bindObj, observer.value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}