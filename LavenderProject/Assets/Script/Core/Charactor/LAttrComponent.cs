using Assets.Script.Core.Entity;
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
        None = 0,
        HP = 1,
        MP = 2,
        MoveSpeed = 3,
        JumpAbility = 4,
        Attack = 5,
        CanMove = 6,
        CurrentMoveSpeed = 7,
    }

    //实体移动组件
    public partial class LAttrComponent : LComponent
    {
        public Dictionary<EAttrType,int> AttrDic = new Dictionary<EAttrType,int>();
        public override void OnAttach(LEntity entity)
        {
            base.OnAttach(entity);
            if(entity is LCharacter)
            {
                var character = entity as LCharacter;
                InitAttrByConfig(character.Config);
            }
        }

        public int GetAttr(EAttrType type, bool allowDefault = false)
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

        public void InitAttrByConfig(LCharacterConfig config)
        {
            if(config == null)
            {
                throw new Exception("config is null");
            }
            SetAttrFloat(EAttrType.JumpAbility, config.JumpHigh);
            SetAttrFloat(EAttrType.MoveSpeed, config.Speed);
            SetAttrFloat(EAttrType.CurrentMoveSpeed, 0);

            SetAttr(EAttrType.HP, config.HP);
            SetAttr(EAttrType.MP, config.MP);
            SetAttr(EAttrType.Attack, config.Attack);
            SetAttr(EAttrType.CanMove, 1);
        }
    }
}