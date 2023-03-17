using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Lavender
{
    public class BuffProcessor : LSingleton<BuffProcessor>
    {

        public void ProcessBuff(LBuff buff)
        {
            if (buff.BuffTypes.Contains(BuffType.Damage))
            {
                ProcessDamageBuff(buff);
            }
        }

        public void ProcessDamageBuff(LBuff buff) 
        {
            var attrCom = buff.Entity.GetComponent<LAttrComponent>();
            var currentHp = attrCom.GetAttr(EAttrType.HP);
            var res = Math.Max(currentHp - buff.DamageVal(), 0);
            attrCom.SetAttr(EAttrType.HP, res);
        }
    }
}
