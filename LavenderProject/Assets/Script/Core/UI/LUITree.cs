using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Lavender.UI
{
    public class LUITree
    {
        public GameObject targetUIRoot { get; set; }

        public Dictionary<GameObject, LUIElement> elements;

    }
}
