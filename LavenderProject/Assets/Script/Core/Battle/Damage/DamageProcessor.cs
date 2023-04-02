using UnityEngine;
using UnityEditor;
using System;

namespace Lavender
{
    public class DamageProcessor : LSingleton<DamageProcessor>
    {
        public void ProcessDamageRequest(DamageRequest request)
        {
            var attrCom = request.Target.AttrComponent;
            var currentHp = attrCom.GetAttr(EAttrType.HP);
            var res = Math.Max(currentHp - CalculateDamage(request), 0);
            attrCom.SetAttr(EAttrType.HP, res);
        }

        public int CalculateDamage(DamageRequest request)
        {
            var ownerAttr = request.Owner.AttrComponent;
            var damageRate = request.DamageRate;
            float damage = 0;
            foreach(var pair in damageRate)
            {
                var attrAmount = ownerAttr.GetAttr(pair.Key);
                damage += attrAmount * pair.Value / 100;
            }
            return (int)damage;
        }
    }
}
