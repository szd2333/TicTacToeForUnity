using System;
using System.Linq;
using System.Reflection;
using TTT;
using UnityEngine;
using UnityEngine.Events;

namespace MVVM
{
    public class DataBindUtil
    {
        public static PropertyInfo GetCompPropInfo(Component component, string propName)
        {
            if (goutil.IsNil(component) || string.IsNullOrEmpty(propName))
            {
                return null;
            }

            var compType = component.GetType();
            var propInfo = compType.GetProperty(propName);

            return propInfo;
        }

        public static object GetCompPropObj(Component component, string propName)
        {
            var propInfo = GetCompPropInfo(component, propName);
            if (propInfo == null)
            {
                return null;
            }

            return propInfo.GetValue(component);
        }

        public static MethodInfo GetMethodInfo(object obj, string methodName)
        {
            if (obj == null || string.IsNullOrEmpty(methodName))
            {
                return null;
            }

            Type objType = obj.GetType();
            
            return objType.GetMethod(methodName);
        }

        public static MethodInfo GetAddListenerMethodInfo(object obj)
        {
            return GetMethodInfo(obj, "AddListener");
        }
        
        public static MethodInfo GetRemoveListenerMethodInfo(object obj)
        {
            return GetMethodInfo(obj, "RemoveListener");
        }

        public static Type[] GetParamTypesByMethodInfo(MethodInfo methodInfo)
        {
            if (methodInfo == null)
            {
                return new Type[]{};
            }

            var methodParams = methodInfo.GetParameters();
            return methodParams.Select(methodParam => methodParam.ParameterType).ToArray();
        }

        /// <summary>
        /// 判断指定方法是否有且只有一个UnityAction类型参数
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="unityActionType">返回方法参数</param>
        /// <returns></returns>
        public static bool IsOnlyUnityActionParam(MethodInfo methodInfo, out Type unityActionType, out string errorTip)
        {
            Type[] paramTypes = GetParamTypesByMethodInfo(methodInfo);
            if (paramTypes == null || paramTypes.Length != 1)
            {
                unityActionType = null;
                errorTip = "不是只有unityAction类型";
                return false;
            }
            
            var firstType = paramTypes[0];
            Type checkType = typeof(UnityAction);
            
            if (firstType != checkType)
            {
                if (!firstType.IsGenericType)
                {
                    unityActionType = null;
                    errorTip = "不是只有unityAction类型";
                    return false;
                }
                
                //泛型特殊处理
                var genericType = firstType.GetGenericTypeDefinition();
                var checkGenericType = typeof(UnityAction<>);
                if (genericType != checkGenericType)
                {
                    unityActionType = null;
                    errorTip = "不是只有unityAction类型";
                    return false;
                }
            }
            unityActionType = firstType;
            errorTip = null;
            return true;
        }
    }
}