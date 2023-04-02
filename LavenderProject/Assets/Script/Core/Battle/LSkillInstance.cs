using System;
using System.Collections.Generic;
using System.Linq;

namespace Lavender
{
    /// <summary>
    /// 技能实例，通过技能类生成的一个技能实例
    /// </summary>
    public class LSkillInstance
    {
        // 技能配置属性，通过技能对象获取
        public LSkillConfig Config => Skill.Config;
        // 技能对象
        public LSkill Skill { get; set; }

        // 当前已过时间
        public float CurrentTime { get; private set; }
        // 技能总时长
        public float TotalTime => Config.TotalTime;
        // 技能是否超时
        public bool OutOfTime => CurrentTime > TotalTime;
        // 等待处理的技能效果队列
        public Queue<LSkillEffect> WaitingEffects { get; private set; }
        // 正在处理的技能效果列表
        public List<LSkillEffect> WorkingEffects { get; private set; }

        // 初始化方法
        public void Init()
        {
            if (Config == null)
            {
                return;
            }
            WaitingEffects = new Queue<LSkillEffect>(Config.SkillEffects);
            WorkingEffects = new List<LSkillEffect>();
        }

        // 更新方法，传入deltaTime表示每帧经过的时间
        public void Update(float deltaTime)
        {
            CurrentTime += deltaTime;
            DealEffect();
        }

        // 处理技能效果的私有方法
        private void DealEffect()
        {
            LSkillEffect effect;
            // 将满足条件的技能效果从等待队列移动到正在处理列表
            while (WaitingEffects.Count > 0 && WaitingEffects.Peek().StartTime < CurrentTime)
            {
                effect = WaitingEffects.Dequeue();
                effect.Skill = Skill;
                effect.GetTargets();
                effect.OnStart();
                WorkingEffects.Add(effect);
            }

            // 从正在处理列表中移除已结束的技能效果
            for (int i = WorkingEffects.Count - 1; i >= 0; i--)
            {
                if (WorkingEffects[i].EndTime > CurrentTime)
                {
                    WorkingEffects[i].OnEnd();
                    WorkingEffects.RemoveAt(i);
                }
            }

            // 遍历正在处理列表，执行需要每帧执行的技能效果
            var tickingEffects = WorkingEffects.Where(e => e.ShouldTick).ToList();
            foreach (var e in tickingEffects)
            {
                e.OnTick();
            }
        }

        // 开始播放动画
        public void StartAnim(LAnimComponent animCom)
        {
            animCom.PlayAnim(Config.AnimationClip);
        }
    }
}
