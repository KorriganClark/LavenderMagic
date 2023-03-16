using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Lavender
{

    public enum AnimType
    {
        Idle,
        Walk,
        Run,
        Jump,
        Fall,
        Attack
    }
    [Serializable]
    public class AnimTypeToClip
    {
        public AnimType type;
        public AnimationClip animationClip;
    }

    [Serializable]
    [CreateAssetMenu(menuName = "Lavender/AnimConfig")]
    public class LAnimConfig : ScriptableObject
    {
        [SerializeField]
        private List<AnimTypeToClip> AnimationClips = new List<AnimTypeToClip>();

        private Dictionary<AnimType, AnimationClip> animationMap;

        public AnimationClip GetAnim(AnimType type)
        {
            if(animationMap == null)
            {
                animationMap = new Dictionary<AnimType, AnimationClip>();
                for(int i = 0;i< AnimationClips.Count;i++)
                {
                    animationMap[AnimationClips[i].type] = AnimationClips[i].animationClip;
                }
            }
            AnimationClip animationClip;
            if(!animationMap.TryGetValue(type, out animationClip))
            {
                throw new Exception("没有此动画，请检查配置");
            }
            return animationClip;
        }

    }
}
