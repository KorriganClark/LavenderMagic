
using UnityEngine;

namespace Lavender
{
    public class BattleStateMachine : BaseStateMachine<string, int>
    {
        public override string ID => "Battle";
        public LEntity Entity { get { return (StateMachine as RootStateMachine)?.Entity; } }
        public LAnimComponent AnimComponent { get { return Entity?.GetComponent<LAnimComponent>(); } }
        public LBattleComponent BattleComponent { get { return Entity?.GetComponent<LBattleComponent>(); } }

        public override void InitTransition()
        {
            base.InitTransition();
            
            AddTransition<StateIdle>(() =>
            {
                if (CurrentState is BattleExitState)
                {
                    return true;
                }
                return false;
            });
        }

        public override void Enter()
        {
            base.Enter();
            AddRequest(StateMachine.CurrentRequest);
            HandleSwitch<BattleEnterState>();
            isWorking = true;
        }

        public override void Exit()
        {
            base.Exit();
        }
    }

    public class BattleEnterState : BaseState<int>
    {
        public override int ID => 0;
        public LEntity Entity { get { return (StateMachine as BattleStateMachine)?.Entity; } }
        public LBattleComponent BattleComponent { get { return Entity?.GetComponent<LBattleComponent>(); } }
        public override void InitTransition()
        {
            base.InitTransition();
            var id = BattleComponent.GetSkillByKey(ESkillKey.NormalAttack).Config.SkillID;
            AddTransition<SkillState>(() =>
            {
                if (CurrentRequest == EStateRequest.NormalAttack)
                {
                    return true;
                }
                return false;
            }, id);
        }
    }

    public class BattleExitState : BaseState<int>
    {
        public override int ID => -1;
    }

    public class SkillState : BaseState<int>
    {
        public LEntity Entity { get { return (StateMachine as BattleStateMachine)?.Entity; } }
        public LBattleComponent BattleComponent { get { return Entity?.GetComponent<LBattleComponent>(); } }
        public LSkill Skill { get; set; }
        public LSkillConfig Config { get { return Skill.Config; } }
        public LSkillInstance Instance { get; set; }
        public override void Init(int ID)
        {
            base.Init(ID);
            Skill = BattleComponent.GetSkillByKey(ESkillKey.NormalAttack);
        }

        public override void InitTransition()
        {
            base.InitTransition();
            AddTransition<BattleExitState>(() =>
            {
                if (Instance != null && Instance.OutOfTime)
                {
                    return true;
                }
                return false;
            });
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("SkillStateEnter");
            Instance = BattleComponent.UseSkill(Skill);
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
        }

        public override void Exit()
        {
            base.Exit();
            Instance = null;
            Debug.Log("SkillStateExit");

        }

        public bool CanBeInterrupt()
        {
            return false;
        }

        public bool IsOver()
        {
            return Instance.OutOfTime;
        }

    }


}
