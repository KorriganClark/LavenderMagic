using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender
{
    public struct DamageRequest
    {
        public LEntity Owner;
        public LEntity Target;
        public Dictionary<EAttrType, float> DamageRate;//技能倍率，根据不同属性决定,单位为百分比
    }
}
