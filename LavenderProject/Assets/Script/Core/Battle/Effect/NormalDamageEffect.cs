using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "Lavender/NormalDamage")]
    public class NormalDamageEffect : LSkillEffect
    {
        public int Damage;
        public float Radius;

        public override void GetTargets()
        {
            var list = LEntityMgr.Instance.GetEntitysByPos(Owner.Position, Radius);
            list.Remove(Owner);
            Targets = list;
        }

        public override void OnStart()
        {
            foreach (var target in Targets)
            {
                var buff = new NormalDamageBuff(Damage);
                BattleCom.RequestAddBuffToOther(buff, target);
            }
        }
    }
}
