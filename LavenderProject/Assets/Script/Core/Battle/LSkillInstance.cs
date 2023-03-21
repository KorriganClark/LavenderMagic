using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        public void Update(float deltaTime)
        {
            currentTime = currentTime + deltaTime;
        }
    }
}
