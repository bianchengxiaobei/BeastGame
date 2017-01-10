using UnityEngine;
using System.Collections.Generic;
using Utility;
using UnityAssetEx.Export;
using System.IO;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：WordFilter
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：字符过滤器
//----------------------------------------------------------------*/
#endregion
namespace Client.Logic
{
    public class WordFilter : Singleton<WordFilter>
    {
        private string[] m_StringFilters;//存的都是小写的字符串
        private Dictionary<int, string> m_Replacers = new Dictionary<int, string>();//key=>length,value=>多少个长度的*
        public void Init()
        {
            string fullPath = ResourceManager.GetFullPath("config/table/language_filter.txt", false);
            this.m_StringFilters = File.ReadAllLines(fullPath);
            for (int i = 0; i < this.m_StringFilters.Length; i++)
            {
                string text = this.m_StringFilters[i].ToLower();//这行字转成小写
                this.m_StringFilters[i] = text;
                int length = text.Length;
                string text2 = new string('*', length);
                if (text2.Equals(text))
                {
                    this.m_StringFilters[i] = null;
                }
                else
                {
                    if (!this.m_Replacers.ContainsKey(length))
                    {
                        this.m_Replacers.Add(length, text2);//也就是replace里面存的是多少个不同长度的*
                    }
                }
            }
        }
        /// <summary>
        /// 字符过滤，替换非法字符为*
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns>是否没有非法字符，为真就是没有非法</returns>
        public bool FilterString(ref string originalString)
        {
            string a = originalString;
            string text = originalString.ToLower();
            string[] stringFilters = this.m_StringFilters;
            for (int i = 0; i < stringFilters.Length; i++)
            {
                string text2 = stringFilters[i];
                if (!string.IsNullOrEmpty(text2))
                {
                    int num = 0;
                    while (true)
                    {
                        num = text.IndexOf(text2, num);
                        if (-1 == num)
                        {
                            break;
                        }
                        string str = originalString.Substring(0, num);
                        string str2 = originalString.Substring(num + text2.Length);
                        originalString = str + this.m_Replacers[text2.Length] + str2;
                        str = text.Substring(0, num);
                        str2 = text.Substring(num + text2.Length);
                        text = str + this.m_Replacers[text2.Length] + str2;
                    }
                }
            }
            return a == originalString;
        }
    }
}
