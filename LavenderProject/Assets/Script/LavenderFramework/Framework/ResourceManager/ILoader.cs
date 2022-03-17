using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework.ResourceManager
{
    public interface ILoader
    {
        /// <summary>
        /// 获取加载结果,获取之后自动销毁相应加载任务
        /// </summary>
        /// <returns></returns>
        object GetResult(LoadTask loadTask);

        /// <summary>
        /// 是否加载完毕
        /// </summary>
        /// <returns></returns>
        bool IsDone(LoadTask loadTask);
    }
}
