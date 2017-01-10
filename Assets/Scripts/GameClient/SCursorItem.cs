using UnityEngine;
using System.Collections.Generic;
using Utility;
using Utility.Export;
using System.Xml;
using System;
using UnityAssetEx.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：SCursorItem
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace GameClient
{
    public struct SCursorItem
    {
        private string m_strTexture;
        private Vector2 m_vec2Hotspot;
        private bool m_bSoft;
        public string Texture
        {
            get
            {
                return this.m_strTexture;
            }
        }
        public Vector2 Hotspot
        {
            get
            {
                return this.m_vec2Hotspot;
            }
        }
        public bool Software
        {
            get
            {
                return this.m_bSoft;
            }
        }
        public SCursorItem(string strTexture, Vector2 hotspot, bool bSoft)
        {
            this.m_strTexture = strTexture;
            this.m_vec2Hotspot = hotspot;
            this.m_bSoft = bSoft;
        }
    }
    internal class CursorConfigMgr : Singleton<CursorConfigMgr>
    {
        private Dictionary<string, SCursorItem> m_dicCursorItem = new Dictionary<string, SCursorItem>();
        private IXLog m_log = XLog.GetLog<CursorConfigMgr>();
        public bool Init()
        {
            try
            {
                string strRelativePath = "config/cursor.xml";
                XmlDocument xmlDocument = XmlResAdapter.GetXmlDocument(ResourceManager.GetFullPath(strRelativePath, false));
                this.OnLoadFinishEventHandler(xmlDocument);
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
            return true;
        }
        public SCursorItem GetCursorItem(string strTexture)
        {
            SCursorItem result;
            if (this.m_dicCursorItem.ContainsKey(strTexture))
            {
                result = this.m_dicCursorItem[strTexture];
            }
            else
            {
                result = default(SCursorItem);
            }
            return result;
        }
        private void OnLoadFinishEventHandler(XmlDocument xmlDoc)
        {
            if (null != xmlDoc)
            {
                try
                {
                    XmlNode xmlNode = xmlDoc.SelectSingleNode("config");
                    foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
                    {
                        XmlElement xmlElement = (XmlElement)xmlNode2;
                        string attribute = xmlElement.GetAttribute("texture");
                        string attribute2 = xmlElement.GetAttribute("hotspot");
                        bool bSoft = false;
                        if (xmlElement.HasAttribute("soft"))
                        {
                            bSoft = xmlElement.GetAttribute("soft").Equals("true", StringComparison.InvariantCultureIgnoreCase);
                        }
                        Vector2 hotspot = UnityTools.String2Vector2(attribute2);
                        SCursorItem value = new SCursorItem(attribute, hotspot, bSoft);
                        this.m_dicCursorItem.Add(value.Texture, value);
                    }
                }
                catch (Exception ex)
                {
                    this.m_log.Fatal(ex.ToString());
                }
            }
        }
    }
}