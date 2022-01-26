using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework
{
    internal abstract class FrameworkModule
    {
        /// <summary>
        /// 轮询更新
        /// </summary>
        /// <param name="elapseSeconds">逻辑时间</param>
        /// <param name="realElapseSeconds">真实时间</param>
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 关闭游戏模块
        /// </summary>
        internal abstract void Shutdown();

    }
}
