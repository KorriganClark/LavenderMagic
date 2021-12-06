using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;

namespace Lavender.LuaUtils
{
    
    public class LuaFastWindow  : OdinEditorWindow
    {
        [MenuItem("LuaTool/LuaFast")]
        private static void Open()
        {
            var window = GetWindow<LuaFastWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        public String code = "UIMgr:ActiveUI(\"BagWindow\")";

        [Button("执行")]
        public void callFunc()
        {
            LuaClient.Excute(code);
        }

    }
}
