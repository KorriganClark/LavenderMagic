using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Lavender
{
    public class BulletInfoCreater
    {
        [MenuItem("TestTools/创建大量大贴图")]
        public static void CreateLotsOfBigTexture()
        {
            System.Random rd = new System.Random();
            var texture = new Texture2D(2048,2048);
            for(int k = 0; k < 200; k++)
            {
                var color = new Color((float)(rd.Next(0, 255)/255.0), (float)(rd.Next(0, 255) / 255.0), (float)(rd.Next(0, 255) / 255.0), 0.5f);
                for(int i = 0; i < 2048; i++)
                    for(int j =0;j <2048; j++)
                    {
                        texture.SetPixel(i,j,color);
                    }
                string texturePath = Application.dataPath + "/Artres/Texture/" + $"ColorFul{k}.png";
                byte[] bytes = texture.EncodeToPNG();
                System.IO.File.WriteAllBytes(texturePath, bytes);
                Debug.Log("write to File over");
            }
            UnityEditor.AssetDatabase.Refresh(); //自动刷新资源
        }
        /// <summary>
        /// 创建大量材质
        /// </summary>
        [MenuItem("TestTools/创建大量材质")]
        public static void CreateLotsOfBulletInfo()
        {
            for (int k = 0; k < 200; k++)
            {
                string texturePath =  "Assets/Artres/Texture/" + $"ColorFul{k}.png";
                string matPath = "Assets/Artres/Material/" + $"ColorFulMat{k}.mat";

                var texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D)) as Texture2D;
                var mat = new Material(Shader.Find("Standard"));
                mat.mainTexture = texture;
                //创建的mat材质放到Assets文件夹下
                AssetDatabase.CreateAsset(mat, matPath);
            }
            UnityEditor.AssetDatabase.Refresh(); //自动刷新资源
        }
        /// <summary>
        /// 创建大量球形Prefab
        /// </summary>
        [MenuItem("TestTools/创建大量球形Prefab")]
        public static void CreateLotsOfSph()
        {
            for (int k = 0; k < 200; k++)
            {
                string matPath = "Assets/Artres/Material/" + $"ColorFulMat{k}.mat";
                string prefabPath = "Assets/Artres/Prefabs/" + $"Sph{k}.prefab";
                var mat = AssetDatabase.LoadAssetAtPath(matPath, typeof(Material)) as Material;
                GameObject target = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                target.GetComponent<MeshRenderer>().material = mat;
                PrefabUtility.SaveAsPrefabAsset(target, prefabPath);
                GameObject.DestroyImmediate(target);
            }
        }

        /// <summary>
        /// 创建大量球形Prefab
        /// </summary>
        [MenuItem("TestTools/创建大量球形Prefab产生依赖")]
        public static void CreateLotsOfSphMore()
        {
            int moreCount = 0;
            for(int k = 0; k < 500; k++)
            {
                System.Random rd = new System.Random();
                string prefabPath = "Assets/Artres/Prefabs/" + $"Sph{rd.Next(0,200)}.prefab";
                GameObject ori = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(GameObject)) as GameObject;
                GameObject target = GameObject.Instantiate(ori);
                for (int i = 1; i < rd.Next(1, 3); i++) 
                {
                    string childPath = "Assets/Artres/Prefabs/" + $"Sph{rd.Next(0, 200)}.prefab";
                    GameObject orii = AssetDatabase.LoadAssetAtPath(childPath, typeof(GameObject)) as GameObject;
                    GameObject child = GameObject.Instantiate(orii);
                    child.transform.SetParent(target.transform);

                }
                if(moreCount > 0)
                {
                    for (int i = 1; i < rd.Next(1, 3); i++)
                    {
                        string childPath = "Assets/Artres/Prefabs/" + $"SphMore{rd.Next(0, moreCount)}.prefab";
                        GameObject orii = AssetDatabase.LoadAssetAtPath(childPath, typeof(GameObject)) as GameObject;
                        GameObject child = GameObject.Instantiate(orii);
                        child.transform.SetParent(target.transform);
                    }
                }
                string prefabMorePath = "Assets/Artres/Prefabs/" + $"SphMore{moreCount}.prefab";
                PrefabUtility.SaveAsPrefabAsset(target, prefabMorePath);
                GameObject.DestroyImmediate(target);
                moreCount++;
            }
        }


    }
}
