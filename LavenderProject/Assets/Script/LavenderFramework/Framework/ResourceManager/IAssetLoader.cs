using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework.ResourceManager
{
    public interface IAssetLoader : ILoader
    {

        /// <summary>
        /// 同步加载
        /// </summary>
        object LoadImmediate(string assetPath);

        /// <summary>
        /// 异步加载
        /// </summary>
        void LoadAsync(object assetBundle, string assetPath, LoadTask loadTask);
    }
}
