using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework.ResourceManager
{
    public class LoadTask : TaskBase
    {

        public object result;

        /// <summary>
        /// 完成回调
        /// </summary>
        public new Action<LoadTask> Completed
        {
            get
            {
                return base.Completed;
            }
            set
            {
                base.Completed = (Action<TaskBase>)value;
            }
        }

        public ILoader Loader { get; set; }
        public virtual void Start<T>(TaskPool<T> taskPool, ILoader loader) where T : TaskBase
        {
            Loader = loader;
            base.Start<T>(taskPool);
        }

        public virtual void SyncGet(object asset)
        {

        }

        public override void Update()
        {
            base.Update();
        }
    }
}
