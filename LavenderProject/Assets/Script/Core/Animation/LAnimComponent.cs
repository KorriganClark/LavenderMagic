using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;
using Assets.Script.Core.Entity;

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
            animConfig = (go as LCharacter).Config.AnimConfig;
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
        /// 传入animator，立即播放一个动画，以后做成处理命令的接口。
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="animType"></param>
        public void PlayAnim(AnimType animType)
        {
            AnimationPlayableUtilities.PlayClip(EntityAnimator, animConfig.GetAnim(animType), out playableGraph);
        }
    }
}


