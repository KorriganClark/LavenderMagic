using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender
{
    /// <summary>
    /// 组件
    /// </summary>
    public class LComponent
    {
        public LEntity Entity { get; set; }

        public virtual void OnAttach(LEntity entity)
        {
            Entity = entity;
        }

        public virtual void Update(float delta)
        {

        }

    }
}
