using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Lavender
{
    public enum EMoveState
    {
        None,
        Idle,
        Walk,
        Run,
        Rush,
        Jump,
        Fall,
    }
    //实体移动组件
    public class LMoveComponent : LComponent
    {
        private CharacterController moveController;

        private LAttrComponent attrComponent;

        public CharacterController MoveController
        {
            get
            {
                if(moveController == null)
                {
                    moveController = Entity?.Root.GetComponent<CharacterController>();
                    if(moveController == null)
                    {
                        moveController = Entity.Root.AddComponent<CharacterController>();
                    }
                }
                return moveController;
            }
        }

        public LAttrComponent AttrComponent
        {
            get
            {
                if(attrComponent== null)
                {
                    attrComponent = Entity.GetComponent<LAttrComponent>();
                    if(attrComponent == null)
                    {
                        throw new Exception("No Attr");
                    }
                }
                return attrComponent;
            }
        }

        public EMoveState MoveState = EMoveState.None;
        public float MoveSpeed
        {
            get
            {
                return (float)(AttrComponent?.CurrentMoveSpeed);
            }
        }
        public float JumpSpeed
        {
            get
            {
                return (float)(AttrComponent?.JumpAbility);
            }
        }
        public float FallSpeed 
        {
            get
            {
                return 10f;
            }
        }
        public bool CanMove
        {
            get
            {
                var val = AttrComponent.GetAttr(EAttrType.CanMove);
                return val != 0;
            }
        }

        public void InitConfig(LCharacterConfig config)
        {

        }

        public void MoveForward()
        {
            if(!CanMove)
            {
                return;
            }

            Vector3 move = (Vector3)(Entity?.Model?.transform.forward * MoveSpeed * Time.deltaTime);
            move.y = 0;
            MoveController.Move(move);
            //Debug.Log($"Move时间:{Time.realtimeSinceStartup}");

        }
        
        public void TryFall()
        {

        }

    }
}