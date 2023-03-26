using System;
using UnityEngine;

namespace Lavender
{
    // LEntityConfig 实体配置类，用于存储实体的基本属性和特征，继承自 ScriptableObject
    [Serializable]
    public class LEntityConfig : ScriptableObject
    {
        // 实体名称
        [SerializeField] private string entityName;
        // 移动速度、跳跃高度
        [SerializeField] private float speed = 5f, jumpAbility = 1f;
        // 攻击力、生命值、法力值
        [SerializeField] private int attack = 1, hp = 100, mp = 100;
        // 实体模型、动画配置、普通攻击技能配置
        [SerializeField] private GameObject model;
        [SerializeField] private LAnimConfig animConfig;
        [SerializeField] private LSkillConfig normalAttack;

        // 属性访问器，用于获取实体配置的各项属性
        public string EntityName => entityName;
        public GameObject Model => model;
        public LAnimConfig AnimConfig => animConfig;
        public LSkillConfig NormalAttack => normalAttack;
        public float Speed => speed;
        public float JumpHigh => jumpAbility;
        public int Attack => attack;
        public int HP => hp;
        public int MP => mp;

        // 判断是否存在实体模型
        public bool HasModel() => model != null;

        // 判断是否存在动画配置
        public bool HasAnimConfig() => animConfig != null;
    }
}