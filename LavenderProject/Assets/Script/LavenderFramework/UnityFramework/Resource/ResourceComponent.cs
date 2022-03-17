using System;
using UnityEngine;
using Lavender.Framework.ResourceManager;
using Lavender.Framework;
using System.Collections;

namespace Lavender.UnityFramework
{
    [DisallowMultipleComponent]
    public sealed class ResourceComponent : FrameworkComponent
    {
        private IResourceManager resourceManager = null;

        protected override void Awake()
        {
            base.Awake();
            resourceManager = FrameworkControl.GetModule<IResourceManager>();
            if(resourceManager == null)
            {
                throw new Exception("ResourceManager is not Valid!");
            }
            InitManager();
        }

        private void InitManager()
        {
            resourceManager.SetLoader(gameObject.AddComponent<AssetLoaderComponent>(), gameObject.AddComponent<AssetBundleLoaderComponent>());
        }

        /// <summary>
        /// 加载对应的资源
        /// </summary>
        /// <param name="assetPath"></param>
        /// <param name="loadCallBack"></param>
        /// <returns></returns>
        public Asset GetAssetAsync(string assetPath, Action<Asset> loadCallBack = null)
        {
            if (loadCallBack != null)
            {
                return resourceManager.GetAssetAsync<Asset>(assetPath, (Action<LoadTask>)loadCallBack);
                
            }
            else
            {
                return resourceManager.GetAssetAsync<Asset>(assetPath);
            }

        }
    }
}
