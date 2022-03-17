using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework.ResourceManager
{
    internal sealed partial class ResourceManager : FrameworkModule, IResourceManager
    {

        private readonly TaskPool<AssetBundleTask> abTaskPool = new TaskPool<AssetBundleTask>();//AB 加载池
        private readonly TaskPool<AssetTask> assetTaskPool = new TaskPool<AssetTask>();//Asset 加载池
        private readonly Dictionary<string, object> assetPool = new Dictionary<string, object>();//资源池
        private readonly Dictionary<string, object> abPool = new Dictionary<string, object>();//AB 池
        private readonly Dictionary<string, string> dependentABs = new Dictionary<string, string>();//资源依赖的包

        public IAssetLoader AssetLoader { get; set; }
        public IAssetBundleLoader AssetBundleLoader { get; set; }

        public ResourceManager()
        {

        }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal override int Priority
        {
            get
            {
                return 3;
            }
        }

        public void SetLoader(IAssetLoader assetLoader, IAssetBundleLoader assetBundleLoader)
        {
            AssetLoader = assetLoader;
            AssetBundleLoader = assetBundleLoader;
        }

        /// <summary>
        /// 资源、AB包任务池轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑时间</param>
        /// <param name="realElapseSeconds">真实时间</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            abTaskPool.UpdateAll();
            assetTaskPool.UpdateAll();
        }

        /// <summary>
        /// 异步获取资源，如果资源已加载，转为同步
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="loadCallBack"></param>
        /// <returns></returns>
        public T GetAssetAsync<T>(string assetPath, Action<LoadTask> loadCallBack) where T : AssetTask, new()
        {
            if(assetPool.TryGetValue(assetPath, out var asset))
            {
                var assetTask = ReferencePool.Acquire<T>();
                assetTask.Completed = loadCallBack;
                assetTask.SyncGet(asset);
                loadCallBack?.Invoke(assetTask);
                return assetTask;
            }
            AssetBundleTask abTask = GetAssetBundleAsync(assetPath);
            if(abTask != null)
            {
                var assetTask = LoadAsset<T>(assetPath, abTask, loadCallBack);
                return assetTask;
            }
            else
            {
                var assetTask = ReferencePool.Acquire<T>();
                assetTask.AssetPath = assetPath;
                assetTask.SyncGet(AssetLoader);
                return assetTask;
            }
            
        }

        /// <summary>
        /// 异步获取AB包
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public AssetBundleTask GetAssetBundleAsync(string assetPath)
        {
            var abTask = ReferencePool.Acquire<AssetBundleTask>();
            if(abPool.TryGetValue(assetPath,out var ab))
            {
                abTask.SyncGet(ab);
                return abTask;
            }
            if (dependentABs.TryGetValue(assetPath, out var abPath))
            {
                abTask.AssetBundlePath = abPath;
                abTask.Completed += (LoadTask loadTask) =>
                {
                    if (loadTask.result != null)
                    {
                        abPool.Add(abPath, loadTask.result);
                    }
                };
                abTask.Start(abTaskPool, AssetBundleLoader);
                return abTask;
            }
            ReferencePool.Release(abTask);
            return null;
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="loadCallBack"></param>
        /// <returns></returns>
        public T LoadAsset<T>(string assetPath, AssetBundleTask abTask, Action<LoadTask> loadCallBack) where T : AssetTask, new()
        {
            var assetTask = ReferencePool.Acquire<T>();
            assetTask.AssetPath = assetPath;
            assetTask.Completed += (LoadTask loadTask) =>
            {
                if (loadTask.result != null)
                {
                    assetPool.Add(assetPath, loadTask.result);
                }
            };
            if(loadCallBack != null)
            {
                assetTask.Completed += loadCallBack;
            }
            assetTask.Start(assetTaskPool, AssetLoader);
            return assetTask;
        }


        /// <summary>
        /// 卸载资源
        /// </summary>
        /// <param name="asset"></param>
        public void UnloadAsset(object asset)
        {
            throw new NotImplementedException();
        }

        internal override void Shutdown()
        {
            throw new NotImplementedException();
        }

        public void SetDependentABs(Dictionary<string, string> dep)
        {
            throw new NotImplementedException();
        }
    }
}
