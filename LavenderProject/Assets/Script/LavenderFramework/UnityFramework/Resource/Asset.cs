using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lavender.Framework.ResourceManager;
using UnityEngine;

namespace Lavender.UnityFramework
{
    public class Asset : AssetTask
    {
        public GameObject AssetGO 
        { 
            get
            {
                return (GameObject)base.Asset;
            } 
        }
    }
}
