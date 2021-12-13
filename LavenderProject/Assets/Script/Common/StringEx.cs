using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Reflection;


namespace Lavender.Common
{
    /// <summary>
    /// 通过编写方法并且添加属性可以做到转换至String 如：
    /// 
    /// <example>
    /// [ToString]
    /// public static string ConvertToString(TestObj obj)
    /// </example>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ToString : Attribute { }

    /// <summary>
    /// 通过编写方法并且添加属性可以做到转换至String 如：
    /// 
    /// <example>
    /// [FromString]
    /// public static TestObj ConvertFromString(string str)
    /// </example>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class FromString : Attribute { }


    public static class StringEx
    {
        public static char Spriter1 = ',';
        public static char Spriter2 = ':';

        public static char FBracket1 = '(';
        public static char BBracket1 = ')';


        public static bool LangIsSomeZone(this string input, string range)
        {
            //Regex r = new Regex(@"^[\u4e00-\u9fa5]");
            Regex r = new Regex(range);
            return r.IsMatch(input);
        }

        /// <summary>
        /// 拆分并去除空格
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] SplitAndTrim(this string str, params char[] separator)
        {
            var res = str.Split(separator);
            for (var i = 0; i < res.Length; i++)
            {
                res[i] = res[i].Trim().RemoveString("\t");
            }
            return res;
        }

        /// <summary>
        /// 替换第一个匹配值
        /// </summary>
        /// <param name="input"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <param name="startAt"></param>
        /// <returns></returns>
        public static string ReplaceFirst(this string input, string oldValue, string newValue, int startAt = 0)
        {
            int index = input.IndexOf(oldValue, startAt);
            if (index < 0)
            {
                return input;
            }
            return (input.Substring(0, index) + newValue + input.Substring(index + oldValue.Length));
        }

        /// <summary>
        /// 删除特定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string RemoveString(this string str, params string[] targets)
        {
            StringBuilder strBuilder = SharedStringBuilder.Get();
            strBuilder.Append(str);
            for (int i = 0; i < targets.Length; i++)
            {
                strBuilder.Replace(targets[i], string.Empty);
            }
            return strBuilder.ToString();
        }

        /// <summary>
        /// 查找在两个字符串中间的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="front"></param>
        /// <param name="behined"></param>
        /// <returns></returns>
        public static string FindBetween(this string str, string front, string behined)
        {
            var startIndex = str.IndexOf(front, StringComparison.Ordinal) + front.Length;
            var endIndex = str.IndexOf(behined, StringComparison.Ordinal);
            if (startIndex < 0 || endIndex < 0 || startIndex > endIndex)
            {
                if (startIndex < 0)
                {
                    Debug.LogError("找不到前者:" + front);
                    return string.Empty;
                }
                if (endIndex < 0)
                {
                    Debug.LogError("找不到后者:" + behined);
                    return string.Empty;
                }
                if (endIndex < startIndex)
                {
                    Debug.LogError("后者小于前者,前者：" + front + "后者:" + behined);
                    return string.Empty;
                }
                return str;
            }
            return str.Substring(startIndex, endIndex - startIndex);
        }

        /// <summary>
        /// 查找在字符后面的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="front"></param>
        /// <returns></returns>
        public static string FindAfter(this string str, string front)
        {
            var startIndex = str?.IndexOf(front) + front.Length ?? 0;
            if (startIndex < 0)
                return str;
            return str?.Substring(startIndex);
        }

        public static string SubString(string str, int startIndex, int length)
        {
            return str.Substring(startIndex, length);
        }

        public static int Length(string str)
        {
            return str.Length;
        }

        public static string GetDateString()
        {
            return $"{DateTime.Now.ToFileTime()}";
        }

        public static void PrintBadChar(string value)
        {
            var indexOfAny = value.IndexOfAny(Path.GetInvalidPathChars());
            if (indexOfAny < 0)
                return;
            Debug.LogError(value[indexOfAny]);
        }

        public static bool StringEquals(string v1, string v2)
        {
            return v1.Equals(v2);
        }

        public static string StringTrim(string value)
        {
            return value.Trim();
        }

        /// <summary>
        /// 空格不换行
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceSpace(string value)
        {
            return value.Replace(" ", "\u00A0");
        }


        public static bool IsInt(string str)
        {
            return Regex.IsMatch(str.Trim(), "\\d+");
        }

        public static bool IsFloat(string str)
        {
            return Regex.IsMatch(str.Trim(), "\\d+.\\d+");
        }

        private static string _illegalRegexContent = "";
        private static bool _isEmptyRegex = true;

        public static string IllegalRegexContent
        {
            get => _illegalRegexContent;
            set
            {
                _illegalRegexContent = value;
                _illegalRegex = new Regex(_illegalRegexContent);

                _isEmptyRegex = string.IsNullOrEmpty(value);
            }
        }

        private static Regex _illegalRegex = new Regex(_illegalRegexContent);
        public static string ReplaceIllegalChar(string source, string replacement = "")
        {
            if (_isEmptyRegex) return source;

            return _illegalRegex.Replace(source, replacement);
        }
        public static bool IsIllegalChar(char c)
        {
            if (_isEmptyRegex) return false;

            return _illegalRegex.IsMatch(c.ToString());
        }
        public static string GetOneLineString(this string source)
        {
            return source
                .Replace("\n", "\\n")
                .Replace("\r", "\\r")
                .Replace("\t", "\\t").TrimEnd();
        }

        /// <summary>
        /// 将一行数据转换为原始数据
        /// </summary>
        /// <param name="oneline"></param>
        /// <returns></returns>
        public static string ParseFromOneLineString(this string oneline)
        {
            return oneline
                .Replace("\\n", "\n")
                .Replace("\\r", "\r")
                .Replace("\\t", "\t");
        }
    }

    public class SharedStringBuilder
    {
        private static readonly StringBuilder builder = new StringBuilder();

        public static StringBuilder Get()
        {
            builder.Clear();
            return builder;
        }
    }

}
