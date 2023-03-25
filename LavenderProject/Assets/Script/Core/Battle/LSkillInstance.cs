using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lavender
{
    /// <summary>
    /// 技能实例，通过技能类生成的一个技能实例
    /// </summary>
    public class LSkillInstance
    {
        public LSkillConfig Config { get { return Skill.Config; } }
        public LSkill Skill { get; set; }

        public float currentTime = 0;
        public float TotalTime { get { return Config.TotalTime; } }
        public bool OutOfTime { get { return currentTime > TotalTime; } }
        public Queue<LSkillEffect> WaitingEffects { get; set; }
        public List<LSkillEffect> WorkingEffects { get; set; }

        public void Init()
        {
            if(Config == null)
            {
                return;
            }
            WaitingEffects = new Queue<LSkillEffect>(Config.SkillEffects);
            WorkingEffects = new List<LSkillEffect>();
        }
        public void Update(float deltaTime)
        {
            currentTime = currentTime + deltaTime;
            DealEffect();
        }

        public void DealEffect()
        {
            LSkillEffect effect;
            while(WaitingEffects.Count > 0 && WaitingEffects.Peek().StartTime < currentTime)
            {
                effect = WaitingEffects.Peek();
                effect.Skill = Skill;
                effect.GetTargets();
                effect.OnStart();
                WaitingEffects.Dequeue();
                WorkingEffects.Add(effect);
            }

            for(int i = WorkingEffects.Count - 1; i >= 0; i--)
            {
                if (WorkingEffects[i].EndTime > currentTime) 
                {
                    WorkingEffects[i].OnEnd();
                    WorkingEffects.RemoveAt(i);
                }
            }

            var ticking =
                (from workingEffect in WorkingEffects
                    where workingEffect.ShouldTick
                    select workingEffect).ToList();
            foreach(var e in ticking)
            {
                e.OnTick();
            }
        }

        public void StartAnim(LAnimComponent animCom) 
        {
            animCom.PlayAnim(Config.AnimationClip);
        }
    }
}
