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

        YAML yaml;
        LUITree uiTree;
        YamlMappingNode prefabNode;
        [FilePath(ParentFolder = "Assets/Artres/UI")]
        [OnValueChanged("LoadPrefabYamlFile")]
        public string prefabPath;

        #endregion

        protected override void OnEnable()
        {
            //yaml = new YAML();
        }
        LUITree tree;
        public void LoadPrefabYamlFile()
        {
            //TextReader reader = File.OpenText(Application.dataPath + "/Artres/UI/" + prefabPath);
            yaml = new YAML(Application.dataPath + "/Artres/UI/" + prefabPath);
            tree = ParseYAML(yaml);

            tree.Print();
        }

        public LUITree ParseYAML(YAML yaml)
        {
            YAML.Node yNode = yaml.RootNode();
            if(yNode.name != "GameObject")
            {
                Debug.LogError("Root not GameObject!!");
                return null;
            }
            LUITree tree = LUITree.BuildTree(yNode.value);
            foreach(var node in yaml.AllNode())
            {
                tree.Parse(node);
            }
            return tree;
        }

        [Button("Print")]
        public void PrintPrefab()
        {
            tree.Print();
            //Debug.Log(yaml.Read("m_Component"));
        }

    }

}

