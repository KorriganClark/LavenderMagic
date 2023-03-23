using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender
{
    [Serializable]
    [CreateAssetMenu(menuName = "Lavender/LCharacter")]
    public class LCharacterConfig : ScriptableObject
    {
        [SerializeField]
        private float speed = 5, jumpAbility = 1;
        [SerializeField]
        private int attack = 1, hp = 100, mp = 100;
        [SerializeField]
        private GameObject model;
        [SerializeField]
        private LAnimConfig animConfig;
        [SerializeField]
        private LSkillConfig normalAttack;

        public GameObject Model { get { return model; } }
        public LAnimConfig AnimConfig { get { return animConfig; } }
        public LSkillConfig NormalAttack { get { return normalAttack; } }
        public float Speed
        {
            get
            {
                return speed;
            }
        }
        public float JumpHigh
        {
            get
            {
                return jumpAbility;
            }
        }
        public int Attack
        {
            get
            {
                return attack;
            }
        }
        public int HP
        {
            get
            {
                return hp;
            }
        }
        public int MP
        {
            get
            {
                return mp;
            }
        }
    }
}
