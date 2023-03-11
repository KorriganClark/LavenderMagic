using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Lavender
{
    public class MoveStateMachine : BaseStateMachine
    {
        public MoveStateMachine(Type currentState, LEntity entity) : base(currentState)
        {
            Entity = entity;
        }

        public LEntity Entity { get; private set; }
        public LMoveComponent MoveComponent { get { return Entity?.GetComponent<LMoveComponent>(); } }
        public LAnimComponent AnimComponent { get { return Entity?.GetComponent<LAnimComponent>(); } }

    }

    public class MoveState : BaseState
    {
        public LAnimComponent AnimComponent { get { return (StateMachine as MoveStateMachine)?.Entity?.GetComponent<LAnimComponent>(); } }
        public LMoveComponent MoveComponent { get { return (StateMachine as MoveStateMachine)?.Entity?.GetComponent<LMoveComponent>(); } }
        public LAttrComponent AttrComponent { get { return (StateMachine as MoveStateMachine)?.Entity?.GetComponent<LAttrComponent>(); } }


    }

    public class StateIdle : MoveState, IState
    {
        public void InitTransition()
        {
            AddTransition<StateWalk>(() =>
            {
                if (AttrComponent?.CurrentMoveSpeed > 0)
                {
                    return true;
                }
                return false;
            });
        }
        
        public void Enter(object param)
        {
            
        }

        public void Exit(object param)
        {
            return;
        }

        public void Update(float deltaTime)
        {
        }
    }

    public class StateWalk : MoveState, IState
    {
        public void InitTransition()
        {
            AddTransition<StateIdle>(() =>
            {
                if (AttrComponent?.CurrentMoveSpeed <= 0)
                {
                    return true;
                }
                return false;
            });
        }
        public void Enter(object param)
        {
            return;
        }

        public void Exit(object param)
        {
            return;
        }

        public void Update(float deltaTime)
        {
            MoveComponent?.MoveForward();
        }
    }

    public class JumpMove : MoveState, IState
    {

        public void Enter(object param)
        {
            return;
        }

        public void Exit(object param)
        {
            return;
        }

        public void Update(float deltaTime)
        {
            return;
        }
    }

}
