using System;
using UnityEngine;

namespace Lavender
{
    // 移动状态枚举
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

    // 实体移动组件
    public class LMoveComponent : LComponent
    {
        private CharacterController moveController; // 移动控制器
        private LAttrComponent attrComponent; // 属性组件

        // 移动控制器
        public CharacterController MoveController
        {
            get
            {
                if (moveController == null)
                {
                    moveController = Entity?.Root.GetComponent<CharacterController>();
                    if (moveController == null)
                    {
                        moveController = Entity.Root.AddComponent<CharacterController>();
                        moveController.center = new Vector3(0, 1.09f, 0);
                    }
                }
                return moveController;
            }
        }

        // 属性组件
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

        // 移动状态、移动速度、跳跃速度等属性
        public EMoveState MoveState = EMoveState.None;
        public float MoveSpeed => (float)(AttrComponent?.CurrentMoveSpeed ?? 0);
        public float JumpSpeed => (float)(AttrComponent?.JumpAbility ?? 0);
        public float FallingAcceleration => 9.8f;
        public float SpeedOnY { get; set; } = 0f;

        // 是否可以移动
        public bool CanMove => AttrComponent.GetAttr(EAttrType.CanMove) != 0;

        // 初始化配置，未实现
        public void InitConfig(LCharacterConfig config)
        {

        }

        // 移动实体
        public void MoveEntity(Vector3 offset)
        {
            var pos = Entity.Root.transform.localPosition;
            pos += offset;
            Entity.Root.transform.localPosition = pos;
        }

        // 更新位置信息
        public void UpdatePos(float deltaTime, bool isOnFloor = true)
        {
            if (!isOnFloor)
            {
                SpeedOnY -= deltaTime * FallingAcceleration; // 根据重力加速度更新竖直速度
            }
            Vector3 move = new Vector3(0, SpeedOnY * deltaTime, 0);
            if (CanMove)
            {
                Vector3 toward = Entity.Forward;
                toward.y = 0f;
                toward.Normalize();
                move += toward * MoveSpeed * deltaTime; // 根据当前速度和方向更新移动距离
            }
            MoveController.Move(move); // 移动实体
        }

        // 向前移动
        public void MoveForward(float deltaTime)
        {
            Vector3 toward = Entity.Forward;
            toward.y = 0f;
            toward.Normalize();
            Vector3 move = toward * MoveSpeed * deltaTime; // 计算移动距离
            MoveEntity(move); // 移动实体
        }

        // 跳跃
        public void Jump()
        {
            SpeedOnY = JumpSpeed; // 竖直速度更新为跳跃速度
        }

        // 重置竖直速度
        public void ResetYSpeed()
        {
            SpeedOnY = 0f;
        }

        // 更新竖直位置信息
        public void UpdatePosY(float deltaTime)
        {
            MoveController.Move(new Vector3(0, SpeedOnY * deltaTime, 0)); // 移动实体
        }
    }
}
