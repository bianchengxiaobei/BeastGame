using UnityEngine;
using System.IO;
using System.Collections.Generic;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：StringFileReader
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.19
// 模块描述：文本字符管理器（key，value通过空格分割）
//----------------------------------------------------------------*/
#endregion
namespace GameData
{
    public class StringFileReader
    {
        public const string ErrorFlag = "null=";
        private Dictionary<string, string> strings;
        /// <summary>
        /// 获取文本字符
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetString(string key)
        {
            string result;
            if (this.strings != null && this.strings.ContainsKey(key))
            {
                result = this.strings[key];
            }
            else
            {
                result = "null=" + key;
            }
            return result;
        }
        /// <summary>
        /// 初始化从文本读出的key，value
        /// </summary>
        /// <param name="r"></param>
        private void ParseString(StreamReader r)
        {
            this.strings = new Dictionary<string, string>();
            string text;
            while ((text = r.ReadLine()) != null)
            {
                int num = text.IndexOf(' ');
                if (num >= 0)
                {
                    string key = text.Substring(0, num);
                    string value = text.Substring(num + 1);
                    this.strings[key] = value;
                }
            }
        }
        /// <summary>
        /// 读取文本
        /// </summary>
        /// <param name="file">文本路径</param>
        /// <returns>是否成功</returns>
        public bool ReadFile(string file)
        {
            bool result;
            if (!File.Exists(file))
            {
                result = false;
            }
            else
            {
                using (StreamReader streamReader = File.OpenText(file))
                {
                    this.ParseString(streamReader);
                }
                result = true;
            }
            return result;
        }
        /// <summary>
        /// 读取资源文本
        /// </summary>
        /// <param name="file">资源文本路径</param>
        /// <returns>是否成功</returns>
        public bool ReadResource(string file)
        {
            TextAsset textAsset = Resources.Load(file, typeof(TextAsset)) as TextAsset;
            bool result;
            if (null != textAsset)
            {
                MemoryStream stream = new MemoryStream(textAsset.bytes);
                StreamReader r = new StreamReader(stream);
                this.ParseString(r);
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}
