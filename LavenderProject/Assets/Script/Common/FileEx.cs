using System;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using Object = UnityEngine.Object;


namespace Lavender.Common
{
    public static class FileEx
    {
        /// <summary>
        /// 保存文本
        /// </summary>
        /// <param name="text"></param>
        /// <param name="path"></param>
        public static void SaveText(string text, string path)
        {
            FileStream fs = null;
            try
            {
                using (fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    fs.SetLength(0);
                    using (StreamWriter sr = new StreamWriter(fs))
                    {
                        sr.Write(text);//开始写入值
                    }
                }
            }
            catch (Exception e)
            {
                if (fs != null)
                    fs.Close();
                throw e;
            }
        }

        public static string ReadText(string filePath, Encoding encoding = null)
        {
            if (filePath.Contains("\\"))
            {
                filePath = filePath.Replace("\\", "/");
            }
            if (!File.Exists(filePath))
            {
                Debug.LogError($"无法找到路径{filePath}");
                return null;
            }
            FileInfo file = new FileInfo(filePath);
            return ReadText(file, encoding);
        }

        /// <summary>
        /// 读取文本
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string ReadText(this FileInfo file, Encoding encoding = null)
        {
            var result = string.Empty;
            using (FileStream fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
            {
                if (encoding == null)
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        result = sr.ReadToEnd();
                    }
                }
                else
                {
                    using (StreamReader sr = new StreamReader(fs, encoding))
                    {
                        result = sr.ReadToEnd();
                    }
                }

            }
            return result;
        }


        /// <summary>
        /// 获取不带后缀的文件路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetFilePathWithoutExtention(string fileName)
        {
            if (fileName.Contains("."))
                return fileName.Substring(0, fileName.LastIndexOf('.'));
            return fileName;
        }

        public static bool Compare(string file1, string file2)
        {
            //  判断相同的文件是否被参考两次。
            if (file1 == file2)
            {
                return true;
            }

            int file1byte = 0;
            int file2byte = 0;

            using (FileStream fs1 = new FileStream(file1, FileMode.Open),
                fs2 = new FileStream(file2, FileMode.Open))
            {
                //  检查文件大小。如果两个文件的大小并不相同,则视为不相同。
                if (fs1.Length != fs2.Length)
                {
                    // 关闭文件。
                    fs1.Close();
                    fs2.Close();
                    return false;
                }

                //  逐一比较两个文件的每一个字节,直到发现不相符或已到达文件尾端为止。
                do
                {
                    // 从每一个文件读取一个字节。
                    file1byte = fs1.ReadByte();
                    file2byte = fs2.ReadByte();
                }

                while ((file1byte == file2byte) && (file1byte != -1));
                // 关闭文件。
                fs1.Close();
                fs2.Close();

            }


            //  返回比较的结果。在这个时候,只有当两个文件的内容完全相同时,"file1byte" 才会等于 "file2byte"。

            return ((file1byte - file2byte) == 0);

        }





        /// <summary>
        /// 使目录存在,Path 可以是目录名也可以是文件名
        /// </summary>
        /// <param name="path"></param>
        public static void MakeFileDirectoryExist(string path)
        {
            string root = Path.GetDirectoryName(path);
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
        }

        public static void DeleteFileSafely(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }

}
