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
            private readonly Dictionary<object, Object<T>> objectMap;
            private readonly List<T> cachedCanReleaseObjects;
            private readonly List<T> cachedToReleaseObjects;
            private readonly bool allowMultiSpawn;
            private float autoReleaseInterval;
            private int capacity;
            private float expireTime;
            private int priority;
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
