using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;

namespace Lavender
{
    [Serializable]
    public class LAnimComponent : LComponent
    {
        //public Dictionary<string, AnimationClip> animClips;
        public List<AnimationClip> animClips;//为了序列化，使用List。以后应当做好动作与动画的映射
        PlayableGraph playableGraph;

        private Animator EntityAnimator
        {
            get
            {
                if(Entity.Model!= null)
                {
                    var res = Entity.Model.GetComponent<Animator>();
                    if (res == null) 
                    {
                        res = Entity.Model.AddComponent<Animator>();
                    }
                    return res;
                }
                return null;
            }
        }
        private void Start()
        {

        }
        /// <summary>
        /// 传入animator，立即播放一个动画，以后做成处理命令的接口。
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="animType"></param>
        public void PlayAnim(int animType)
        {
            AnimationPlayableUtilities.PlayClip(EntityAnimator, animClips[animType], out playableGraph);
        }
    }
}


