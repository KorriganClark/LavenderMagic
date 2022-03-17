using Lavender.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lavender.UnityFramework
{
    public class BaseComponent : FrameworkComponent
    {

        /// <summary>
        /// 驱动游戏框架更新
        /// </summary>
        protected virtual void Update()
        {
            //LavenderGameMode.Update();
            FrameworkControl.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        public void ShutDown()
        {
            Destroy(gameObject);
        }
    }
}
