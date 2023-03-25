using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lavender
{
    /// <summary>
    /// 技能类，为技能配置在游戏中的运行时包装，是对SkillConfig的解析
    /// </summary>
    public class LSkill
    {
        public LSkill(LEntity entity, LSkillConfig config) 
        {
            Owner = entity;
            Config = config;
        }
        public LEntity Owner { get; set; }

        public LSkillConfig Config { get; set; }
        
        public LSkillInstance CreateInstance()
        {
            var instance = new LSkillInstance();
            instance.Skill = this;
            instance.Init();
            return instance;
        }
    }
}
