using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender
{
    [Serializable]
    [CreateAssetMenu(menuName = "Lavender/LSkill")]
    public class LSkillConfig : ScriptableObject
    {
        [SerializeField]
        private int skillID;
        [SerializeField]
        private string skillName;
        [SerializeField]
        private string skillDescription;
        [SerializeField]
        private int damage;
        [SerializeField]
        private float totalTime;
        [SerializeField]
        private AnimationClip animationClip;
        [SerializeField]
        private List<LSkillEffect> skillEffects = new List<LSkillEffect>();
        [SerializeField]
        private List<float> interruptPoint = new List<float>();
        public int SkillID { get { return skillID; } }
        public string SkillName { get { return skillName; } }
        public string SkillDescription { get { return skillDescription; } }
        public int Damage { get { return damage; } }
        public float TotalTime { get { return totalTime; } }
        public AnimationClip AnimationClip { get { return animationClip; } }
        public List<LSkillEffect> SkillEffects { get { return skillEffects; } }
        public List<float> InterruptPoint { get { return interruptPoint; } }
    }
}
