using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;

namespace Lavender
{
    [Serializable]
    public class LAnimComponent : LComponent
    {
        public LAnimConfig animConfig;
        PlayableGraph playableGraph;

        public override void OnAttach(LGameObject go)
        {
            base.OnAttach(go);
            if(go is LCharacter)
            {
                animConfig = (go as LCharacter).Config.AnimConfig;
            }
            else
            {
                animConfig = (go as LMonster).Config.AnimConfig;
            }
        }

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
        /// 播放组件原有的Anim。
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="animType"></param>
        public void PlayAnim(AnimType animType)
        {
            AnimationPlayableUtilities.PlayClip(EntityAnimator, animConfig.GetAnim(animType), out playableGraph);
        }

        /// <summary>
        /// 播放传入的Anim。
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="animType"></param>
        public void PlayAnim(AnimationClip clip)
        {
            AnimationPlayableUtilities.PlayClip(EntityAnimator, clip, out playableGraph);
        }

    }
}


