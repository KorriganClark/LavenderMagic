using Lavender.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender.UnityFramework
{
    class BaseComponent : FrameworkComponent
    {
        /// <summary>
        /// 驱动游戏框架更新
        /// </summary>
        private void Update()
        {
            FrameworkControl.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        public void ShutDown()
        {
            Destroy(gameObject);
        }
    }
}
