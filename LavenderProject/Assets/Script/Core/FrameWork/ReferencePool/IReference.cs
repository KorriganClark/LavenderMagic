using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework
{
    /// <summary>
    /// 引用池使用，实现该接口以进入引用池
    /// </summary>
    public interface IReference
    {
        /// <summary>
        /// 回池时调用
        /// </summary>
        public void Clear();
    }
}
