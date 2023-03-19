using Assets.Script.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using static UnityEngine.EventSystems.EventTrigger;

namespace Lavender
{
    public enum ESkillKey
    {
        NormalAttack,
        ElementalSkill,
        ElementalExplosion
    }

    public class LBattleComponent : LComponent
    {

        private BuffProcessor processor { get { return BuffProcessor.Instance; } }

        private Dictionary<ESkillKey, LSkill> skills = new Dictionary<ESkillKey, LSkill>();

        private List<LBuff> buffs = new List<LBuff>();

        private Queue<LSkillInstance> skillQueue = new Queue<LSkillInstance>();

        private Queue<ESkillKey> skillRequest = new Queue<ESkillKey>();
        public bool IsReleaseSkill()
        {
            return skillQueue.Count > 0;
        }

        public void AddRequest(ESkillKey skillKey)
        {
            if(skillRequest.Count == 0) { skillRequest.Enqueue(skillKey); }
        }

        public override void OnAttach(LGameObject go)
        {
            base.OnAttach(go);
            if(go is LCharacter)
            {
                var character = go as LCharacter;
                InitSkillConfig(character.Config);
            }
        }

        #region Skill

        public void InitSkillConfig(LCharacterConfig config)
        {
            skills[ESkillKey.NormalAttack] = new LSkill(Entity, config.NormalAttack);
        }

        public LSkill GetSkillByKey(ESkillKey skillKey)
        {
            return skills[skillKey];
        }

        public void UseSkill(ESkillKey key)
        {
            if(skills.TryGetValue(key, out LSkill skill)) 
            {
                //暂时只允许一个技能
                if(skillQueue.Count <= 0)
                {
                    skillQueue.Enqueue(skill.CreateInstance());
                }
            }
        }

        public override void Update(float delta)
        {
            base.Update(delta);
            if(skillQueue.Count <= 0)
            {
                return;
            }
            LSkillInstance skillInstance = skillQueue.Peek();
            skillInstance.Update(delta);
            if(skillInstance.OutOfTime)
            {
                skillQueue.Dequeue();
            }
        }


        #endregion 



        #region Buff

        public void RequestAddBuffToOther(LBuff buff, LEntity target)
        {
            LBattleMgr.Instance.ProcessAddBuffRequest(Entity, target, buff);
        }

        public void AddBuff(LBuff buff)
        {
            buffs.Add(buff);
            buff.OnAttachToEntity(Entity);
        }

        public void RemoveBuff(LBuff buff)
        {
            buffs.Remove(buff);
            buff.OnLeaveFromEntity(Entity);
        }
        
        public void ApplyBuff(LBuff buff)
        {
            processor.ProcessBuff(buff);
            if (buff.RemoveAfterApply)
            {
                RemoveBuff(buff);
            }
        }
        #endregion
    }
}
