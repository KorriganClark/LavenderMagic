using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;
using Assets.Script.Core.Entity;

namespace Lavender
{



    [Serializable]
    public class LCharacterControl : LSingleton<LCharacterControl>
    {
        private LAttrComponent attrComponent;
        public LEntity Entity { get; private set; }

        public LCharacter Character { get { return Entity as LCharacter; } }

        public MoveStateMachine MoveStateMachine { get; private set; }
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
        public void Init()
        {
            MoveStateMachine = new MoveStateMachine(typeof(StateIdle), Entity);
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
            var isMoving = input.LeftAndRightYInput != 0 || input.ForwadAndBackInput != 0;
            if(isMoving)
            {
                AttrComponent.CurrentMoveSpeed = AttrComponent.MoveSpeed;
                TowardsUpdate(input.ForwadAndBackInput, input.LeftAndRightYInput);
            }
            else
            {
                AttrComponent.CurrentMoveSpeed = 0;
            }

        }

        public void TowardsUpdate(float verticalInput, float horizontalInput)
        {
            Vector3 cameraToward = Entity.GetComponent<ThirdPersonCameraComponent>().CameraTrans.forward;
            cameraToward.y = 0;
            //float mo = MathF.Sqrt(verticalInput * verticalInput + horizontalInput * horizontalInput);
            var res = new Vector3(verticalInput * cameraToward.x + horizontalInput * cameraToward.z, 0,
                                  verticalInput * cameraToward.z - horizontalInput * cameraToward.x);
            Entity.Model.transform.forward = res.normalized;
        }

        public void Update(float dt)
        {
            if (MoveStateMachine != null)
            {
                MoveStateMachine.Update(dt);
            }
        }
    }

    
}

