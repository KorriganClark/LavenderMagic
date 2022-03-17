using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework.ResourceManager
{
    public enum AssetLoadStatus
    {
        Loading,
        DependentLoading,
    }

    public class AssetTask : LoadTask
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public string AssetPath { get; set; }

        /// <summary>
        /// AB 加载任务
        /// </summary>
        public AssetBundleTask ABTask { get; set; }

        public object AB { get; set; }

        /// <summary>
        /// 资源对象
        /// </summary>
        public object Asset { get; set; }

        private AssetLoadStatus assetLoadStatus = AssetLoadStatus.DependentLoading;

        /// <summary>
        /// 加载器
        /// </summary>
        public IAssetLoader AssetLoader 
        { 
            get 
            {
                return (IAssetLoader)base.Loader;
            }
            set
            {
                base.Loader = value;
            }
        }

        protected override void Run()
        {
            if(ABTask == null)
            {
                assetLoadStatus = AssetLoadStatus.Loading;
            }
            else if(ABTask.status == TaskStatus.Waiting)
            {
                throw new Exception("ABTask Not Running");
            }
            //AssetLoader.LoadAsync(AB, AssetPath, this);
        }

        /// <summary>
        /// 同步获取，结束异步加载
        /// </summary>
        /// <param name="asset"></param>
        public void SyncGet(IAssetLoader assetLoader)
        {
            AssetLoader = assetLoader;
            Asset = AssetLoader.LoadImmediate(AssetPath);
            Finish();
        }

        public override void Update()
        {
            base.Update();
            switch (assetLoadStatus)
            {
                case AssetLoadStatus.DependentLoading:
                    UpdateABLoading();
                    break;
                case AssetLoadStatus.Loading:
                    UpdateLoading();
                    break;
            }
        }

        /// <summary>
        /// 更新AB包加载状态
        /// </summary>
        public void UpdateABLoading()
        {
            if (!ABTask.isDone)
            {
                return;
            }
            AB = ABTask.result;
            if(AB == null)
            {
                Asset = AssetLoader.LoadImmediate(AssetPath);
                if (Asset != null)
                {
                    Finish();
                }
                else
                {
                    Finish("Null Asset");
                }
            }
            else
            {
                AssetLoader.LoadAsync(AB, AssetPath, this);
                assetLoadStatus = AssetLoadStatus.Loading;
            }
        }

        /// <summary>
        /// 更新加载状态
        /// </summary>
        public void UpdateLoading()
        {
            if (AssetLoader.IsDone(this))
            {
                Asset = AssetLoader.GetResult(this);
                if (Asset != null)
                {
                    Finish();
                }
                else
                {
                    Finish("Null Asset");
                }
            }
        }
    }
}
