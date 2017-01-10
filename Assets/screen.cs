using UnityEngine;
using System.Xml;
using System.Collections.Generic;
using Client.Data;
using System;
using Utility;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：screen
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public class screen : MonoBehaviour
{
    /*public string path;
    private string dic = "F://chenfuling/龙翼编年史/龙翼编年史/bin";
    void Start()
    {
        if (!string.IsNullOrEmpty(path))
        {
            path = dic + "/" + path;
            XmlDocument xmlDocument = XmlResAdapter.GetXmlDocument(path);
            print(xmlDocument.InnerXml);
        }
    }*/
    void Start()
    {
        Debug.Log(Screen.resolutions.Length);
    }

}
