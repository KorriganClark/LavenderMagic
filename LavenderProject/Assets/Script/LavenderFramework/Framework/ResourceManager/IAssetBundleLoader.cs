using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework.ResourceManager
{
    public interface IAssetBundleLoader : ILoader
    {
        /// <summary>
        /// 异步加载
        /// </summary>
        void LoadAsync(string assetBundlePath);

    }
}
