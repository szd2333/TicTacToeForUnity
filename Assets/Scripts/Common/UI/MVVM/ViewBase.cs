using UnityEngine;

namespace MVVM
{
    public abstract class ViewBase<T> : MonoBehaviour where T : ViewModelBase
    {
        protected T _viewModel;
        private PropertyBinder _propertyBinder;
        private EventBinder _eventBinder;

        #region 公共方法

        public void Open(T viewMdoel)
        {
            _viewModel = viewMdoel;
            BindValues();
            BindEvents();
            OnOpenFinish();
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

        protected void BindValue(Component component, Observer observer, string arg)
        {
            if (_propertyBinder == null)
            {
                _propertyBinder = new PropertyBinder();
            }
            _propertyBinder.AddValueBinder(component, observer, arg);
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
        
        protected void Dispose()
        {
            ClearBindValues();
        }
    }

}