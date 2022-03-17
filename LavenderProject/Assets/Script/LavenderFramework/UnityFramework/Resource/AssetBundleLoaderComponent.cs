using Lavender.Framework.ResourceManager;
using UnityEngine;

namespace Lavender.UnityFramework
{
    public class AssetBundleLoaderComponent : MonoBehaviour, IAssetBundleLoader
    {
        public object GetResult(LoadTask loadTask)
        {
            throw new System.NotImplementedException();
        }

        public bool IsDone(LoadTask loadTask)
        {
            throw new System.NotImplementedException();
        }

        public void LoadAsync(string assetBundlePath)
        {
            throw new System.NotImplementedException();
        }
    }
}
