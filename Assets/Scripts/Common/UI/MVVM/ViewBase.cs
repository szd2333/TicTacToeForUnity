using System;
using System.Collections.Generic;
using UnityEngine;

namespace TTT
{
    public abstract class ViewBase : MonoBehaviour
    {
        protected object[] initParamObjects;
        protected ViewModelBase _viewModel;
        private PropertyBinder _propertyBinder;
        private EventBinder _eventBinder;
        private HashSet<ViewBase> _subViewHash = new HashSet<ViewBase>();

        #region 公共方法

        public abstract Type ViewModelType { get; }
        public abstract UIRootType RootType { get; }

        public void Open(object[] paramObjects, ViewModelBase viewModel)
        {
            initParamObjects = paramObjects;
            _viewModel = viewModel;
            if (_viewModel == null)
            {
                Debug.LogError("viewModel不得为空");
                return;
            }
            viewModel.OnInit();
            BindValues();
            BindEvents();
            OnOpenFinish();
            viewModel.OnActive();
        }

        public void Close()
        {
            Dispose();
            OnCloseFinish();
        }

        #endregion

        #region ViewBase生命周期虚函数

        protected virtual void BindValues()
        {
            
        }

        protected virtual void BindEvents()
        {
            
        }

        protected virtual void OnOpenFinish()
        {
            
        }

        protected virtual void OnCloseFinish()
        {
            
        }

        #endregion

        #region 绑定参数

        protected void BindValue(UnityEngine.Object bindObj, Observer observer, string arg)
        {
            if (_propertyBinder == null)
            {
                _propertyBinder = new PropertyBinder();
            }
            _propertyBinder.AddValueBinder(bindObj, observer, arg);
        }

        protected void ClearBindValues()
        {
            if (_propertyBinder == null)
            {
                return;
            }
            _propertyBinder.RemoveAllValueBinder();
        }

        #endregion
        
        #region 绑定事件

        protected void BindEvent(Component component, string eventName, string triggerMethodName)
        {
            if (_eventBinder == null)
            {
                _eventBinder = new EventBinder(this._viewModel);
            }
            _eventBinder.AddEventBinder(component, eventName, triggerMethodName);
        }

        protected void ClearBindEvents()
        {
            if (_eventBinder == null)
            {
                return;
            }
            _eventBinder.RemoveAllEventBinder();
        }

        #endregion

        #region 绑定子界面
        protected void BindSubView(ViewBase subView, ViewModelBase subViewModel)
        {
            if (subView == null || subViewModel == null)
            {
                return;
            }

            if (_subViewHash.Contains(subView))
            {
                return;
            }

            _subViewHash.Add(subView);
            subView.Open(null, subViewModel);
        }

        protected void ClearAllSubView()
        {
            foreach (ViewBase subView in _subViewHash)
            {
                subView.Close();
            }
            _subViewHash.Clear();
        }
        
        #endregion

        protected void Dispose()
        {
            ClearBindValues();
            ClearBindEvents();
            ClearAllSubView();
            if (_viewModel != null)
            {
                _viewModel.OnDispose();
            }
        }
    }

}