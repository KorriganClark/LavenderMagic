using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender
{
    public class NormalDamageBuff : LBuff
    {
        public override List<BuffType> BuffTypes { get { return new List<BuffType>{BuffType.Damage}; } }
        public override bool ApplyOnAttach { get { return true; } }
        public override bool RemoveAfterApply { get { return true; } }

        public int Damage { get; set; }

        public NormalDamageBuff(int damage) 
        {
            Damage = damage;
        }

        public override int DamageVal()
        {
            base.DamageVal();
            return Damage;
        }

    }
}
