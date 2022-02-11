using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework.ObjectPool
{
    /// <summary>
    /// 对象池基类
    /// </summary>
    public abstract class ObjectBase : IReference
    {
        private string name;
        private object target;
        private bool locked;
        private int priority;
        private DateTime lastUseTime;

        public ObjectBase()
        {
            name = null;
            target = null;
            locked = false;
            priority = 0;
            lastUseTime = default(DateTime);
        }

        /// <summary>
        /// 获取对象名称
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        public object Target
        {
            get
            {
                return target;
            }
        }

        public bool Locked
        {
            get
            {
                return locked;
            }
            set
            {
                locked = true;
            }
        }

        public int Priority
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
        /// 获取自定义释放检查标记。
        /// </summary>
        public virtual bool CustomCanReleaseFlag
        {
            get
            {
                return true;
            }
        }

        public DateTime LastUseTime
        {
            get
            {
                return lastUseTime;
            }
            internal set
            {
                LastUseTime = value;
            }
        }

        protected void Initialize(object target)
        {
            Initialize(null, target, false, 0);
        }

        protected void Initialize(string name, object target)
        {
            Initialize(name, target, false, 0);
        }

        protected void Initialize(string name, object target, bool locked)
        {
            Initialize(name, target, locked);
        }

        protected void Initialize(string name, object target, int priority)
        {
            Initialize(name, target, priority);
        }

        protected void Initialize(string name, object target, bool locked, int priority)
        {
            if(target == null)
            {
                throw new Exception("Target is null!!");
            }

            this.name = name ?? string.Empty;
            this.target = target;
            this.locked = locked;
            this.priority = priority;
            this.lastUseTime = DateTime.UtcNow;
        }

        public virtual void Clear()
        {
            name = null;
            target = null;
            locked = false;
            priority = 0;
            lastUseTime = default(DateTime);
        }

        protected internal virtual void OnSpawn()
        {

        }

        protected internal virtual void OnUnspawn()
        {

        }

        protected internal abstract void Release(bool isShutdown);

    }
}
