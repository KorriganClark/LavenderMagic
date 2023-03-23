using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender
{
    public class LMonster : LEntity
    {
        public LMonsterConfig Config { get; set; }

        public RootStateMachine StateMachine { get; set; }
        public override void InitParams()
        {
            ComponentTypes = new List<Type>
            {
                typeof(LMoveComponent),
                typeof(LAnimComponent),
                typeof(LAttrComponent),
                typeof(LBattleComponent),
            };

            Config = config as LMonsterConfig;
            if (Config == null)
            {
                throw new Exception("No config!");
            }

            Model = GameObject.Instantiate(Config.Model);
            Model.transform.SetParent(Root.transform);
            Model.transform.localPosition = Vector3.zero;
        }
        public override void OnInit()
        {
            StateMachine = new RootStateMachine();
            StateMachine.Start(this);
        }
    }


}
