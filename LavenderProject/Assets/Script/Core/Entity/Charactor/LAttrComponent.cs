
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using YamlDotNet.Core.Tokens;

namespace Lavender
{
    public enum EAttrType
    {
        None,
        HP,
        MP,
        MaxHP,
        MoveSpeed,
        JumpAbility,
        Attack,
        CanMove,
        CurrentMoveSpeed,
        PyroAmount,   // 火
        HydroAmount,  // 水
        AnemoAmount,  // 风
        ElectroAmount, // 雷
        DendroAmount, // 草
        CryoAmount,   // 冰
        GeoAmount,    // 岩
    }

    //实体移动组件
    public partial class LAttrComponent : LComponent
    {
        public Dictionary<EAttrType,int> AttrDic = new Dictionary<EAttrType,int>();
        public override void OnAttach(LGameObject go)
        {
            base.OnAttach(go);
            if(go is LEntity)
            {
                var entity = go as LEntity;
                InitAttrByConfig(entity.Config);
            }
        }

        public int GetAttr(EAttrType type, bool allowDefault = true)
        {
            int res;
            if(!AttrDic.TryGetValue(type, out res))
            {
                if (allowDefault)
                {
                    return 0;
                }
                throw new Exception($"没有该属性:{type}！");
            }
            return res;
        }

        public float GetAttrFloat(EAttrType type)
        {
            return GetAttr(type) * 1.0f / FloatTransRate;
        }

        public void SetAttr(EAttrType type, int val)
        {
            AttrDic[type] = val;
        }

        public void SetAttrFloat(EAttrType type, float val)
        {
            SetAttr(type, (int)MathF.Floor(val * FloatTransRate));
        }

        public void InitAttrByConfig(LEntityConfig config)
        {
            if(config == null)
            {
                throw new Exception("config is null");
            }
            SetAttrFloat(EAttrType.JumpAbility, config.JumpHigh);
            SetAttrFloat(EAttrType.MoveSpeed, config.Speed);
            SetAttrFloat(EAttrType.CurrentMoveSpeed, 0);

            SetAttr(EAttrType.HP, config.HP);
            SetAttr(EAttrType.MaxHP, config.HP);
            SetAttr(EAttrType.MP, config.MP);
            SetAttr(EAttrType.Attack, config.Attack);
            SetAttr(EAttrType.CanMove, 1);
        }
    }
}