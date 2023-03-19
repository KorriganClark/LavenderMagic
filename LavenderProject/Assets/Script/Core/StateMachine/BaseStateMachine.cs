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
        void Enter();
        /// <summary>
        /// 结束函数
        /// </summary>
        /// <param name="param"></param>
        void Exit();
    }
    /// <summary>
    /// 状态基类
    /// </summary>
    public class BaseState<TStateID> : IState
    {
        protected EStateRequest currentRequest;
        /// <summary>
        /// 转移函数
        /// </summary>
        private Dictionary<TransitionHandler, TStateID> transitionHandlers = new Dictionary<TransitionHandler, TStateID>();
        private Dictionary<TStateID, Type> transitionTypes = new Dictionary<TStateID, Type>(); 
        public virtual TStateID ID { get; private set; }
        /// <summary>
        /// 归属的状态机
        /// </summary>
        public IStateMachine<TStateID> StateMachine { get; set; }
        /// <summary>
        /// 初始化，根据唯一ID进行相应配置
        /// </summary>
        /// <param name="ID"></param>
        public virtual void Init(TStateID ID)
        {
            this.ID = ID;
        }
        /// <summary>
        /// 构建转移函数
        /// </summary>
        public virtual void InitTransition() 
        {
        }

        public void AddTransition<T>(TransitionHandler handler) where T : BaseState<TStateID>, IState, new()
        {
            var state = StateMachine.GetState<T>();
            transitionHandlers.Add(handler, state.ID);
            transitionTypes.Add(state.ID, typeof(T));
        }
        public void AddTransition<T>(TransitionHandler handler, TStateID stateID) where T : BaseState<TStateID>, IState, new()
        {
            transitionHandlers.Add(handler, stateID);
            transitionTypes.Add(stateID, typeof(T));
        }

        public void CheckTransition()
        {
            if (StateMachine == null) { return; }
            if(!(this is IStateMachine<TStateID>))//子状态机不直接处理请求
            {
                currentRequest = StateMachine.CurrentRequest;
            }
            MethodInfo mi;
            foreach (var pair in transitionHandlers)
            {
                var handler = pair.Key;
                if (handler())
                {
                    if (currentRequest != EStateRequest.None)
                    {
                        StateMachine.AddRequest(currentRequest);//给下一个状态或子状态机使用
                    }
                    //大量性能浪费，可以将mi存储起来，以 MethodInfo为值来构建Dic
                    mi = StateMachine.GetType().GetMethod("HandleSwitch", new Type[] {typeof(TStateID)}).MakeGenericMethod( transitionTypes[pair.Value]);
                    mi.Invoke(StateMachine, new object[] { pair.Value });
                }
            }
        }

        public virtual void Update(float deltaTime)
        {
            
        }

        public virtual void Enter()
        {
            
        }

        public virtual void Exit()
        {
            
        }
    }

    public enum EStateRequest
    {
        None,
        Jump,
        NormalAttack,
        ElementalSkill,
        ElementalBurst
    }

    public interface IStateMachine<TChildStateID>
    {
        EStateRequest CurrentRequest { get; }
        void AddRequest(EStateRequest request);
        T HandleSwitch<T>() where T : BaseState<TChildStateID>, IState, new();
        T HandleSwitch<T>(TChildStateID childStateID) where T : BaseState<TChildStateID>, IState, new();
        T GetState<T>() where T : BaseState<TChildStateID>, new();
        T GetState<T>(TChildStateID stateID) where T : BaseState<TChildStateID>, new();

        BaseState<TChildStateID> CurrentState { get; }
        BaseState<TChildStateID> LastState { get; }

    }

    /// <summary>
    /// 状态机基类
    /// TStateID 为自身状态标注类型
    /// TChildStateID 为子状态标注类型
    /// </summary>
    public class BaseStateMachine<TStateID, TChildStateID> : BaseState<TStateID>, IStateMachine<TChildStateID>
    {
        protected virtual bool IsDebug
        {
            get { return false; }
        }

        private Dictionary<TChildStateID, BaseState<TChildStateID>> states = new Dictionary<TChildStateID, BaseState<TChildStateID>> ();
        //private List<BaseState<TStateID>> states = new List<BaseState<TStateID>>();
        private Queue<EStateRequest> requests = new Queue<EStateRequest>();
        private BaseState<TChildStateID> currentState;
        private BaseState<TChildStateID> lastState;
        protected bool isWorking = false;
        protected bool Working { get { return isWorking; } }
        public BaseState<TChildStateID> CurrentState { get { return currentState; } }
        public BaseState<TChildStateID> LastState { get { return lastState; } }
        public bool IsRootMachine { get { return StateMachine == null; } }
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

        public override void Update(float deltaTime)
        {
            if(!Working)
            {
                return;
            }
            currentState.CheckTransition();//进行状态转移
            currentState.Update(deltaTime);
            
            if(IsDebug)
            {
                Debug.Log(currentState.ToString());
            }
        }

        public void AddRequest(EStateRequest request)
        {
            requests.Enqueue(request);
        }
        public T HandleSwitch<T>() where T : BaseState<TChildStateID>, IState, new()
        {
            return HandleSwitch<T>(default(TChildStateID));
        }
        public T HandleSwitch<T>(TChildStateID childStateID) where T : BaseState<TChildStateID>, IState, new()
        {
            T state;
            if (childStateID == null)
            {
                state = GetState<T>();
            }
            else
            {
                state = GetState<T>(childStateID);
            }
            if (currentState != null && state.Equals(currentState))
            {
                return null;
            }
            currentState?.Exit();//初始化时可能没有前置状态
            lastState = currentState;
            currentState = state;
            currentState.Enter();
            return state;
        }
        public T GetState<T>() where T : BaseState<TChildStateID>, new()
        {
            foreach(var pair in states)
            {
                if(typeof(T) == pair.Value.GetType())
                {
                    return pair.Value as T;
                }
            }
            var state = new T();
            state.StateMachine = this;
            states.Add(state.ID, state);
            state.InitTransition();
            return state;
        }
        public T GetState<T>(TChildStateID stateID) where T : BaseState<TChildStateID>, new()
        {
            BaseState<TChildStateID> state;
            if(!states.TryGetValue(stateID, out state))
            {
                state = new T();
                states.Add(state.ID, state);
                state.StateMachine = this;
                state.Init(stateID);
                state.InitTransition();
            }
            return (T)state;
        }
    }
}
