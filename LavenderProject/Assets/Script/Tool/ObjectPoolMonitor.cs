using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using Sirenix.Utilities;
using UnityEngine;
using UnityEditor;
using Sirenix.Utilities.Editor;

namespace Lavender.Monitor
{
    public class ObjectPoolMonitor : OdinEditorWindow
    {
        [MenuItem("Monitor/对象池监视器")]
        private static void Open()
        {
            var window = GetWindow<ObjectPoolMonitor>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        #region Odin 显示



        #endregion

    }
}
