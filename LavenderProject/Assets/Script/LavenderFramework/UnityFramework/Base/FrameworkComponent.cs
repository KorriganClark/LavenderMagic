using UnityEngine;

namespace Lavender.UnityFramework
{
    public abstract class FrameworkComponent : MonoBehaviour
    {
        /// <summary>
        /// 框架组件基类,
        /// </summary>
        protected virtual void Awake()
        {
            FrameworkComponentControl.RegisterComponent(this);
        }
    }
}
