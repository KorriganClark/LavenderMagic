using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Lavender
{


    public delegate bool TransitionHandler();


    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IState
    {

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
    public class BaseState : IState
    {
        protected EStateRequest currentRequest;
        public BaseStateMachine StateMachine { get; set; }
        public Dictionary<TransitionHandler, Type> transitionHandlers = new Dictionary<TransitionHandler, Type>();
        /// <summary>
        /// 构建转移函数
        /// </summary>
        public virtual void InitTransition() 
        {
            currentRequest = StateMachine.CurrentRequest;
        }

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

        public virtual void Update(float deltaTime)
        {
            
        }

        public virtual void Enter(object param)
        {
            
        }

        public virtual void Exit(object param)
        {
            
        }
    }

    public enum EStateRequest
    {
        None,
        Jump,
        Battle
    }

    /// <summary>
    /// 状态机基类
    /// </summary>
    public class BaseStateMachine
    {
        protected virtual bool IsDebug
        {
            get { return false; }
        }

        private List<BaseState> states = new List<BaseState>();
        private Queue<EStateRequest> requests = new Queue<EStateRequest>();
        private BaseState currentState;
        public BaseState CurrentState { get { return currentState; } }
        public EStateRequest CurrentRequest 
        { 
            get
            {
                if(requests.Count == 0)
                {
                    return EStateRequest.None;
                }
                return requests.Dequeue(); 
            }
        }
        
        public BaseStateMachine(Type currentState)
        {
            this.currentState = Activator.CreateInstance(currentState) as BaseState;
            var state = CurrentState;
            state.StateMachine = this;
            this.currentState.InitTransition();
        }
        public void AddRequest(EStateRequest request)
        {
            requests.Enqueue(request);
        }
        public void Update(float deltaTime)
        {
            currentState.CheckTransition();//进行状态转移
            currentState.Update(deltaTime);
            
            if(IsDebug)
            {
                Debug.Log(currentState.ToString());
            }
        }
        public T HandleSwitch<T>() where T : BaseState, IState, new()
        {
            var state = GetState<T>();
            if (state.Equals(currentState))
            {
                return null;
            }
            currentState.Exit(null);
            currentState = state;
            currentState.Enter(null);
            return state ;
        }

        public T GetState<T>() where T : BaseState, new()
        {
            for(int i = 0; i < states.Count; i++)
            {
                if(typeof(T) == states[i].GetType())
                {
                    return states[i] as T;
                }
            }
            var state = new T();
            state.StateMachine = this;
            state.InitTransition();
            states.Add(state);
            return state;
        }
    }
}
