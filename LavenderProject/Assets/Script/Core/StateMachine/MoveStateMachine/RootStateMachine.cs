using System;
using System.Diagnostics;

namespace Lavender
{
    // RootStateMachine 类表示一个实体的根状态机，它扩展了基础状态机，用于处理实体的状态转换和更新。
    public class RootStateMachine : BaseStateMachine<string, string>
    {
        // 是否处于调试状态
        protected override bool IsDebug => false;

        // 当前状态机所属的实体
        public LEntity Entity { get; private set; }

        // 获取实体的移动组件
        public LMoveComponent MoveComponent => Entity?.GetComponent<LMoveComponent>();

        // 获取实体的动画组件
        public LAnimComponent AnimComponent => Entity?.GetComponent<LAnimComponent>();

        // 启动状态机
        public void Start(LEntity entity)
        {
            Entity = entity;
            HandleSwitch<StateIdle>();
            isWorking = true;
        }

        // 更新状态机
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }

    // MoveState 类表示一个移动状态基类，用于处理实体的移动状态转换和更新。
    public class MoveState : BaseState<string>
    {
        // 获取当前状态机所属的实体
        public LEntity Entity => (StateMachine as RootStateMachine)?.Entity;

        // 获取实体的动画组件
        public LAnimComponent AnimComponent => Entity?.GetComponent<LAnimComponent>();

        // 获取实体的移动组件
        public LMoveComponent MoveComponent => Entity?.GetComponent<LMoveComponent>();

        // 获取实体的属性组件
        public LAttrComponent AttrComponent => Entity?.GetComponent<LAttrComponent>();

        // 获取实体的战斗组件
        public LBattleComponent BattleComponent => Entity?.GetComponent<LBattleComponent>();

        // 添加跳跃和下落转换
        public void AddJumpAndFallTransition()
        {
            AddTransition<StateJump>(() => CurrentRequest == EStateRequest.Jump);
            AddTransition<StateFall>(() => !MoveComponent.MoveController.isGrounded);
        }

        // 添加战斗转换
        public void AddBattleTransition()
        {
            AddTransition<BattleStateMachine>(() => CurrentRequest == EStateRequest.NormalAttack);
        }

        // 添加死亡转换
        public void AddDeadTransition()
        {
            AddTransition<StateDead>(() => AttrComponent.GetAttr(EAttrType.HP) <= 0);
        }
    }

    // StateIdle 类表示一个静止状态，用于处理实体在静止状态下的状态转换和更新。
    public class StateIdle : MoveState, IState
    {
        public override string ID => "Idle";

        public override void InitTransition()
        {
            base.InitTransition();
            AddTransition<StateWalk>(() => AttrComponent?.CurrentMoveSpeed > 0);
            AddJumpAndFallTransition();
            AddBattleTransition();
            AddDeadTransition();
        }

        public override void Enter()
        {
            base.Enter();
            AnimComponent.PlayAnim(AnimType.Idle);
        }
    }

    // StateWalk 类表示一个行走状态，用于处理实体在行走状态下的状态转换和更新。
    public class StateWalk : MoveState, IState
    {
        public override string ID => "Walk";

        public override void InitTransition()
        {
            base.InitTransition();
            AddTransition<StateIdle>(() => AttrComponent?.CurrentMoveSpeed <= 0);
            AddJumpAndFallTransition();
            AddBattleTransition();
            AddDeadTransition();
        }

        public override void Update(float deltaTime)
        {
            MoveComponent?.MoveForward(deltaTime);
        }

        public override void Enter()
        {
            base.Enter();
            AnimComponent.PlayAnim(AnimType.Walk);
        }
    }

    // StateJump 类表示一个跳跃状态，用于处理实体在跳跃状态下的状态转换和更新。
    public class StateJump : MoveState, IState
    {
        public override string ID => "Jump";

        // 初始化状态转换
        public override void InitTransition()
        {
            base.InitTransition();
            // 当实体接触地面时转换为闲置状态
            AddTransition<StateIdle>(() => MoveComponent.MoveController.isGrounded);
            // 当实体在Y轴上的速度小于等于0时转换为下落状态
            AddTransition<StateFall>(() => MoveComponent.SpeedOnY <= 0);
        }

        // 进入跳跃状态时的操作
        public override void Enter()
        {
            base.Enter();
            MoveComponent.Jump();
            AnimComponent.PlayAnim(AnimType.Jump);
        }

        // 在跳跃状态下的更新操作
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            MoveComponent.UpdatePos(deltaTime, false);
        }
    }

    // StateFall 类表示一个下落状态，用于处理实体在下落状态下的状态转换和更新。
    public class StateFall : MoveState, IState
    {
        public override string ID => "Fall";

        // 初始化状态转换
        public override void InitTransition()
        {
            base.InitTransition();
            // 当实体接触地面时转换为闲置状态
            AddTransition<StateIdle>(() => MoveComponent.MoveController.isGrounded);
        }

        // 在下落状态下的更新操作
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            MoveComponent.UpdatePos(deltaTime, false);
        }

        // 退出下落状态时的操作
        public override void Exit()
        {
            base.Exit();
            MoveComponent.ResetYSpeed();
        }
    }

    // StateDead 类表示一个死亡状态，用于处理实体在死亡状态下的状态转换和更新。
    public class StateDead : BaseState<string>
    {
        public override string ID => "Dead";

        public LEntity Entity => (StateMachine as RootStateMachine)?.Entity;

        public LAnimComponent AnimComponent => Entity?.GetComponent<LAnimComponent>();

        public LAttrComponent AttrComponent => Entity?.GetComponent<LAttrComponent>();

        public LBattleComponent BattleComponent => Entity?.GetComponent<LBattleComponent>();

        public float deadTime = 0;

        // 进入死亡状态时的操作
        public override void Enter()
        {
            base.Enter();
            deadTime = 0;
            AnimComponent.PlayAnim(AnimType.Dead);
        }

        // 在死亡状态下的更新操作
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            deadTime += deltaTime;

            // 当死亡动画播放完毕后，销毁实体
            if (deadTime > AnimComponent.animConfig.GetAnim(AnimType.Dead).length)
            {
                LEntityMgr.Instance.DestroyEntity(Entity);
            }
        }
    }


}
