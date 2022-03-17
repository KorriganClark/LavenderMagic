using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender.Framework
{
    public static class FrameworkControl
    {
        private static readonly List<FrameworkModule> frameworkModules = new List<FrameworkModule>();

        private static float _realtimeSinceUpdateStartup;

        private static float _maxUpdateTimeSlice = 0.01f;
        public static float maxUpdateTimeSlice { get { return _maxUpdateTimeSlice; } set { _maxUpdateTimeSlice = value; } } 
        public static bool busy => Time.realtimeSinceStartup - _realtimeSinceUpdateStartup >= maxUpdateTimeSlice;

        /// <summary>
        /// 轮询更新，需要上层调用
        /// </summary>
        /// <param name="elapseSeconds">逻辑时间</param>
        /// <param name="realElapseSeconds">真实时间</param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            _realtimeSinceUpdateStartup = Time.realtimeSinceStartup;
            foreach (var module in frameworkModules)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理所有游戏框架模块。
        /// </summary>
        public static void ShutDown()
        {
            foreach(var module in frameworkModules)
            {
                module.Shutdown();
            }
            frameworkModules.Clear();
            ReferencePool.ClearAll();
        }

        /// <summary>
        /// 获得模块接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new Exception($"You must get module by interface, but {interfaceType.FullName} is not!");
            }
            string moduleName = interfaceType.Namespace + "." + interfaceType.Name.Substring(1);
            Type moduleType = Type.GetType(moduleName);
            if(moduleName == null)
            {
                throw new Exception($"Not have {moduleName}");
            }
            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 创建模块，模块为内部类，无法直接给外部调用，外部只能调用接口
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        private static FrameworkModule GetModule(Type moduleType)
        {
            foreach(var module in frameworkModules)
            {
                if(module.GetType() == moduleType)
                {
                    return module;
                }
            }
            var res = (FrameworkModule)Activator.CreateInstance(moduleType);
            frameworkModules.Add(res);
            return res;
        }



    }
}
