using UnityEngine;
using System.Collections.Generic;
using Utility.Export;
using System.Xml;
using System;
using UnityAssetEx.Export;
using Utility;
using Game;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：StringConfigMgr
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.19
// 模块描述：字符信息管理器
//----------------------------------------------------------------*/
#endregion
internal class StringConfigMgr
{
    private Dictionary<string, string> m_oDicAllStringData;
    private Dictionary<int, string> m_oDicAllErrStringData;
    private Dictionary<string, string> m_oDicPlayerIconStringData;
    private List<string> m_oListTips;
    private IXLog m_log = XLog.GetLog<StringConfigMgr>();
    private static StringConfigMgr gs_Singleton = new StringConfigMgr();
    public static StringConfigMgr singleton
    {
        get
        {
            return StringConfigMgr.gs_Singleton;
        }
    }
    public StringConfigMgr()
    {
        this.m_oDicAllStringData = new Dictionary<string, string>();
        this.m_oDicAllErrStringData = new Dictionary<int, string>();
        this.m_oDicPlayerIconStringData = new Dictionary<string, string>();
        this.m_oListTips = new List<string>();
    }
    public void Init()
    {
        try
        {
            string strRelativePath = "config/string.xml";
            XmlDocument xmlDocument = XmlResAdapter.GetXmlDocument(ResourceManager.GetFullPath(strRelativePath, false));
            this.OnLoadStringFinishEventHandler(xmlDocument);
            string strRelativePath2 = "config/errorcode.xml";
            XmlDocument xmlDocument2 = XmlResAdapter.GetXmlDocument(ResourceManager.GetFullPath(strRelativePath2, false));
            this.OnLoadErrorCodeStringFinishEventHandler(xmlDocument2);
            string strRelativePath3 = "config/tips.xml";
            XmlDocument xmlDocument3 = XmlResAdapter.GetXmlDocument(ResourceManager.GetFullPath(strRelativePath3, false));
            this.OnLoadTipsStringFinishEventHandler(xmlDocument3);
        }
        catch (Exception ex)
        {
            this.m_log.Fatal(ex.ToString());
        }
    }
    private void OnLoadStringFinishEventHandler(XmlDocument xmlDoc)
    {
        if (xmlDoc != null)
        {
            XmlNode xmlNode = xmlDoc.SelectSingleNode("table");
            XmlNodeList childNodes = xmlNode.ChildNodes;
            foreach (XmlNode xmlNode2 in childNodes)
            {
                XmlElement xmlElement = (XmlElement)xmlNode2;
                string attribute = xmlElement.GetAttribute("id");
                string attribute2 = xmlElement.GetAttribute("content");
                bool flag = this.m_oDicAllStringData.ContainsKey(attribute);
                if (flag)
                {
                }
                this.m_oDicAllStringData.Add(attribute, attribute2);
            }
        }
    }
    private void OnLoadErrorCodeStringFinishEventHandler(XmlDocument xmlDoc)
    {
        if (xmlDoc != null)
        {
            try
            {
                XmlNode xmlNode = xmlDoc.SelectSingleNode("errorcode");
                XmlNodeList xmlNodeList = xmlNode.SelectNodes("error");
                foreach (XmlNode xmlNode2 in xmlNodeList)
                {
                    XmlElement xmlElement = (XmlElement)xmlNode2;
                    int num = Convert.ToInt32(xmlElement.GetAttribute("value"));
                    string attribute = xmlElement.GetAttribute("comment");
                    if (!string.IsNullOrEmpty(attribute))
                    {
                        if (!this.m_oDicAllErrStringData.ContainsKey(num))
                        {
                            this.m_oDicAllErrStringData.Add(num, attribute);
                        }
                        else
                        {
                            this.m_log.Error("StringData Repeat! " + num);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Fatal(ex.Message);
            }
        }
    }
    private void OnLoadTipsStringFinishEventHandler(XmlDocument xmlDoc)
    {
        if (xmlDoc != null)
        {
            try
            {
                XmlNode xmlNode = xmlDoc.SelectSingleNode("table");
                XmlNodeList childNodes = xmlNode.ChildNodes;
                foreach (XmlNode xmlNode2 in childNodes)
                {
                    XmlElement xmlElement = (XmlElement)xmlNode2;
                    string attribute = xmlElement.GetAttribute("id");
                    string attribute2 = xmlElement.GetAttribute("content");
                    this.m_oListTips.Add(attribute2);
                }
            }
            catch (Exception ex)
            {
                XLog.Log.Fatal(ex.Message);
            }
        }
    }
    protected string TryGetString(string strId)
    {
        string result;
        if (this.m_oDicAllStringData.ContainsKey(strId))
        {
            result = this.m_oDicAllStringData[strId];
        }
        else
        {
            result = strId;
        }
        return result;
    }
    protected string TryGetErrString(int nId)
    {
        string result;
        if (this.m_oDicAllErrStringData.ContainsKey(nId))
        {
            string text = this.m_oDicAllErrStringData[nId];
            result = text;
        }
        else
        {
            result = nId.ToString();
        }
        return result;
    }
    /// <summary>
    /// 随机从提示列表中取出一条提示，如果没有提示，就显示"一起来战斗"
    /// </summary>
    /// <returns></returns>
    protected string TryGetTips()
    {
        int count = this.m_oListTips.Count;
        string result;
        if (count <= 0)
        {
            result = "一起来战斗";
        }
        else
        {
            int index = UnityEngine.Random.Range(0, count - 1);
            result = this.m_oListTips[index];
        }
        return result;
    }
    /// <summary>
    /// 根据字符id获取相应的字符
    /// </summary>
    /// <param name="strId">字符id</param>
    /// <returns></returns>
    public static string GetString(string strId)
    {
        return StringConfigMgr.singleton.TryGetString(strId);
    }
    public static string GetErrString(int nId)
    {
        return StringConfigMgr.singleton.TryGetErrString(nId);
    }
    /// <summary>
    /// 随机取出提示字符串
    /// </summary>
    /// <returns></returns>
    public static string GetTips()
    {
        return StringConfigMgr.singleton.TryGetTips();
    }
    public static string ConvertCustomText(string strInput)
    {
        string result;
        if (string.IsNullOrEmpty(strInput))
        {
            result = strInput;
        }
        else
        {
            result = strInput.Replace("#playername#", Singleton<PlayerRole>.singleton.Name);
        }
        return result;
    }
    public static string GetReplaceString(string strInput)
    {
        if (!string.IsNullOrEmpty(strInput))
        {
            strInput = strInput.Replace("#playerlevel#", Singleton<PlayerRole>.singleton.Level.ToString());
            strInput = strInput.Replace("#beastcount#", Singleton<PlayerRole>.singleton.GetActiveBeastCount().ToString());
        }
        return strInput;
    }
}
