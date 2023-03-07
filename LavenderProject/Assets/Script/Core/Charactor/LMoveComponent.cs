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
        }
        private void TryToMove()
        {
            /*
            if (CanMove == false)
            {
                if (MoveState == EMoveState.Idle)
                {
                    animComp.PlayAnim(characterModel.GetComponent<Animator>(), 0);
                    MoveState = EMoveState.Idle;
                }
                return;
            }

            Vector3 move = camObj.transform.forward * Input.GetAxis("Vertical") + camObj.transform.right * Input.GetAxis("Horizontal");
            move.y = 0;
            Vector3 dir = move.normalized * characterConfig.Speed * Time.deltaTime;
            moveController.Move(dir);

            if (dir.sqrMagnitude != 0)
            {
                characterModel.transform.forward = move.normalized;
            }

            if (dir.sqrMagnitude != 0 && moveState == MoveState.Idle)
            {
                animComp.PlayAnim(characterModel.GetComponent<Animator>(), 1);
                moveState = MoveState.Walk;
                characterModel.transform.forward = move.normalized;
            }
            else if (dir.sqrMagnitude * 100 == 0 && moveState == MoveState.Walk)
            {
                animComp.PlayAnim(characterModel.GetComponent<Animator>(), 0);
                moveState = MoveState.Idle;
            }*/
        }

    }
}