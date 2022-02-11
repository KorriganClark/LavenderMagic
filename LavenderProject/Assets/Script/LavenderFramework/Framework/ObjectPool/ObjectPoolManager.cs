using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework.ObjectPool
{
    internal sealed partial class ObjectPoolManager : FrameworkModule, IObjectPoolManager
    {
        private const int DefaultCapacity = int.MaxValue;
        private const float DefaultExpireTime = float.MaxValue;
        private const int DefaultPriority = 0;

        private readonly Dictionary<string, ObjectPoolBase> objectPools;
        private readonly List<ObjectPoolBase> cashedAllObjectPools;
        private readonly Comparison<ObjectPoolBase> objectPoolComparer;

        public ObjectPoolManager()
        {
            objectPools = new Dictionary<string, ObjectPoolBase>();
            cashedAllObjectPools = new List<ObjectPoolBase>();
            objectPoolComparer = ObjectPoolComparer;
        }

        /// <summary>
        /// 游戏框架模块优先级
        /// </summary>
        internal override int Priority
        {
            get
            {
                return 6;
            }
        }

        /// <summary>
        /// 对象池数量
        /// </summary>
        public int Count
        {
            get
            {
                return objectPools.Count;
            }
        }

        /// <summary>
        /// 对象池管理器轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑时间</param>
        /// <param name="realElapseSeconds">真实时间</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach(var objectPoolKeyValue in objectPools)
            {
                objectPoolKeyValue.Value.Update(elapseSeconds, realElapseSeconds);
            }
        }

        internal override void Shutdown()
        {
            foreach(var objectPoolKeyValue in objectPools)
            {
                objectPoolKeyValue.Value.Shutdown();
            }
            objectPools.Clear();
            cashedAllObjectPools.Clear();
        }

        public bool HasObjectPool(string name)
        {
            return objectPools.ContainsKey(name);
        }

        public bool HasObjectPool(Type objectType)
        {
            if(objectType == null)
            {
                throw new Exception("Type is null!");
            }
            
            foreach(var objectPool in objectPools)
            {
                if(objectPool.Value.GetType() == objectType)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <param name="objectType">对象类型。</param>
        /// <returns>要获取的对象池。</returns>
        public ObjectPoolBase GetObjectPool(Type objectType)
        {
            if (objectType == null)
            {
                throw new Exception("Object type is invalid.");
            }

            foreach (var objectPool in objectPools)
            {
                if (objectPool.Value.ObjectType == objectType)
                {
                    return objectPool.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取对象池。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam>
        /// <param name="name">对象池名称。</param>
        /// <returns>要获取的对象池。</returns>
        public IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase
        {
            foreach (var objectPool in objectPools)
            {
                if (objectPool.Value.ObjectType == typeof(T) && name == objectPool.Value.Name)
                {
                    return (IObjectPool<T>)objectPool.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取所有对象池。
        /// </summary>
        /// <param name="sort">是否根据对象池的优先级排序。</param>
        /// <returns>所有对象池。</returns>
        public ObjectPoolBase[] GetAllObjectPools(bool sort = false)
        {
            if (sort)
            {
                List<ObjectPoolBase> results = new List<ObjectPoolBase>();
                foreach (var objectPool in objectPools)
                {
                    results.Add(objectPool.Value);
                }

                results.Sort(objectPoolComparer);
                return results.ToArray();
            }
            else
            {
                int index = 0;
                ObjectPoolBase[] results = new ObjectPoolBase[objectPools.Count];
                foreach (var objectPool in objectPools)
                {
                    results[index++] = objectPool.Value;
                }
                return results;
            }
        }

        public IObjectPool<T> CreateObjectPool<T>(string name, bool allowMultiSpawn = false, float autoReleaseInterval = DefaultExpireTime, int capacity = DefaultCapacity, float expireTime = DefaultExpireTime, int priority = DefaultPriority) where T : ObjectBase
        {
            if (HasObjectPool(name))
            {
                throw new Exception("Already exist this Object Pool!");
            }
            ObjectPool<T> objectPool = new ObjectPool<T>(name, allowMultiSpawn, autoReleaseInterval, capacity, expireTime, priority);
            objectPools.Add(name, objectPool);
            return objectPool;
        }

        public bool DestroyObjectPool(string name)
        {
            ObjectPoolBase objectPool = null;
            if (objectPools.TryGetValue(name, out objectPool))
            {
                objectPool.Shutdown();
                return objectPools.Remove(name);
            }
            return false;
        }


        private static int ObjectPoolComparer(ObjectPoolBase a, ObjectPoolBase b)
        {
            return a.Priority.CompareTo(b.Priority);
        }
    }
}
