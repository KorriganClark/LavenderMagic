using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework.ResourceManager
{
    public class AssetBundleTask : LoadTask
    {
        /// <summary>
        /// AB路径
        /// </summary>
        public string AssetBundlePath { get; set; }

        public object AB { get; set; }

        /// <summary>
        /// 加载器
        /// </summary>
        public IAssetBundleLoader AssetBundleLoader { get; set; }

        protected override void Run()
        {
            AssetBundleLoader.LoadAsync(AssetBundlePath);
        }

        /// <summary>
        /// 同步获取，结束异步加载
        /// </summary>
        /// <param name="asset"></param>
        public override void SyncGet(object assetBundle)
        {
            AB = assetBundle;
            Finish();
        }

        public override void Update()
        {
            base.Update();
            if (AssetBundleLoader.IsDone(this))
            {
                AB = AssetBundleLoader.GetResult(this);
                if (AB != null)
                {
                    Finish();
                }
                else
                {
                    Finish("Null AB");
                }
            }
        }
    }
}
