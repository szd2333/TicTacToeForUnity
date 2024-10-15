using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TTT;
using UnityEngine;
using UnityEngine.Events;

namespace MVVM
{
    public class EventBinder
    {
        private ViewModelBase _viewModel;
        private Dictionary<Component, Dictionary<string, Delegate>> _bindEventDict = new Dictionary<Component, Dictionary<string, Delegate>>();

        public EventBinder(ViewModelBase viewModelBase)
        {
            this._viewModel = viewModelBase;
        }
        
        public void AddEventBinder(Component component, string eventName, string triggerMethodName)
        {
            if (_viewModel == null)
            {
                return;
            }
            if (!_CanEventBinder(component, eventName, triggerMethodName))
            {
                return;
            }

            if (!_bindEventDict.ContainsKey(component))
            {
                _bindEventDict[component] = new Dictionary<string, Delegate>();
            }
            var triggerMethodDict = _bindEventDict[component];
            
            //已存在则移除旧的
            if (triggerMethodDict.ContainsKey(eventName))
            {
                _RemoveValueBinder(component, eventName);
            }
            
            var eventObj = DataBindUtil.GetCompPropObj(component, eventName);
            if (eventObj == null)
            {
                Debug.LogError($"绑定组件不存在事件对象, component:{component.GetType()}, eventName:{eventName}");
                return;
            }
            
            //获取监听方法
            var addListenerMethod = DataBindUtil.GetAddListenerMethodInfo(eventObj);
            if (addListenerMethod == null)
            {
                Debug.LogError($"绑定组件事件不存在AddListener方法, component:{component.GetType()}, eventName:{eventName}");
                return;
            }
            
            //获取监听事件类型
            Type eventType;
            string errorTip = ""; 
            if(!DataBindUtil.IsOnlyUnityActionParam(addListenerMethod, out eventType, out errorTip))
            {
                Debug.LogError($"绑定组件事件AddListener方法不合规, component:{component.GetType()}, eventName:{eventName}, {errorTip}");
                return;
            }

            //获取触发方法
            var triggerMethodInfo = DataBindUtil.GetMethodInfo(_viewModel, triggerMethodName);
            if (triggerMethodInfo == null)
            {
                Debug.LogError($"绑定触发方法不存在, bindObj:{_viewModel.GetType()}, bindMethodName:{triggerMethodName}");
                return;
            }
            
            //绑定触发方法
            Delegate triggerDelegate = Delegate.CreateDelegate(eventType, _viewModel, triggerMethodInfo);
            
            var p = new object[] { triggerDelegate };

            bool isSuccess = true;
            try
            {
                addListenerMethod.Invoke(eventObj, p);
                
            }
            catch (Exception e)
            {
                isSuccess = false;
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                if (isSuccess)
                {
                    triggerMethodDict.Add(triggerMethodName, triggerDelegate);
                }
            }
        }
        
        public void RemoveAllEventBinder()
        {
            if (_bindEventDict == null)
            {
                return;
            }
            foreach (var _bindEventDictPair in _bindEventDict)
            {
                Component component = _bindEventDictPair.Key;
                Dictionary<string, Delegate> triggerMethodDict = _bindEventDictPair.Value;
                foreach (var bindEventPair in triggerMethodDict)
                {
                    string eventName = bindEventPair.Key;
                    _RemoveValueBinder(component, eventName, false);
                }
            }
            _bindEventDict.Clear();
        }

        private void _RemoveValueBinder(Component component, string eventName, bool needRemoveInDict = true)
        {
            if (_viewModel == null)
            {
                return;
            }

            if (goutil.IsNil(component))
            {
                return;
            }
            
            if (!_bindEventDict.ContainsKey(component))
            {
                return;
            }
            var triggerMethodDict = _bindEventDict[component];
            if (!triggerMethodDict.ContainsKey(eventName))
            {
                return;
            }
            Delegate triggerDelegate = triggerMethodDict[eventName];
            if (needRemoveInDict)
            {
                triggerMethodDict.Remove(eventName);
            }

            var eventObj = DataBindUtil.GetCompPropObj(component, eventName);
            if (eventObj == null)
            {
                return;
            }
            
            //获取移除监听方法
            var removeListenerMethod = DataBindUtil.GetRemoveListenerMethodInfo(eventObj);
            if (removeListenerMethod == null)
            {
                return;
            }
            
            var p = new object[] { triggerDelegate };

            bool isSuccess = true;
            try
            {
                removeListenerMethod.Invoke(eventObj, p);
                
            }
            catch (Exception e)
            {
                isSuccess = false;
                Console.WriteLine(e);
                throw;
            }
            
        }

        private bool _CanEventBinder(Component component, string eventName, string triggerMethodName)
        {
            if (goutil.IsNil(component))
            {
                Debug.LogError($"绑定参数类型错误, component 不得为空");
                return false;
            }

            if (string.IsNullOrEmpty(eventName))
            {
                Debug.LogError($"绑定参数类型错误, eventName 不得为空");
                return false;
            }
            
            if (string.IsNullOrEmpty(triggerMethodName))
            {
                Debug.LogError($"绑定参数类型错误, triggerMethodName 不得为空");
                return false;
            }

            var eventObj = DataBindUtil.GetCompPropObj(component, eventName);
            if (eventObj == null)
            {
                Debug.LogError($"绑定组件不存在事件对象, component:{component.GetType()}, eventName:{eventName}");
                return false;
            }
            
            //判断监听事件的类型
            var addListenerMethod = DataBindUtil.GetAddListenerMethodInfo(eventObj);
            if (addListenerMethod == null)
            {
                Debug.LogError($"绑定组件事件不存在AddListener方法, component:{component.GetType()}, eventName:{eventName}");
                return false;
            }

            Type eventType;
            string errorTip = ""; 
            if(!DataBindUtil.IsOnlyUnityActionParam(addListenerMethod, out eventType, out errorTip))
            {
                Debug.LogError($"绑定组件事件AddListener方法不合规, component:{component.GetType()}, eventName:{eventName}, {errorTip}");
                return false;
            }
            
            
            //判断监听方法的参数类型
            var eventArgTypes = eventType.GetGenericArguments();
            var triggerMethodInfo = DataBindUtil.GetMethodInfo(_viewModel, triggerMethodName);
            if (triggerMethodInfo == null)
            {
                Debug.LogError($"绑定触发方法不存在, bindObj:{_viewModel.GetType()}, bindMethodName:{triggerMethodName}");
                return false;
            }
            
            var triggerMethodParamTypes = DataBindUtil.GetParamTypesByMethodInfo(triggerMethodInfo);
            if (triggerMethodParamTypes == null || !triggerMethodParamTypes.SequenceEqual(eventArgTypes))
            {
                string bindEventParamTypes = eventArgTypes == null ? "null" :
                    string.Concat(eventArgTypes.Select(item => item.Name).ToArray(), ", ");
                string showBindParamTypes = triggerMethodParamTypes == null ? "null" :
                    string.Concat(triggerMethodParamTypes.Select(item => item.Name).ToArray(), ", ");
                Debug.LogError(string.Format("绑定触发方法类型错误, compontType:{0}, bindObj:{1}, bindEventParamTypes:{2}, bindMethodName:{3}, bindMethodParamTypes:{4}", 
                    component.GetType(), bindEventParamTypes, triggerMethodName, showBindParamTypes));
                return false;
            }

            return true;
        }
    }
}