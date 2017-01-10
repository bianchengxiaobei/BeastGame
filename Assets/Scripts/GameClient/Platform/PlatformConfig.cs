using UnityEngine;
using Utility;
using System;
using UnityPlugin.Export;
using Utility.Export;
using System.Xml;
using UnityAssetEx.Export;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：PlatformConfig
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace GameClient.Data
{
    public class PlatformConfig : Singleton<PlatformConfig>
    {
        private EnumPlatformType m_ePlatformType = EnumPlatformType.ePlatformType_KZ;
        private string m_strPayUrl = "http://passport.kongzhong.com/v/billing/pay/game_bank";
        private string m_strRegisterUrl = "http://www.lybns.com/index.php/member/register.html";
        private string m_strForgetUrl = "http://www.lybns.com/index.php/member/forget.html";
        private IXLog m_log = XLog.GetLog<PublishConfig>();
        /// <summary>
        /// 平台类型：空中网，百度，无
        /// </summary>
        public EnumPlatformType EPlatformType
        {
            get
            {
                return this.m_ePlatformType;
            }
        }
        public string PayUrl
        {
            get
            {
                return this.m_strPayUrl;
            }
        }
        public string AnnounceUrl
        {
            get
            {
                return this.m_strAnnounceUrl;
            }
        }
        public string RegisterUrl
        {
            get
            {
                return this.m_strRegisterUrl;
            }
        }
        public string ForgetUrl
        {
            get
            {
                return this.m_strForgetUrl;
            }
        }
        private string m_strAnnounceUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 初始化平台url，如PayUrl，RegisterURl，ForgetUrl等
        /// </summary>
        public void Init()
        {
            try
            {
                string strRelativePath = "config/platform.xml";
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
                            if (!(text == "type"))
                            {
                                if (!(text == "payurl"))
                                {
                                    if (!(text == "registerurl"))
                                    {
                                        if (!(text == "forgeturl"))
                                        {
                                            if (text == "announceurl")
                                            {
                                                this.m_strAnnounceUrl = xmlNode2.InnerText;
                                            }
                                        }
                                        else
                                        {
                                            this.m_strForgetUrl = xmlNode2.InnerText;
                                        }
                                    }
                                    else
                                    {
                                        this.m_strRegisterUrl = xmlNode2.InnerText;
                                    }
                                }
                                else
                                {
                                    this.m_strPayUrl = xmlNode2.InnerText;
                                }
                            }
                            else
                            {
                                this.m_ePlatformType = (EnumPlatformType)Convert.ToInt32(xmlNode2.InnerText);
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
