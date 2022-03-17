using Lavender.Framework;
using Lavender.Framework.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender.UnityFramework
{
    [DisallowMultipleComponent]
    public sealed class ObjectPoolComponent : FrameworkComponent
    {
        private IObjectPoolManager objectPoolManager = null;

        public int Count
        {
            get
            {
                return objectPoolManager.Count;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            objectPoolManager = FrameworkControl.GetModule<IObjectPoolManager>();
            if(objectPoolManager == null)
            {
                throw new Exception("objectPoolManager is not valid");
            }
        }

        public IObjectPool<T> GetObjectPool<T>(string name) where T: ObjectBase
        {
            return objectPoolManager.GetObjectPool<T>(name);
        }

        public IObjectPool<T> CreateObjectPool<T>(string name) where T : ObjectBase
        {
            return objectPoolManager.CreateObjectPool<T>(name);
        }
    }
}
