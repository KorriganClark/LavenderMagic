
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class YAMLChangeHelper
{
    public static void ChangeMaterialYAML(string material_path, string shader_path)
    {
        YAML yaml = new YAML(material_path);

        Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(shader_path);
        if (shader != null)
        {
            var fileID = GetFileID(shader);
            var GUID = AssetDatabase.AssetPathToGUID(shader_path);
            var context = "{fileID: " + fileID + ", guid: " + GUID + ", type: 3}";

            yaml.Modify("m_Shader", context);
            yaml.Save();
        }
        else
        {
            Debug.LogError("[YAMLChangeHelper][TestRead]Can't Find Shader!!! path = " + shader_path);
        }
    }

    private static readonly PropertyInfo InspectorMode = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);
    public static long GetFileID(UnityEngine.Object target)
    {
        SerializedObject serializedObject = new SerializedObject(target);
        InspectorMode.SetValue(serializedObject, UnityEditor.InspectorMode.Debug, null);
        SerializedProperty localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile");
        return localIdProp.longValue;
    }
}



public class YAML
{
    private string[] m_lines;
    private List<Node> m_node_list = new List<Node>();
    private string m_path;
    private bool m_binary;

    public YAML(string path)
    {
        this.m_path = path;

        //this.m_lines = File.ReadAllLines(path);

        List<string> lines = new List<string>();
        var stream = File.OpenRead(path);
        var lineCount = 0;
        using (StreamReader sr = new StreamReader(stream))
        {
            while (sr.Peek() >= 0)
            {
                var line = sr.ReadLine();
                if (lineCount == 0)
                {
                    if (line != "%YAML 1.1")
                    {
                        m_binary = true;
                        break;
                    }
                }
                lines.Add(line);
                lineCount++;
            }
        }

        this.m_lines = lines.ToArray();

        string fileIdTemp = "";
        for (int i = 0; i < m_lines.Length; i++)
        {
            String line = m_lines[i];
            if (line.Trim() == "")
            {
                continue;
            }
            else if (line.Trim().Substring(0, 1) == "#")
            {
                continue;
            }

            String[] kv = Regex.Split(line, ":", RegexOptions.IgnoreCase);
            //FindPreSpace(line);
            Node node = new Node();
            node.space = FindPreSpace(line);
            node.name = kv[0].Trim();

            // 去除前后空白符
            String fline = line;//.Trim();
            node.hasValue = fline.Contains(":");
            int first = fline.IndexOf(':');
            int fileIdTag = fline.IndexOf("&");

            if(fileIdTag != -1)
            {
                fileIdTemp = fline.Substring(fileIdTag + 1, fline.Length - fileIdTag - 1);
                continue;
            }
            else if (first == -1)
            {
                node.value = null;
            }
            else
            {
                if (fline.Contains("%"))
                {
                    node.value = first == fline.Length - 1 ? null : fline.Substring(first + 1, fline.Length - first - 1);
                }
                else
                {
                    node.value = first == fline.Length - 1 ? null : fline.Substring(first + 2, fline.Length - first - 2);
                }
            }
            if(node.value == null && fileIdTemp != "")
            {
                node.value = fileIdTemp;
                fileIdTemp = "";
            }
            node.parent = FindParent(node.space);
            m_node_list.Add(node);
        }

        //Formatting是为了定出tier，但是对于Scene这种上mb级别的文件速度太慢
        this.Formatting();
    }

    public bool Binary()
    {
        return m_binary;
    }

    public void Modify(string key, string value)
    {
        Node node = FindNodeByKey(key);
        if (node != null)
        {
            node.value = value;
        }
    }

    public string CheckBuildInRes()
    {
        foreach (var item in m_lines)
        {
            if (item.Contains("0000000000000000f000000000000000"))
            {
                return item;
            }
        }
        return null;
    }

    public Node RootNode()
    {
        if(m_node_list.Count < 2)
        {
            return null;
        }
        Node node = m_node_list[2];
        return node;
    }
    public List<Node> AllNode()
    {
        return m_node_list;
    }

    // 读取值
    public string Read(string key)
    {
        Node node = FindNodeByKey(key);
        if (node != null)
        {
            return node.value;
        }
        return null;
    }

    public List<Node> FindAllNodeByKey(string key)
    {
        var list = new List<Node>();
        string[] ks = key.Split('.');
        for (int i = 0; i < m_node_list.Count; i++)
        {
            Node node = m_node_list[i];
            if (node.name == ks[ks.Length - 1])
            {
                // 判断父级
                Node tem = node;
                // 统计匹配到的次数
                int count = 1;
                for (int j = ks.Length - 2; j >= 0; j--)
                {
                    if (tem.parent.name == ks[j])
                    {
                        count++;
                        // 继续检查父级
                        tem = tem.parent;
                    }
                }

                if (count == ks.Length)
                {
                    list.Add(node);
                }
            }
        }
        return list;

    }

    // 根据key找节点
    private Node FindNodeByKey(string key)
    {
        string[] ks = key.Split('.');
        for (int i = 0; i < m_node_list.Count; i++)
        {
            Node node = m_node_list[i];
            if (node.name == ks[ks.Length - 1])
            {
                // 判断父级
                Node tem = node;
                // 统计匹配到的次数
                int count = 1;
                for (int j = ks.Length - 2; j >= 0; j--)
                {
                    if (tem.parent.name == ks[j])
                    {
                        count++;
                        // 继续检查父级
                        tem = tem.parent;
                    }
                }

                if (count == ks.Length)
                {
                    return node;
                }
            }
        }
        return null;
    }

    // 保存到文件中
    public void Save()
    {
        StreamWriter stream = File.CreateText(this.m_path);
        for (int i = 0; i < m_node_list.Count; i++)
        {
            Node node = m_node_list[i];
            StringBuilder sb = new StringBuilder();

            // 放入前置空格
            for (int j = 0; j < node.space; j++)
            {
                sb.Append(" ");
            }

            sb.Append(node.name);
            if (node.hasValue)
            {
                if (node.name.Contains("%") || node.value == null)
                {
                    sb.Append(":");
                }
                else
                    sb.Append(": ");
            }
            if (node.value != null)
            {
                sb.Append(node.value);
            }
            stream.WriteLine(sb.ToString());
        }
        stream.Flush();
        stream.Close();
    }

    // 格式化
    public void Formatting()
    {
        // 先找出根节点
        List<Node> parentNode = new List<Node>();
        for (int i = 0; i < m_node_list.Count; i++)
        {
            Node node = m_node_list[i];
            if (node.parent == null)
            {
                parentNode.Add(node);
            }
        }

        List<Node> fNodeList = new List<Node>();
        // 遍历根节点
        for (int i = 0; i < parentNode.Count; i++)
        {
            Node node = parentNode[i];
            fNodeList.Add(node);
            FindChildren(node, fNodeList);
        }

        // 指针指向格式化后的
        m_node_list = fNodeList;
    }


    // 层级
    int tier = 0;
    // 查找孩子并进行分层
    private void FindChildren(Node node, List<Node> fNodeList)
    {
        // 当前层 默认第一级，根在外层进行操作
        tier++;

        for (int i = 0; i < m_node_list.Count; i++)
        {
            Node item = m_node_list[i];
            if (item.parent == node)
            {
                item.tier = tier;
                fNodeList.Add(item);
                FindChildren(item, fNodeList);
            }
        }

        // 走出一层
        tier--;
    }

    //查找前缀空格数量
    private int FindPreSpace(string str)
    {
        List<char> chars = str.ToList();
        int count = 0;
        foreach (char c in chars)
        {
            if (c == ' ' || c == '-')
            {
                count++;
            }
            else
            {
                break;
            }
        }
        return count;
    }

    // 根据缩进找上级
    private Node FindParent(int space)
    {

        if (m_node_list.Count == 0)
        {
            return null;
        }
        else
        {
            // 倒着找上级
            for (int i = m_node_list.Count - 1; i >= 0; i--)
            {
                Node node = m_node_list[i];
                if (node.space < space)
                {
                    return node;
                }
            }
            // 如果没有找到 返回null
            return null;
        }
    }

    // 节点类
    public class Node
    {
        // 名称
        public string name { get; set; }
        // 值
        public string value { get; set; }
        // 父级
        public Node parent { get; set; }
        // 前缀空格
        public int space { get; set; }
        // 所属层级
        public int tier { get; set; }

        //是否有值
        public bool hasValue = true;
    }
}

