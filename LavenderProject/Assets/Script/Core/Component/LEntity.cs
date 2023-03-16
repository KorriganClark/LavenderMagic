using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Lavender
{
    public class LEntity : LGameObject
    {
        public ScriptableObject config;
        public GameObject Model
        {
            get;set;
        }

    }
}
