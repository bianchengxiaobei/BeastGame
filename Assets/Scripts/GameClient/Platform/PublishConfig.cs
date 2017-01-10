using UnityEngine;
using Utility;
using Utility.Export;
using System;
using System.Xml;
using UnityAssetEx.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PublishConfig
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace GameClient.Data
{
    internal class PublishConfig : Singleton<PublishConfig>
    {
        private bool m_bSupportCommand = false;
        private bool m_bSelectMap = false;
        private bool m_bShowServerList = false;
        private bool m_bSupportHideUI = false;
        private IXLog m_log = XLog.GetLog<PublishConfig>();
        public bool IsSupportCommand
        {
            get
            {
                return this.m_bSupportCommand;
            }
        }
        public bool IsSelectMap
        {
            get
            {
                return this.m_bSelectMap;
            }
        }
        public bool IsShowServerList
        {
            get
            {
                return this.m_bShowServerList;
            }
        }
        public bool IsSupportHideUI
        {
            get
            {
                return this.m_bSupportHideUI;
            }
        }
        public void Init()
        {
            try
            {
                string strRelativePath = "config/publish.xml";
                XmlDocument xmlDocument = XmlResAdapter.GetXmlDocument(ResourceManager.GetFullPath(strRelativePath, false));
                this.OnLoadFinishEventHandler(xmlDocument);
            }
            catch (Exception ex)
            {
                this.m_log.Fatal(ex.ToString());
            }
        }
        private void OnLoadFinishEventHandler(XmlDocument xmlDoc)
        {
            if (null != xmlDoc)
            {
                try
                {
                    XmlNode xmlNode = xmlDoc.SelectSingleNode("Root");
                    foreach (XmlNode xmlNode2 in xmlNode.ChildNodes)
                    {
                        string text = xmlNode2.Name.ToLower();
                        if (text != null)
                        {
                            if (!(text == "supportcommand"))
                            {
                                if (!(text == "selectmap"))
                                {
                                    if (!(text == "showserverlist"))
                                    {
                                        if (text == "supporthideui")
                                        {
                                            this.m_bSupportHideUI = xmlNode2.InnerText.ToLower().Equals("true");
                                        }
                                    }
                                    else
                                    {
                                        this.m_bShowServerList = xmlNode2.InnerText.ToLower().Equals("true");
                                    }
                                }
                                else
                                {
                                    this.m_bSelectMap = xmlNode2.InnerText.ToLower().Equals("true");
                                }
                            }
                            else
                            {
                                this.m_bSupportCommand = xmlNode2.InnerText.ToLower().Equals("true");
                            }
                        }
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
