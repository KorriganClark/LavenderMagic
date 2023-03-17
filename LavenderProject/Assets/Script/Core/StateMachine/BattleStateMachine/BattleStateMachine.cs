using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.EventSystems.EventTrigger;

namespace Lavender
{
    public class BattleStateMachine : BaseStateMachine
    {
        public BattleStateMachine(Type currentState, LEntity entity) : base(currentState)
        {
            Entity = entity;
        }
        public LEntity Entity { get; private set; }

        public LAnimComponent AnimComponent { get { return Entity?.GetComponent<LAnimComponent>(); } }
        
    }
}
