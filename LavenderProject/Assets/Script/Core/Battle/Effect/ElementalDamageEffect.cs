using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace Lavender
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "Lavender/ElementalDamage")]
    public class ElementalDamageEffect : DamageEffect
    {
        public EElementType ElementType;
        public override DamageRequest GetDamageRequest(LEntity target)
        {
            var request = new DamageRequest();
            request.Owner = Owner;
            request.Target = target;
            request.DamageRate = new Dictionary<EAttrType, float>
            {
                { EAttrType.Attack, DamageRate }
            };
            request.ElementType = ElementType;
            request.IsPhysics = false;
            return request;
        }
    }
}
