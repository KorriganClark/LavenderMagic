using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;

namespace Lavender
{
    [Serializable]
    public class LCharacterControl : LSingleton<LCharacterControl>
    {
        // 角色属性组件和战斗组件
        private LAttrComponent attrComponent;
        private LBattleComponent battleComponent;

        public LEntity Entity { get; private set; } // 角色实体

        public LCharacter Character { get { return Entity as LCharacter; } } // 强制转换为角色类型

        public RootStateMachine RootStateMachine { get; private set; } // 状态机

        // 获取属性组件
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

        // 获取战斗组件
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

        // 初始化
        public void Init()
        {
            RootStateMachine = new RootStateMachine(); // 创建状态机
            RootStateMachine.Start(Entity); // 启动状态机
            var camera = Entity.AddComponent<ThirdPersonCameraComponent>(); // 添加第三人称摄像机组件
            InputMgr.Instance.Camera = camera; // 将摄像机组件注册到输入管理器
            InputMgr.Instance.CharacterControl = this; // 将角色控制器注册到输入管理器
        }

        // 设置目标实体
        public void SetTarget(LEntity entity)
        {
            Entity = entity;
            Init(); // 初始化
        }

        // 处理玩家输入
        public void DealPlayerInput(CharacterPCInput input)
        {
            var isMoving = input.LeftAndRightInput != 0 || input.ForwadAndBackInput != 0;
            // 如果角色当前不在跳跃或下落状态下
            if (!(RootStateMachine.CurrentState is StateJump || RootStateMachine.CurrentState is StateFall))
            {
                if (isMoving) // 如果有移动输入
                {
                    AttrComponent.CurrentMoveSpeed = AttrComponent.MoveSpeed; // 更新当前速度属性
                    TowardsUpdate(input.ForwadAndBackInput, input.LeftAndRightInput); // 更新朝向
                }
                else // 否则将当前速度属性设为 0
                {
                    AttrComponent.CurrentMoveSpeed = 0;
                }
            }
            if (input.JumpPressInput) // 如果有跳跃输入
            {
                RootStateMachine.AddRequest(EStateRequest.Jump); // 向状态机发送跳跃请求
            }

            if (input.MouseLeftClick) // 如果有攻击输入
            {
                RootStateMachine.AddRequest(EStateRequest.NormalAttack); // 向状态机发送普通攻击请求
            }
        }

        // 更新角色朝向
        public void TowardsUpdate(float verticalInput, float horizontalInput)
        {
            Vector3 cameraToward = Entity.GetComponent<ThirdPersonCameraComponent>().CameraTrans.forward; // 获取摄像机的朝向
            cameraToward.y = 0; // 将垂直方向设为 0
            var res = new Vector3(verticalInput * cameraToward.x + horizontalInput * cameraToward.z, 0,
                                  verticalInput * cameraToward.z - horizontalInput * cameraToward.x); // 根据输入计算出新的朝向
            Quaternion targetRotation = Quaternion.LookRotation(res.normalized, Vector3.up); // 计算出目标旋转角度
            Entity.Rotation = targetRotation;
        }

        // 更新状态机
        public void Update(float dt)
        {
            if (RootStateMachine != null) // 如果状态机不为空
            {
                RootStateMachine.Update(dt); // 更新状态机
            }
        }
    }
}
