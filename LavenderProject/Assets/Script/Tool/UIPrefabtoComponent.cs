using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;
using YamlDotNet;
using System;
using UnityEditor;
using Sirenix.Utilities.Editor;
using YamlDotNet.RepresentationModel;
using Sirenix.OdinInspector;
using System.IO;
using Lavender.Lua;
using Lavender.Common;

namespace Lavender.UI.UITool
{
    public class UIPrefabtoComponent : OdinEditorWindow
    {

        [MenuItem("UITool/UIPrefabtoComponent")]
        private static void Open()
        {
            var window = GetWindow<UIPrefabtoComponent>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        #region

        [FilePath(ParentFolder = "Assets/Artres/UI")]
        [OnValueChanged("LoadPrefabYamlFile")]
        public string prefabPath;

        public string prefabName;

        [FolderPath(ParentFolder = "Assets/Script/Lua/LavenderUI/Components")]
        //[InfoBox(message: "如果该路径下已有重名脚本，仅刷新注释", visibleIfMemberName: "CheckPath")]
        public string scriptFolderPath;
        #endregion

        protected override void OnEnable()
        {
            //yaml = new YAML();
        }



        [Button("生成Lua代码")]
        public void GenLuaFilebyPrefab()
        {
            var LuaFilePath = Application.dataPath + "/Script/Lua/LavenderUI/Components/" + scriptFolderPath + "/" + prefabName + ".lua";
            var document = new LuaDocumentNode();

            document.ClassName = prefabName;
            document.AddField(new LuaFieldNode(document.ClassName, LuaMemberType.Local, new LuaScriptStatementNode($"Roact.Component:extend(\"{document.ClassName}\")")));
            string[] testString = { "test1", "test2" };

            document.AddField(new LuaFieldNode(document.ClassName + ".state", LuaMemberType.Global, new LuaScriptStateNode(testString)));
            document.AddField(new LuaFieldNode(document.ClassName + ".slot", LuaMemberType.Global, new LuaScriptStateNode(testString)));
            document.AddField(new LuaFieldNode(document.ClassName + ".props", LuaMemberType.Global, new LuaScriptStateNode(testString)));

            var dataBindFunc = new LuaFunctionNode("dataBind", LuaMemberType.Local);
            dataBindFunc.statementNodes.Add(new LuaScriptStatementNode($"self.slot = self.state"));

            var buildTreeFunc = new LuaFunctionNode("buildTree", LuaMemberType.Local);
            buildTreeFunc.statementNodes.Add(new LuaUITreeNode(new GameObject()));

            var initFunc = new LuaFunctionNode("init", LuaMemberType.Local);
            initFunc.statementNodes.Add(new LuaScriptStatementNode($"self:setState({document.ClassName}.state)"));

            var renderFunc = new LuaFunctionNode("render", LuaMemberType.Local);
            renderFunc.statementNodes.Add(new LuaScriptStatementNode($"self.props = {document.ClassName}.props"));
            renderFunc.statementNodes.Add(new LuaScriptStatementNode($"{document.ClassName}.dataBind(self)"));
            renderFunc.statementNodes.Add(new LuaScriptStatementNode($"return {document.ClassName}.buildTree(self)"));

            document.AddFunction(dataBindFunc);
            document.AddFunction(buildTreeFunc);
            document.AddFunction(initFunc);
            document.AddFunction(renderFunc);
            FileEx.SaveText(document.ToString(), LuaFilePath);
        }


    }

}

