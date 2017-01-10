using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Xml;
using Utility;
using Utility.Export;
using UnityAssetEx.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：UITipConfigMgr
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace GameClient.Data
{
    internal class UITipConfigMgr : Singleton<UITipConfigMgr>
    {
        private Dictionary<string, string> strings = new Dictionary<string, string>();
        private IXLog m_log = XLog.GetLog<UITipConfigMgr>();
        public void Init()
        {
            string strRelativePath = "config/uitips.string";
            this.ReadFile(ResourceManager.GetFullPath(strRelativePath, false));
        }
        /// <summary>
        /// 根据key（ui名字）来获取tip
        /// </summary>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public string GetTip(string strKey)
        {
            string result;
            if (string.IsNullOrEmpty(strKey))
            {
                result = null;
            }
            else
            {
                string text = null;
                if (this.strings.TryGetValue(strKey, out text))
                {
                    result = text;
                }
                else
                {
                    result = null;
                }
            }
            return result;
        }
        private bool ReadFile(string file)
        {
            bool result;
            if (!File.Exists(file))
            {
                this.m_log.Error("false == File.Exists(file):" + file);
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
        /// 以空格为分割符，前面名称为key，后面tip为value
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
    }
}