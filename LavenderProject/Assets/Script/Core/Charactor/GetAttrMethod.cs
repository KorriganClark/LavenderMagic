using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender
{
    /// <summary>
    /// AttrComponent的获取属性方法。
    /// 由于部分属性为浮点数，所以通过该方式进行转化
    /// </summary>
    public partial class LAttrComponent: LComponent
    {
        public readonly int FloatTransRate = 10000;  

        public float JumpAbility
        {
            get 
            {
                return GetAttr(EAttrType.JumpAbility) / FloatTransRate;
            }
            set 
            { 
                SetAttr(EAttrType.JumpAbility, (int)MathF.Floor(value * FloatTransRate));
            }
        }

        public float MoveSpeed
        {
            get
            {
                return GetAttr(EAttrType.MoveSpeed) * 1.0f / FloatTransRate;
            }
            set
            {
                SetAttr(EAttrType.MoveSpeed, (int)MathF.Floor(value * FloatTransRate));
            }
        }
        public float CurrentMoveSpeed
        {
            get
            {
                return GetAttr(EAttrType.CurrentMoveSpeed, true) * 1.0f / FloatTransRate;
            }
            set
            {
                SetAttr(EAttrType.CurrentMoveSpeed, (int)MathF.Floor(value * FloatTransRate));
            }
        }

    }
}
