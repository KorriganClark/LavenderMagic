using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.Collections;
using Sirenix.Utilities;
using Sirenix.OdinInspector.Editor;
using YamlDotNet;
using UnityEditor;
using Sirenix.Utilities.Editor;
using YamlDotNet.RepresentationModel;
using Sirenix.OdinInspector;
using System.IO;

namespace Lavender.UI
{
    public class LUITree
    {
        public UINode treeRoot;

        private Dictionary<string, UINode> uiNodes = new Dictionary<string, UINode>();

        private Dictionary<string, UINodeComponent> uiComps = new Dictionary<string, UINodeComponent>();

        public delegate void DealMessageTask(YAML.Node node);

        public static DealMessageTask CurrentTask;//需要改成堆栈

        public static LUITree BuildTree(string context)
        {
            LUITree tree = new LUITree();
            //tree.treeRoot = UINode.NewNode(context);
            return tree;
        }

        public void Print()
        {
            UINode currentNode = treeRoot;
            List<UINode> queue = new List<UINode>();
            queue.Add(currentNode);
            while(queue.Count > 0)
            {
                currentNode = queue[0];
                queue.RemoveAt(0);
                queue.AddRange(currentNode.Childs);
                currentNode.Print();
            }
        }

        public void Parse(YAML.Node node)
        {
            var value = node.value;
            if (value != null && node.name == "GameObject")
            {
                if (!uiNodes.ContainsKey(value))
                {
                    uiNodes.Add(value, UINode.NewNode(value));
                    if(treeRoot == null)
                    {
                        treeRoot = uiNodes[value];
                    }
                }
                return;
            }

            if(node.tier == 0)
            {
                if (value!= null && !uiComps.ContainsKey(value))
                {

                    switch (node.name)
                    {
                        case "RectTransform":
                            uiComps.Add(value, UINodeRectTransform.NewNode(node));
                            break;
                        case "MonoBehaviour":
                            uiComps.Add(value, UIMonoBehaviour.NewNode(node));
                            break;
                    }
                }
                return;
            }
            else
            {
                ParseProperty(node);
            }
        }

        public  void ParseProperty(YAML.Node node)
        {
            var fatherID = node?.parent?.value;
            if(fatherID == null && node?.tier >= 2)
            {
                if (CurrentTask != null)
                {
                    CurrentTask(node);
                }
                return;
            }
            
            if (uiNodes.ContainsKey(fatherID))
            {
                uiNodes[fatherID].SetProperty(node);
            }
            else if(uiComps.ContainsKey(fatherID))
            {
                uiComps[fatherID].SetProperty(node);
                if (node.name == "m_Father")
                {
                    var fatherFileID = UINodeComponent.ParseFileID(node.value);
                    if (fatherFileID != "0")
                    {
                        var currentNodeID = uiComps[fatherID].GameObjectID;
                        var fatherNodeID = uiComps[fatherFileID].GameObjectID;
                        uiNodes[fatherNodeID].AddChild(uiNodes[currentNodeID]);
                    }

                }
                else if (node.name == "m_Script")
                {
                    var compTemp = (UIMonoBehaviour)uiComps[fatherID];
                    uiComps[fatherID] = (UINodeComponent)Convert.ChangeType(uiComps[fatherID], compTemp.BehaviourType);
                    //后面设置数据能够根据不同的脚本类型进行设置
                }

            }
            
            
            
        }

        public class UINode
        {

            private UINode parent;
            private ElementType elementType;
            private string gameObjID;
            private YAML.Node componentYamlNode;
            
            private List<string> compIDs = new List<string>();
            private List<UINode> childs = new List<UINode>();

            private string m_Name;

            public List<UINode> Childs
            {
                get
                {
                    return childs;
                }
            }

            public string GameObjectID
            {
                get
                {
                    return gameObjID;
                }
            }
            public void AddChild(UINode node)
            {
                childs.Add(node);
            }

            public static UINode NewNode(string context)
            {
                UINode res = new UINode();
                res.Parse(context);
                return res;
            }

            public void Parse(string context)
            {
                gameObjID = context;

            }

            public static string ParseFileID(string context)
            {
                string[] ss = context.Split(' ');
                return ss[1].Substring(0, ss[1].Length - 1);
            }

            /// <summary>
            /// 添加组件，以委托的形式交给LUITree进行调用。
            /// </summary>
            /// <param name="node"></param>
            public void AddComponent(YAML.Node node)
            {
                if(node?.parent == null)
                {
                    return;
                }
                if (componentYamlNode == node.parent)
                {
                    string []ss = node.value.Split(' ');
                    string res = ss[1].Substring(0, ss[1].Length-1);

                    compIDs.Add(res);
                }
            }

            /// <summary>
            /// 解析 Component，将节点保存，生成委托以获取接下来的消息
            /// </summary>
            /// <param name="node"></param>
            public void ParseComponent(YAML.Node node)
            {
                componentYamlNode = node;
                CurrentTask = AddComponent;
            }

            /// <summary>
            /// 设置某个属性
            /// </summary>
            /// <param name="node"></param>
            public void SetProperty(YAML.Node node)
            {
                if (node == null || node.name == null)
                {
                    Debug.LogError("Invalid Node, Please Check Prefab Yaml!");
                    return;
                }
                switch (node.name)
                {
                    case "m_Name":
                        m_Name = node.value;
                        break;
                    case "m_Component":
                        ParseComponent(node);
                        break;

                }

            }

            public void Print()
            {
                Debug.Log("ID: " + gameObjID);
                foreach (var comps in compIDs)
                {
                    //Debug.Log("Comps: " + comps);
                }
            }
        }

        public class UINodeComponent
        {
            private string compID;
            //private string compType;
            private string gameObjectID;

            public string GameObjectID
            {
                get
                {
                    return gameObjectID;
                }
            }

            public UINodeComponent(string id)
            {
                this.compID = id;
            }
            
            public static UINodeComponent NewNode(YAML.Node node)
            {
                //UINodeComponent node = new UINodeComponent();

                switch (node.name)
                {
                    case "RectTransform":
                        return new UINodeRectTransform(node.value);
                    

                }
                return null;
            }

            public virtual void SetProperty(YAML.Node node)
            {
                switch (node.name)
                {
                    case "m_GameObject":
                        gameObjectID = ParseFileID(node.value);
                        break;
                }
            }

            public static string ParseFileID(string context)
            {
                string[] ss = context.Split(' ');
                return ss[1].Substring(0, ss[1].Length - 1);
            }

        }

        public class UINodeRectTransform: UINodeComponent
        {
            public UINodeRectTransform(string id) : base(id)
            {
            }

            private string fatherID;

            public override void SetProperty(YAML.Node node)
            {
                base.SetProperty(node);
                switch (node.name)
                {
                    case "m_Father":
                        fatherID = ParseFileID(node.value);
                        break;
                }
            }
        }
        public class UIMonoBehaviour : UINodeComponent
        {
            public UIMonoBehaviour(string id) : base(id)
            {
            }

            private string m_Script;

            public Type BehaviourType
            {
                get
                {
                    switch (m_Script)
                    {
                        case "5f7201a12d95ffc409449d95f23cf332":
                            return typeof(UINodeText);
                        case "fe87c0e1cc204ed48ad3b37840f39efc":
                            return typeof(UINodeImage);
                    }
                    return null;
                }
            }

            public static string ParseGUID(string context)
            {
                string[] ss = context.Split(' ');
                return ss[3].Substring(0, ss[3].Length - 1);
            }

            public override void SetProperty(YAML.Node node)
            {
                base.SetProperty(node);
                switch (node.name)
                {
                    case "m_Script":
                        m_Script = ParseGUID(node.value);
                        break;
                }
            }
        }

        public class UINodeText : UIMonoBehaviour
        {
            public UINodeText(string id) : base(id)
            {
            }

            private string m_Text;

            public override void SetProperty(YAML.Node node)
            {
                base.SetProperty(node);
                switch (node.name)
                {
                    case "m_Text":
                        m_Text = node.value;
                        break;
                }
            }
        }

        public class UINodeImage : UIMonoBehaviour
        {
            public UINodeImage(string id): base(id)
            {
            }

            private Color color = new Color();

            private void ParseColor(string context)
            {
                string[] ss = context.Split(' ');
                color.r = float.Parse(ss[1].Substring(0, ss[1].Length - 1));
                color.g = float.Parse(ss[3].Substring(0, ss[3].Length - 1));
                color.b = float.Parse(ss[5].Substring(0, ss[5].Length - 1));
                color.a = float.Parse(ss[7].Substring(0, ss[7].Length - 1));
                //return ss[3].Substring(0, ss[3].Length - 1);
            }

            public override void SetProperty(YAML.Node node)
            {
                base.SetProperty(node);
                switch (node.name)
                {
                    case "m_Color":
                        ParseColor(node.value);
                        break;
                }
            }
        }

    }
}
