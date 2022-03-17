using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender.UnityFramework
{
    /// <summary>
    /// 框架组件控制核心，用于注册控件、获取控件、游戏结束时销毁核心组件
    /// </summary>
    public static class FrameworkComponentControl
    {
        private static readonly List<FrameworkComponent> frameworkComponents = new List<FrameworkComponent>();

        public static T GetComponent<T>() where T : FrameworkComponent
        {
            return (T)GetComponent(typeof(T));
        }

        public static FrameworkComponent GetComponent(Type type)
        {
            foreach(var component in frameworkComponents)
            {
                if(component.GetType() == type)
                {
                    return component;
                }
            }
            throw new Exception($"No FrameworkComponent: {type.FullName}!");
        }

        /// <summary>
        /// 停止核心组件运作
        /// </summary>
        public static void ShutDown()
        {
            BaseComponent baseComponent = GetComponent<BaseComponent>();
            if(baseComponent != null)
            {
                baseComponent.ShutDown();
            }
        }

        /// <summary>
        /// 注册组件
        /// </summary>
        /// <param name="frameworkComponent"></param>
        internal static void RegisterComponent(FrameworkComponent frameworkComponent)
        {
            if(frameworkComponent == null)
            {
                Debug.LogError("FrameworkComponent is Null!");
            }

            Type type = frameworkComponent.GetType();
            foreach(var component in frameworkComponents)
            {
                if(type == component.GetType())
                {
                    Debug.LogError("FrameworkComponent already Existed!");
                    return;
                }
            }
            frameworkComponents.Add(frameworkComponent);
        }
    }
}
