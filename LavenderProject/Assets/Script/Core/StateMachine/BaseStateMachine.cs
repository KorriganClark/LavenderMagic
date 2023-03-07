using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Lavender
{
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState
    {
        IState HandleSwitch<T>() where T : BaseState, IState, new()
        {
            return new T();
        }
        void Update(float deltaTime);
        /// <summary>
        /// 入口函数
        /// </summary>
        /// <param name="param"></param>
        void Enter(object param);
        /// <summary>
        /// 结束函数
        /// </summary>
        /// <param name="param"></param>
        void Exit(object param);
    }
    /// <summary>
    /// 状态基类
    /// </summary>
    public class BaseState
    {
        public BaseStateMachine StateMachine { get; set; }

    }


    /// <summary>
    /// 状态机基类
    /// </summary>
    public class BaseStateMachine
    {
        private List<IState> states = new List<IState>();

        private IState currentState;
        public IState CurrentState { get { return currentState; } }

        public BaseStateMachine(Type currentState)
        {
            this.currentState = Activator.CreateInstance(currentState) as IState;
            var state = CurrentState as BaseState;
            state.StateMachine = this;
        }

        public void Update(float deltaTime)
        {
            currentState.Update(deltaTime);
        }
        public IState HandleSwitch<T>() where T : BaseState, IState, new()
        {
            var state = GetState<T>();
            if (state.Equals(currentState))
            {
                return null;
            }
            currentState.Exit(null);
            currentState = state;
            currentState.Enter(null);
            return state;
        }

        public IState GetState<T>() where T : BaseState, IState, new()
        {
            for(int i = 0; i < states.Count; i++)
            {
                if(typeof(T) == states[i].GetType())
                {
                    return states[i];
                }
            }
            var state = new T() as BaseState;
            state.StateMachine = this;
            states.Add(state as IState);
            return (IState)state;
        }
    }
}
