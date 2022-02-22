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
        /// 对象代理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private sealed class Object<T> : IReference where T : ObjectBase
        {
            private T target;
            private int spawnCount;

            public Object()
            {
                target = null;
                spawnCount = 0;
            }

            public string Name
            {
                get
                {
                    return target.Name;
                }
            }

            public bool Locked
            {
                get
                {
                    return target.Locked;
                }
                internal set
                {
                    target.Locked = value;
                }
            }

            /// <summary>
            /// 获取对象的优先级。
            /// </summary>
            public int Priority
            {
                get
                {
                    return target.Priority;
                }
                internal set
                {
                    target.Priority = value;
                }
            }

            /// <summary>
            /// 获取自定义释放检查标记。
            /// </summary>
            public bool CustomCanReleaseFlag
            {
                get
                {
                    return target.CustomCanReleaseFlag;
                }
            }

            /// <summary>
            /// 获取对象上次使用时间。
            /// </summary>
            public DateTime LastUseTime
            {
                get
                {
                    return target.LastUseTime;
                }
            }

            /// <summary>
            /// 获取对象是否正在使用。
            /// </summary>
            public bool IsInUse
            {
                get
                {
                    return spawnCount > 0;
                }
            }

            public int SpawnCount
            {
                get
                {
                    return spawnCount;
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="obj"></param>
            /// <param name="spawned">是否执行初始化回调</param>
            /// <returns></returns>
            public static Object<T> Create(T obj, bool spawned)
            {
                if(obj == null)
                {
                    throw new Exception("obj is null!");
                }

                Object<T> internalObject = ReferencePool.Acquire<Object<T>>();
                internalObject.target = obj;
                internalObject.spawnCount = spawned ? 1 : 0;
                if (spawned)
                {
                    obj.OnSpawn();
                }
                return internalObject;
            }


            public void Clear()
            {
                target = null;
                spawnCount = 0;
            }

            public T GetTarget()
            {
                return target;
            }

            /// <summary>
            /// 获取对象。
            /// </summary>
            /// <returns>对象。</returns>
            public T Spawn()
            {
                spawnCount++;
                target.LastUseTime = DateTime.UtcNow;
                target.OnSpawn();
                return target;
            }


            /// <summary>
            /// 回收对象。
            /// </summary>
            public void Unspawn()
            {
                target.OnUnspawn();
                target.LastUseTime = DateTime.UtcNow;
                spawnCount--;
                if(spawnCount < 0)
                {
                    throw new Exception("spawnCount already 0!");
                }
            }

            /// <summary>
            /// 释放对象。
            /// </summary>
            /// <param name="isShutdown">是否是关闭对象池时触发。</param>
            public void Release(bool isShutDown)
            {
                target.Release(isShutDown);
                ReferencePool.Release(target);
            }
        }
    }
}
