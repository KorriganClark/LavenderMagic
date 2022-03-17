using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework.ResourceManager
{

    public interface IResourceManager
    {

        void SetLoader(IAssetLoader assetLoader, IAssetBundleLoader assetBundleLoader);

        T GetAssetAsync<T>(string assetPath, Action<LoadTask> loadCallBack = null) where T : AssetTask, new();

        AssetBundleTask GetAssetBundleAsync(string assetBundlePath);

        void SetDependentABs(Dictionary<string, string> dep);

        //LoadTask LoadAsset(string assetPath, ,Action<LoadTask> loadCallBack);

        void UnloadAsset(object asset);
    }
}
