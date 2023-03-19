using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;
using Assets.Script.Core.Entity;
using System.ComponentModel;
using System.Reflection;
using YamlDotNet.Core.Tokens;
using Unity.VisualScripting;
using Newtonsoft.Json;

namespace Lavender
{
    [Serializable]
    public class LCharacterControl : LSingleton<LCharacterControl>
    {
        private LAttrComponent attrComponent;
        private LBattleComponent battleComponent;
        public LEntity Entity { get; private set; }

        public LCharacter Character { get { return Entity as LCharacter; } }

        public RootStateMachine RootStateMachine { get; private set; }
        public LAttrComponent AttrComponent
        {
            get
            {
                if (attrComponent == null)
                {
                    attrComponent = Entity.GetComponent<LAttrComponent>();
                    if (attrComponent == null)
                    {
                        throw new Exception("No Attr");
                    }
                }
                return attrComponent;
            }
        }
        public LBattleComponent BattleComponent
        {
            get
            {
                if (battleComponent == null)
                {
                    battleComponent = Entity.GetComponent<LBattleComponent>();
                    if (battleComponent == null)
                    {
                        throw new Exception("No Battle");
                    }
                }
                return battleComponent;
            }
        }
        public void Init()
        {
            RootStateMachine = new RootStateMachine();
            RootStateMachine.Start(Entity);
            var camera = Entity.AddComponent<ThirdPersonCameraComponent>();
            InputMgr.Instance.Camera = camera;
            InputMgr.Instance.CharacterControl = this;
        }

        public void SetTarget(LEntity entity)
        {
            Entity = entity;
            Init();
        }


        public void DealPlayerInput(CharacterPCInput input)
        {
            var isMoving = input.LeftAndRightInput != 0 || input.ForwadAndBackInput != 0;
            if(!(RootStateMachine.CurrentState is StateJump || RootStateMachine.CurrentState is StateFall))
            {
                if (isMoving)
                {
                    AttrComponent.CurrentMoveSpeed = AttrComponent.MoveSpeed;
                    TowardsUpdate(input.ForwadAndBackInput, input.LeftAndRightInput);
                }
                else
                {
                    AttrComponent.CurrentMoveSpeed = 0;
                }
            }
            if (input.JumpPressInput)
            {
                RootStateMachine.AddRequest(EStateRequest.Jump);
            }

            if(input.MouseLeftClick)
            {
                //BattleComponent.AddRequest(ESkillKey.NormalAttack);
                RootStateMachine.AddRequest(EStateRequest.NormalAttack);
            }
        }

        public void TowardsUpdate(float verticalInput, float horizontalInput)
        {
            Vector3 cameraToward = Entity.GetComponent<ThirdPersonCameraComponent>().CameraTrans.forward;
            cameraToward.y = 0;
            var res = new Vector3(verticalInput * cameraToward.x + horizontalInput * cameraToward.z, 0,
                                  verticalInput * cameraToward.z - horizontalInput * cameraToward.x);
            Quaternion targetRotation = Quaternion.LookRotation(res.normalized, Vector3.up);
            Entity.Model.transform.rotation = Quaternion.Lerp(Entity.Model.transform.rotation, targetRotation,
         Time.deltaTime * 10);
        }

        public void Update(float dt)
        {
            if (RootStateMachine != null)
            {
                RootStateMachine.Update(dt);
            }
        }
    }

    
}

