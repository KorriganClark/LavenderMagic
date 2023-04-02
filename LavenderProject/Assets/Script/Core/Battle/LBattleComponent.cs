using System.Collections.Generic;

namespace Lavender
{
    /// <summary>
    /// 技能键，用于区分不同的技能类型
    /// </summary>
    public enum ESkillKey
    {
        NormalAttack,
        ElementalSkill,
        ElementalExplosion
    }

    /// <summary>
    /// 战斗组件，用于管理实体的战斗行为，包括技能、Buff 等
    /// </summary>
    public class LBattleComponent : LComponent
    {

        private Dictionary<ESkillKey, LSkill> skills = new Dictionary<ESkillKey, LSkill>(); // 技能字典

        private List<LBuff> buffs = new List<LBuff>(); // Buff 列表

        private Queue<LSkillInstance> skillQueue = new Queue<LSkillInstance>(); // 技能队列

        /// <summary>
        /// 检测是否有等待释放的技能
        /// </summary>
        public bool IsReleaseSkill()
        {
            return skillQueue.Count > 0;
        }

        public override void OnAttach(LGameObject go)
        {
            base.OnAttach(go);
            if (go is LCharacter)
            {
                var character = go as LCharacter;
                InitSkillConfig(character.Config);
            }
        }

        #region Skill

        /// <summary>
        /// 初始化技能配置
        /// </summary>
        public void InitSkillConfig(LEntityConfig config)
        {
            skills[ESkillKey.NormalAttack] = new LSkill(Entity, config.NormalAttack);
        }

        /// <summary>
        /// 根据技能键获取对应的技能
        /// </summary>
        public LSkill GetSkillByKey(ESkillKey skillKey)
        {
            return skills[skillKey];
        }

        /// <summary>
        /// 使用指定的技能
        /// </summary>
        public void UseSkill(ESkillKey key)
        {
            if (skills.TryGetValue(key, out LSkill skill))
            {
                // 暂时只允许一个技能
                if (skillQueue.Count <= 0)
                {
                    skillQueue.Enqueue(skill.CreateInstance());
                }
            }
        }

        /// <summary>
        /// 使用指定的技能
        /// </summary>
        public LSkillInstance UseSkill(LSkill skill)
        {
            // 暂时只允许一个技能
            if (skillQueue.Count <= 0)
            {
                var instance = skill.CreateInstance();
                skillQueue.Enqueue(instance);
                return instance;
            }
            return null;
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            if (skillQueue.Count <= 0)
            {
                return;
            }
            LSkillInstance skillInstance = skillQueue.Peek();
            skillInstance.Update(delta);
            if (skillInstance.OutOfTime)
            {
                skillQueue.Dequeue();
            }
        }

        #endregion 


        #region Buff

        /// <summary>
        /// 向其他实体请求添加 Buff
        /// </summary>
        public void RequestAddBuffToOther(LBuff buff, LEntity target)
        {
            LBattleMgr.Instance.ProcessAddBuffRequest(Entity, target, buff);
        }

        /// <summary>
        /// 添加一个 Buff
        /// </summary>
        public void AddBuff(LBuff buff)
        {
            buffs.Add(buff);
            buff.OnAttachToEntity(Entity);
        }

        /// <summary>
        /// 移除一个 Buff
        /// </summary>
        public void RemoveBuff(LBuff buff)
        {
            buffs.Remove(buff);
            buff.OnLeaveFromEntity(Entity);
        }

        #endregion
    }
}
