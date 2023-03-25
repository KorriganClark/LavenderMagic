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
        public LGameObject GO { get; set; }
        public LEntity Entity
        {
            get
            {
                return GO as LEntity;
            }
            set
            {
                GO = value;
            }
        }

        public virtual void OnAttach(LGameObject go)
        {
            GO = go;
        }

        public virtual void Update(float delta)
        {

        }

        public virtual void OnDetach()
        {
            GO = null;
        }

    }
}
