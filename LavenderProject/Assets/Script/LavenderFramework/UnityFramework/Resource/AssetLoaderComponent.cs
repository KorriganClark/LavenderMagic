using Lavender.Framework.ResourceManager;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Lavender.UnityFramework
{
    public class AssetLoaderComponent : MonoBehaviour, IAssetLoader
    {

        private readonly Dictionary<LoadTask, AssetBundleRequest> requests = new Dictionary<LoadTask, AssetBundleRequest>();

        public object GetResult(LoadTask loadTask)
        {
            if(requests.TryGetValue(loadTask, out var request))
            {
                if (request.isDone)
                {
                    requests.Remove(loadTask);
                    return request.asset;
                }
                else
                {
                    throw new System.Exception("Not Done!");
                }
            }
            throw new System.Exception("No Request!");
        }

        public bool IsDone(LoadTask loadTask)
        {
            if (requests.TryGetValue(loadTask, out var request))
            {
                return request.isDone;
            }
            throw new System.Exception("No Request!");
        }

        public void LoadAsync(object assetBundle, string assetPath, LoadTask loadTask)
        {
            requests.Add(loadTask, (assetBundle as AssetBundle).LoadAssetAsync(assetPath));
        }

        object IAssetLoader.LoadImmediate(string assetPath)
        {
            return AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject));
        }
    }
}
