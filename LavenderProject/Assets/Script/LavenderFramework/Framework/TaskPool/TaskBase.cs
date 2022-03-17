using Lavender.Framework.ResourceManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender.Framework
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskStatus
    {
        Waiting,//等待执行
        Running,//执行中
        Succeed,//已完成
        Failed,//异常终止，失败
    }

    public class TaskBase : IEnumerator, IReference
    {
        /// <summary>
        /// 当前状态
        /// </summary>
        public TaskStatus status { get; protected set; } = TaskStatus.Waiting;

        /// <summary>
        /// 完成回调
        /// </summary>
        public Action<TaskBase> Completed;

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool isDone => status == TaskStatus.Failed || status == TaskStatus.Succeed;

        public object Current => null;

        /// <summary>
        /// 错误码
        /// </summary>
        public string error { get; protected set; }

        public float progress { get; protected set; }

        public bool MoveNext()
        {
            return !isDone;
        }

        public void Reset()
        {

        }

        public virtual void Start<T>(TaskPool<T> taskPool) where T : TaskBase
        {
            status = TaskStatus.Running;
            progress = 0;
            taskPool.Process((T)this);
            Run();
        }

        /// <summary>
        /// 实际的任务处理逻辑入口
        /// </summary>
        protected virtual void Run()
        {

        }

        public virtual void Update()
        {

        }

        public void Cancel()
        {
            Finish("Canceled");
        }

        /// <summary>
        /// 结束任务，根据参数决定完成状态，进行完成回调
        /// </summary>
        /// <param name="errorCode"></param>
        protected void Finish(string errorCode = null)
        {
            error = errorCode;
            status = string.IsNullOrEmpty(error) ? TaskStatus.Succeed : TaskStatus.Failed;
            progress = 1;
            Complete();
        }

        /// <summary>
        /// 执行完成回调，失败也会执行
        /// </summary>
        private void Complete()
        {
            if (Completed == null)
            {
                return;
            }
            var saved = Completed;
            Completed.Invoke(this);
            Completed -= saved;
        }

        /// <summary>
        /// 引用池回收
        /// </summary>
        public void Clear()
        {
            status = TaskStatus.Waiting;
            Completed = null;
            error = null;
            progress = 0;
        }
    }
}
