using Lavender;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Core.Entity
{
    public class LCharacter : LEntity
    {
        public LCharacterConfig Config { get; set; }

        public override void InitParams()
        {
            ComponentTypes = new List<Type> 
            { 
                typeof(LMoveComponent),
                typeof(LAnimComponent),
                typeof(LAttrComponent),
            };

            Config = config as LCharacterConfig;
            if(Config == null)
            {
                throw new Exception("No config!");
            }
            
            Model = GameObject.Instantiate(Config.Model);
            Model.transform.SetParent(Root.transform);
            Model.transform.localPosition = Vector3.zero;
        }
    }
}
