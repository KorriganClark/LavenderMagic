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
    /// <summary>
    /// UITree，一棵 UI树，该类负责读取Yaml文件的初步解析结果并将其转化为有结构的树形式。
    /// </summary>
    public class LUITree
    {
        /// <summary>
        /// 根节点
        /// </summary>
        public UINode treeRoot;

        /// <summary>
        /// 所有节点,键为fileID
        /// </summary>
        private Dictionary<string, UINode> uiNodes = new Dictionary<string, UINode>();

        /// <summary>
        /// 所有节点上挂载的组件
        /// </summary>
        private Dictionary<string, UINodeComponent> uiComps = new Dictionary<string, UINodeComponent>();

        /// <summary>
        /// 用于处理一个 YamlNode 结构的委托形式
        /// </summary>
        /// <param name="node"></param>
        public delegate void DealMessageTask(YAML.Node node);

        /// <summary>
        /// 当前需要处理的信息,该 task 是为了解决 Yaml 中的数组形式
        /// </summary>
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

        /// <summary>
        /// 解析单个 node 的信息,这个 node 是 Yaml 格式中的单位
        /// </summary>
        /// <param name="node"></param>
        public void Parse(YAML.Node node)
        {
            var value = node.value;
            
            //构建新的节点
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

            //层级为 0 时,是解析组件
            if(node.tier == 0)
            {
                //排掉一些注释类信息
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
            else//否则即为给已有的节点或组件设置数据
            {
                ParseProperty(node);
            }
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="node"></param>
        public  void ParseProperty(YAML.Node node)
        {
            var fatherID = node?.parent?.value;
            //数组形式的数据,交给事先定义好的 task 处理
            if(fatherID == null && node?.tier >= 2)
            {
                if (CurrentTask != null)
                {
                    CurrentTask(node);
                }
                return;
            }
            //设置节点属性
            if (uiNodes.ContainsKey(fatherID))
            {
                uiNodes[fatherID].SetProperty(node);
            }
            else if(uiComps.ContainsKey(fatherID))//设置组件属性
            {
                if(uiComps[fatherID]== null)
                {
                    Debug.LogError("Comp Null!");
                }
                uiComps[fatherID].SetProperty(node);//调用具体的设置方法
                if (node.name == "m_Father")//如果是 transform 中的父子关系属性,将其反馈到节点关系中,todo 第一个 gameObject 不一定是根节点,需要全部生成后再构建父子关系
                {
                    var fatherFileID = UINodeComponent.ParseFileID(node.value);
                    if (fatherFileID != "0")
                    {
                        var currentNodeID = uiComps[fatherID].GameObjectID;
                        var fatherNodeID = uiComps[fatherFileID].GameObjectID;
                        uiNodes[fatherNodeID].AddChild(uiNodes[currentNodeID]);
                    }

                }
                else if (node.name == "m_Script") //失败的尝试
                {
                    //var compTemp = (UIMonoBehaviour)uiComps[fatherID];
                    //uiComps[fatherID] = (UINodeComponent)Convert.ChangeType(uiComps[fatherID], compTemp.BehaviourType);
                    //后面设置数据能够根据不同的脚本类型进行设置
                }

            }
            
            
            
        }

        /// <summary>
        /// 节点，即单个元素，UI的最小划分单位，以单个 GameObject 为区分。节点之间有父子关系，节点共同组成 UITree
        /// </summary>
        public class UINode
        {

            private UINode parent;
            private ElementType elementType;
            private string gameObjID;
            //给组件识别父子关系用的
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

        /// <summary>
        /// 组件类，每个节点上都会挂载复数个组件，组件描述了节点的类型、数据、父子关系。在 Yaml 中，组件与 GameObject 同级，但解析后，组件附属于节点
        /// </summary>
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
                    case "MonoBehaviour":
                        return new UIMonoBehaviour(node.value);
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

            public static Vector2 ParseVector2D(string context)
            {
                string[] ss = context.Split(' ');
                return new Vector2(float.Parse(ss[1].Substring(0, ss[1].Length - 1)), float.Parse(ss[3].Substring(0, ss[3].Length - 1)));
            } 

        }

        /// <summary>
        /// RectTransform 组件，决定 UI 的大小、位置、父子关系等的组件，该组件挂载完毕后，能将节点挂载到 UI 树中，形成父子关系。
        /// </summary>
        public class UINodeRectTransform: UINodeComponent
        {
            public UINodeRectTransform(string id) : base(id)
            {
            }

            private string fatherID;

            private Vector2 m_AnchorMin;

            private Vector2 m_AnchorMax;

            private Vector2 m_AnchorPostion;

            public override void SetProperty(YAML.Node node)
            {
                base.SetProperty(node);
                switch (node.name)
                {
                    case "m_Father":
                        fatherID = ParseFileID(node.value);
                        break;
                    case "m_AnchorMin":
                        m_AnchorMin = ParseVector2D(node.value);
                        break;
                    case "m_AnchorMax":
                        m_AnchorMax = ParseVector2D(node.value);
                        break;
                    case "m_AnchorPostion":
                        m_AnchorPostion = ParseVector2D(node.value);
                        break;
                }
            }
        }
        
        /// <summary>
        /// 脚本组件，如 Text,Image 等，该脚本决定了该节点的类型，同时脚本类型在后续读取 m_Script 时才能决定
        /// </summary>
        public class UIMonoBehaviour : UINodeComponent
        {
            public UIMonoBehaviour(string id) : base(id)
            {
            }

            private string m_Script;

            private dynamic scriptInstance;

            public Type BehaviourType
            {
                get
                {
                    switch (m_Script)
                    {
                        case "5f7201a12d95ffc409449d95f23cf332":
                            return typeof(UINodeTextPart);
                        case "fe87c0e1cc204ed48ad3b37840f39efc":
                            return typeof(UINodeImagePart);
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
                        scriptInstance = System.Activator.CreateInstance(BehaviourType);//利用 Type 创建实例
                        break;
                }
                if(scriptInstance != null)
                {
                    scriptInstance.SetProperty(node);
                }
            }
        }

        /// <summary>
        /// Text 组件，以组件的形式挂载到Monobehaviour下，因为不能将父类转换为子类，所以不能以继承的形式。
        /// </summary>
        public class UINodeTextPart
        {
            private string m_Text;

            public void SetProperty(YAML.Node node)
            {
                switch (node.name)
                {
                    case "m_Text":
                        m_Text = node.value;
                        break;
                }
            }
        }

        public class UINodeImagePart
        {

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

            public void SetProperty(YAML.Node node)
            {
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
