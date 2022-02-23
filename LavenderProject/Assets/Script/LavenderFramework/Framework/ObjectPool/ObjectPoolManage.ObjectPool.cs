using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework.ObjectPool
{
    internal sealed partial class ObjectPoolManager : FrameworkModule, IObjectPoolManager
    {
        /// <summary>
        /// 对象池
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        private sealed class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
        {
            //存储所有对象代理的集合，以具体对象为键
            private readonly Dictionary<object, Object<T>> objectMap;
            //可以释放的对象，所有未使用，未上锁的对象
            private readonly List<T> cachedCanReleaseObjects;
            //将要释放的对象，根据容器大小、回收周期决定
            private readonly List<T> cachedToReleaseObjects;
            //允许对同一个对象使用多次
            private readonly bool allowMultiSpawn;
            //自动进行对象释放的周期
            private float autoReleaseInterval;
            //容量，当池中对象数超过该值时立即执行释放逻辑，但不一定会释放到小于该值，因为可能剩下的都被占用着
            private int capacity;
            //过期时间，用于释放
            private float expireTime;
            //优先级
            private int priority;
            //据上次释放的时间，用于自动释放
            private float autoReleaseTime;

            public ObjectPool(string name, bool allowMultiSpawn, float autoReleaseInterval, int capacity, float expireTime, int priority)
                : base(name)
            {
                objectMap = new Dictionary<object, Object<T>>();
                cachedCanReleaseObjects = new List<T>();
                cachedToReleaseObjects = new List<T>();
                this.allowMultiSpawn = allowMultiSpawn;
                this.autoReleaseInterval = autoReleaseInterval;
                Capacity = capacity;
                ExpireTime = expireTime;
                this.priority = priority;
                autoReleaseTime = 0f;
            }

            /// <summary>
            /// 对象池类型
            /// </summary>
            public override Type ObjectType
            {
                get
                {
                    return typeof(T);
                }
            }

            public override int Count
            {
                get
                {
                    return objectMap.Count;
                }
            }

            public override int CanReleaseCount
            {
                get
                {
                    GetCanReleaseObjects(cachedCanReleaseObjects);
                    return cachedCanReleaseObjects.Count;
                }
            }

            public override bool AllowMultiSpawn
            {
                get
                {
                    return allowMultiSpawn;
                }
            }

            public override float AutoReleaseInterval 
            { 
                get
                {
                    return autoReleaseInterval;
                }
                set
                {
                    autoReleaseInterval = value;
                }
            }

            public override int Capacity
            {
                get
                {
                    return capacity;
                }
                set
                {
                    if(value < 0)
                    {
                        throw new Exception("value is node valid");
                    }

                    if(capacity == value)
                    {
                        return;
                    }
                    capacity = value;
                    Release();
                }
            }

            public override float ExpireTime
            {
                get
                {
                    return expireTime;
                }
                set
                {
                    if(value < 0f)
                    {
                        throw new Exception("ExpireTime is invalid");
                    }

                    if(expireTime == value)
                    {
                        return;
                    }

                    expireTime = value;
                    Release();
                }
            }

            public override int Priority 
            {
                get
                {
                    return priority;
                }
                set
                {
                    priority = value;
                }
            }

            /// <summary>
            /// 注册对象
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="spawned"></param>
            public void Register(T obj,bool spawned)
            {
                if(obj == null)
                {
                    throw new Exception("Object is invalid");
                }

                Object<T> objectProxy = Object<T>.Create(obj, spawned);
                objectMap.Add(obj.Target, objectProxy);

                if(Count > capacity)
                {
                    Release();
                }
            }

            /// <summary>
            /// 能否生成
            /// </summary>
            /// <param name="target">目标</param>
            /// <returns></returns>
            public bool CanSpawn(object target)
            {
                if(target == null)
                {
                    throw new Exception("Target is invalid.");
                }
                Object<T> objectProxy;
                if (objectMap.TryGetValue(target, out objectProxy))
                {
                    if (allowMultiSpawn || !objectProxy.IsInUse)
                    {
                        return true;
                    }
                }
                return false;
            }

            public T Spawn()
            {
                foreach(var proxyPair in objectMap)
                {
                    if(allowMultiSpawn || !proxyPair.Value.IsInUse)
                    {
                        return proxyPair.Value.Spawn();
                    }
                }
                return null;
            }

            public void Unspawn(T proxy)
            {
                Unspawn(proxy.Target);
            }

            public void Unspawn(object target)
            {
                var proxy = GetObject(target);
                if(proxy != null)
                {
                    proxy.Unspawn();
                    if(Count > capacity && proxy.SpawnCount <= 0)
                    {
                        Release();
                    }
                }
                else
                {
                    throw new Exception("No target!");
                }
            }

            public bool ReleaseObject(T obj)
            {
                if(obj == null)
                {
                    throw new Exception("No object");
                }
                return ReleaseObject(obj.Target);
            }

            public bool ReleaseObject(object target)
            {
                if(target == null)
                {
                    throw new Exception("No target!");
                }
                var proxy = GetObject(target);
                if(proxy == null)
                {
                    return false;
                }
                if(proxy.IsInUse || proxy.Locked || !proxy.CustomCanReleaseFlag)
                {
                    return false;
                }
                objectMap.Remove(proxy.GetTarget().Target);
                proxy.Release(false);
                ReferencePool.Release(proxy);
                return true;
            }

            public override void Release()
            {
                Release(Count - capacity);
            }

            public override void Release(int toReleaseCount)
            {
                if(toReleaseCount < 0)
                {
                    toReleaseCount = 0;
                }
                DateTime expireFlag = DateTime.MinValue;
                if(expireTime < float.MaxValue)
                {
                    expireFlag = DateTime.UtcNow.AddSeconds(-expireTime);//该节点之前的都应当释放
                }

                autoReleaseTime = 0f;
                GetCanReleaseObjects(cachedCanReleaseObjects);
                List<T> toReleaseObjects = DefaultReleaseObjectFiler(cachedCanReleaseObjects, toReleaseCount, expireFlag);
                if(toReleaseObjects == null || toReleaseObjects.Count == 0)
                {
                    return;
                }
                foreach(T toReleaseObject in toReleaseObjects)
                {
                    ReleaseObject(toReleaseObject);
                }
            }

            internal override void Update(float elapseSeconds, float realElapseSeconds)
            {
                autoReleaseTime += realElapseSeconds;
                if(autoReleaseTime < autoReleaseInterval)
                {
                    return;
                }
                Release();
            }

            internal override void Shutdown()
            {
                foreach(var pair in objectMap)
                {
                    pair.Value.Release(true);
                    ReferencePool.Release(pair.Value);
                }

                objectMap.Clear();
                cachedCanReleaseObjects.Clear();
                cachedToReleaseObjects.Clear();
            }

            private Object<T> GetObject(object target)
            {
                if (target == null)
                {
                    throw new Exception("Target is invalid.");
                }

                Object<T> proxy = null;
                if (objectMap.TryGetValue(target, out proxy))
                {
                    return proxy;
                }

                return null;
            }

            /// <summary>
            /// 筛选出能够释放的对象
            /// </summary>
            /// <param name="results"></param>
            private void GetCanReleaseObjects(List<T> results)
            {
                if(results == null)
                {
                    throw new Exception("Results is Invalid");
                }
                results.Clear();
                foreach(var objectPair in objectMap)
                {
                    var objectProxy = objectPair.Value;
                    if (objectProxy.IsInUse || objectProxy.Locked || !objectProxy.CustomCanReleaseFlag)
                    {
                        continue;
                    }
                    results.Add(objectProxy.GetTarget());
                }
            }

            /// <summary>
            /// 默认的释放优先顺序，超时的必定释放，如果数量还不够，根据优先级和最近使用原则进行选择
            /// </summary>
            /// <param name="candidateObjects"></param>
            /// <param name="toReleaseCount"></param>
            /// <param name="expireTime"></param>
            /// <returns></returns>
            private List<T> DefaultReleaseObjectFiler(List<T>candidateObjects,int toReleaseCount, DateTime expireTime)
            {
                cachedToReleaseObjects.Clear();

                if(expireTime > DateTime.MinValue)
                {
                    for(int i = candidateObjects.Count - 1; i >= 0; i--)
                    {
                        if(candidateObjects[i].LastUseTime <= expireTime)
                        {
                            cachedToReleaseObjects.Add(candidateObjects[i]);
                            candidateObjects.RemoveAt(i);
                            continue;
                        }
                    }

                    toReleaseCount -= cachedToReleaseObjects.Count;
                }

                for (int i = 0; toReleaseCount > 0 && i < candidateObjects.Count; i++) 
                {
                    for (int j = i +1;j < candidateObjects.Count; j++)
                    {
                        if(candidateObjects[i].Priority > candidateObjects[j].Priority
                            || candidateObjects[i].Priority == candidateObjects[j].Priority 
                            && candidateObjects[i].LastUseTime > candidateObjects[j].LastUseTime)
                        {
                            T temp = candidateObjects[i];
                            candidateObjects[i] = candidateObjects[j];
                            candidateObjects[j] = temp;
                        }
                    }
                    cachedToReleaseObjects.Add(candidateObjects[i]);
                    toReleaseCount--;
                }

                return cachedToReleaseObjects;
            }

        }
    }
}
