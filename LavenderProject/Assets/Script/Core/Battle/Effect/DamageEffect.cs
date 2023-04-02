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
    public class DamageEffect : LSkillEffect
    {
        public float DamageRate;
        public float Radius;
        public virtual DamageRequest GetDamageRequest(LEntity target)
        {
            var request = new DamageRequest();
            request.Owner = Owner;
            request.Target = target;
            request.DamageRate = new Dictionary<EAttrType, float>
            {
                { EAttrType.Attack, DamageRate }
            };
            request.IsPhysics = true;
            return request;
        }

        public override void GetTargets()
        {
            Targets = LEntityMgr.Instance.GetEntitysByPos(Owner.Position, Radius, Owner).ToList();
        }

        public override void OnStart()
        {
            foreach (var target in Targets)
            {
                
                DamageProcessor.Instance.ProcessDamageRequest(GetDamageRequest(target));
                /*
                var buff = new DamageBuff(Damage);

                BattleCom.RequestAddBuffToOther(buff, target);*/
            }
        }
    }
}
