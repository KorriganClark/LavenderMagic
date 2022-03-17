using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender.Framework
{
    public sealed class TaskPool<T> where T :TaskBase
    {
        private readonly List<T> Processing = new List<T>();

        /// <summary>
        /// 任务池中任务数量
        /// </summary>
        public int Count
        {
            get
            {
                return Processing.Count;
            }
        }
        /// <summary>
        /// 将某个任务加入处理队列
        /// </summary>
        /// <param name="task"></param>
        public void Process(T task)
        {
            Processing.Add(task);
        }
        /// <summary>
        /// 任务池轮询，由于单个任务池不属于模块，需要业务调用方单独调用以进行更新
        /// </summary>
        public void UpdateAll()
        {
            for(var index = 0; index < Processing.Count; index++)
            {
                var item = Processing[index];
                if (FrameworkControl.busy)
                {
                    return;
                }
                item.Update();
                if (!item.isDone)
                {
                    continue;
                }

                Processing.RemoveAt(index);
                index--;
                if(item.status == TaskStatus.Failed)
                {
                    //Log($"Unable to complete {item.GetType().Name} with error: {item.error}");
                }
                //本来这里进行完成回调，移到了Task里完成时立刻执行。不知道会不会出问题
            }
        }

        public void ClearAll()
        {
            Processing.Clear();
        }

    }
}
