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
        private AnimationClip animationClip;

        public int SkillID { get { return skillID; } }
        public string SkillName { get { return skillName; } }
        public string SkillDescription { get { return skillDescription; } }
        public int Damage { get { return damage; } }
        public AnimationClip AnimationClip { get { return animationClip; } }
    }
}
