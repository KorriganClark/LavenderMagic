using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender
{
    /// <summary>
    /// 技能效果，技能的逻辑层，实现实际的技能逻辑，即对敌人造成伤害等
    /// 技能效果可在技能实例运行中产生，一个技能可产生多个技能效果，可指定产生的时间
    /// </summary>
    [Serializable]
    public class LSkillEffect : ScriptableObject
    {
        public float StartTime, EndTime;
        public bool ShouldTick;
        public float Duration { get { return EndTime - StartTime; } }

        public List<LEntity> Targets = new List<LEntity>();

        public LSkill Skill { get; set; }

        public LEntity Owner { get { return Skill?.Owner; } }

        public LBattleComponent BattleCom { get { return Owner?.GetComponent<LBattleComponent>(); } }

        public virtual void GetTargets() { }

        public virtual void OnStart()
        {

        }

        public virtual void OnTick() 
        {

        }

        public virtual void OnEnd()
        {

        }
    }
}
