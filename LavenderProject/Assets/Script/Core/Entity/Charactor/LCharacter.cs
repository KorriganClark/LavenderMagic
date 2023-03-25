using Lavender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender
{
    public class LCharacter : LEntity
    {
        public override void InitParams()
        {
            ComponentTypes = new List<Type> 
            { 
                typeof(LMoveComponent),
                typeof(LAnimComponent),
                typeof(LAttrComponent),
                typeof(LBattleComponent),
            };
            Name = Config.EntityName;
            Model = GameObject.Instantiate(Config.Model);
            Model.transform.SetParent(Root.transform);
            Model.transform.localPosition = Vector3.zero;
        }
    }
}
