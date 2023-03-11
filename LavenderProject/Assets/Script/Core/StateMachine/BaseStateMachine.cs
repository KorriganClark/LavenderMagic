using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace Lavender
{


    public delegate bool TransitionHandler();


    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// 构建转移函数
        /// </summary>
        void InitTransition() { }

        void Update(float deltaTime) { }
        /// <summary>
        /// 入口函数
        /// </summary>
        /// <param name="param"></param>
        void Enter(object param) { }
        /// <summary>
        /// 结束函数
        /// </summary>
        /// <param name="param"></param>
        void Exit(object param) { }
    }
    /// <summary>
    /// 状态基类
    /// </summary>
    public class BaseState
    {
        public BaseStateMachine StateMachine { get; set; }
        public Dictionary<TransitionHandler, Type> transitionHandlers = new Dictionary<TransitionHandler, Type>();

        public void AddTransition<T>(TransitionHandler handler) where T : BaseState, IState, new()
        {
            transitionHandlers.Add(handler, typeof(T));
        }

        public void CheckTransition()
        {
            if (StateMachine == null) { return; }
            MethodInfo mi;
            foreach (var pair in transitionHandlers)
            {
                var handler = pair.Key;
                if (handler())
                {
                    //大量性能浪费，可以将mi存储起来，以 MethodInfo为值来构建Dic
                    mi = StateMachine.GetType().GetMethod("HandleSwitch").MakeGenericMethod(pair.Value);
                    mi.Invoke(StateMachine, new object[0]);
                }
            }
        }
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
            this.currentState.InitTransition();
        }

        public void Update(float deltaTime)
        {
            (currentState as BaseState).CheckTransition();//进行状态转移
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
            var iState = state as IState;
            iState.InitTransition();
            states.Add(iState);
            return iState;
        }
    }
}
