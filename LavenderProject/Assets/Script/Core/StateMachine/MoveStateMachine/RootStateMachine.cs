using System;
using System.Diagnostics;

namespace Lavender
{
    public class RootStateMachine : BaseStateMachine<string, string>
    {
        protected override bool IsDebug
        {
            get { return false; }
        }

        public LEntity Entity { get; private set; }
        public LMoveComponent MoveComponent { get { return Entity?.GetComponent<LMoveComponent>(); } }
        public LAnimComponent AnimComponent { get { return Entity?.GetComponent<LAnimComponent>(); } }

        public void Start(LEntity entity)
        {
            Entity = entity;
            HandleSwitch<StateIdle>();
            isWorking = true;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }
    }

    public class MoveState : BaseState<string>
    {
        public LEntity Entity { get { return (StateMachine as RootStateMachine)?.Entity; } }
        public LAnimComponent AnimComponent { get { return Entity?.GetComponent<LAnimComponent>(); } }
        public LMoveComponent MoveComponent { get { return Entity?.GetComponent<LMoveComponent>(); } }
        public LAttrComponent AttrComponent { get { return Entity?.GetComponent<LAttrComponent>(); } }
        public LBattleComponent BattleComponent { get { return Entity?.GetComponent<LBattleComponent>(); } }
        public void AddJumpAndFallTransition()
        {
            AddTransition<StateJump>(() =>
            {
                if(CurrentRequest == EStateRequest.Jump)
                {
                    return true;
                }
                return false;
            });
            AddTransition<StateFall>(() =>
            {
                if (!MoveComponent.MoveController.isGrounded)
                {
                    return true;
                }
                return false;
            });
        }

        public void AddBattleTransition()
        {
            AddTransition<BattleStateMachine>(() =>
            {
                if (CurrentRequest == EStateRequest.NormalAttack)
                {
                    return true;
                }
                return false;
            });
        }
        public void AddDeadTransition()
        {
            AddTransition<StateDead>(() =>
            {
                if (AttrComponent.GetAttr(EAttrType.HP) <= 0) 
                {
                    return true;
                }
                return false;
            });
        }
    }

    public class StateIdle : MoveState, IState
    {
        public override string ID => "Idle";

        public override void InitTransition()
        {
            base.InitTransition();
            AddTransition<StateWalk>(() =>
            {
                if (AttrComponent?.CurrentMoveSpeed > 0)
                {
                    return true;
                }
                return false;
            });
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

    public class StateWalk : MoveState, IState
    {
        public override string ID => "Walk";

        public override void InitTransition()
        {
            base.InitTransition();
            AddTransition<StateIdle>(() =>
            {
                if (AttrComponent?.CurrentMoveSpeed <= 0)
                {
                    return true;
                }
                return false;
            });
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

    public class StateJump : MoveState, IState
    {
        public override string ID => "Jump";

        public override void InitTransition()
        {
            base.InitTransition();
            AddTransition<StateIdle>(() =>
            {
                if (MoveComponent.MoveController.isGrounded)
                {
                    return true;
                }
                return false;
            });
            AddTransition<StateFall>(() =>
            {
                if (MoveComponent.SpeedOnY <= 0)
                {
                    return true;
                }
                return false;
            });
        }

        public override void Enter()
        {
            base.Enter();
            MoveComponent.Jump();
            AnimComponent.PlayAnim(AnimType.Jump);

        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            MoveComponent.UpdatePos(deltaTime, false);

            //MoveComponent.UpdatePosY(deltaTime);
            //MoveComponent.MoveForward(deltaTime);
        }
    }

    public class StateFall : MoveState, IState
    {
        public override string ID => "Fall";

        public override void InitTransition()
        {
            base.InitTransition();
            AddTransition<StateIdle>(() =>
            {
                if (MoveComponent.MoveController.isGrounded)
                {
                    return true;
                }
                return false;
            });
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            MoveComponent.UpdatePos(deltaTime, false);
        }

        public override void Exit()
        {
            base.Exit();
            MoveComponent.ResetYSpeed();
        }
    }

    public class StateDead : BaseState<string>
    {
        public override string ID => "Dead";
        public LEntity Entity { get { return (StateMachine as RootStateMachine)?.Entity; } }
        public LAnimComponent AnimComponent { get { return Entity?.GetComponent<LAnimComponent>(); } }
        public LAttrComponent AttrComponent { get { return Entity?.GetComponent<LAttrComponent>(); } }
        public LBattleComponent BattleComponent { get { return Entity?.GetComponent<LBattleComponent>(); } }
        public float deadTime = 0;
        public override void Enter()
        {
            base.Enter();
            deadTime = 0;
            AnimComponent.PlayAnim(AnimType.Dead);
        }
        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            deadTime += deltaTime;
            if(deadTime > AnimComponent.animConfig.GetAnim(AnimType.Dead).length)
            {
                LEntityMgr.Instance.DestroyEntity(Entity);
            }
        }
    }


}
