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
                        moveController.center = new Vector3(0, 1.09f, 0);
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
        public float FallingAcceleration
        {
            get
            {
                return 9.8f;
            }
        }
        public float SpeedOnY { get; set; } = 0f;
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

        public void MoveEntity(Vector3 offset)
        {
            var pos = Entity.Root.transform.localPosition;
            pos += offset;
            Entity.Root.transform.localPosition = pos;
        }

        public void UpdatePos(float deltaTime, bool isOnFloor = true)
        {
            if(!isOnFloor)
            {
                SpeedOnY -= deltaTime * FallingAcceleration;
            }
            Vector3 move = new Vector3(0, SpeedOnY * deltaTime, 0);
            if (CanMove)
            {
                Vector3 toward = Entity.Model.transform.forward;
                toward.y = 0f;
                toward.Normalize();
                move += toward * MoveSpeed * deltaTime;
            }
            MoveController.Move(move);
        }


        public void MoveForward(float deltaTime)
        {
            Vector3 toward = Entity.Model.transform.forward;
            toward.y = 0f;
            toward.Normalize();
            Vector3 move = toward * MoveSpeed * deltaTime;
            MoveEntity(move);
        }

        public void Jump()
        {
            SpeedOnY = AttrComponent.JumpAbility;
        }

        public void ResetYSpeed()
        {
            SpeedOnY = 0f;
        }
        
        public void UpdatePosY(float deltaTime)
        {
            MoveController.Move(new Vector3(0, SpeedOnY * deltaTime, 0));
        }

    }
}