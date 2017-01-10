using UnityEngine;
using System;
using System.Xml;
using System.IO;
using System.Collections;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：XmlResAdapter
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
public static class XmlResAdapter 
{
    private static string s_encryptedKey = "lybnsxml";
    private static readonly int s_max_buffer_size = 5242880;
    private static byte[] s_buffer = new byte[XmlResAdapter.s_max_buffer_size];
    public static XmlDocument GetXmlDocument(string absoluteFilePath)
    {
        XmlDocument result;
        if (!File.Exists(absoluteFilePath))
        {
            Debug.Log("null");
            result = null;
        }
        else
        {
            XmlDocument xmlDocument = new XmlDocument();
            FileStream fileStream = new FileStream(absoluteFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader binaryReader = new BinaryReader(fileStream);
            uint num = binaryReader.ReadUInt32();
            if (4294967295u == num)
            {
                DateTime now = DateTime.Now;
                for (int i = 0; i <= 5120; i++)
                {
                    int num2 = binaryReader.Read(XmlResAdapter.s_buffer, i * 1024, 1024);
                    if (num2 < 1024)
                    {
                        byte[] array = new byte[1024 * i + num2];
                        Array.Copy(XmlResAdapter.s_buffer, array, array.Length);
                        string xml = EncryptString.Decrypt(array, XmlResAdapter.s_encryptedKey);
                        xmlDocument.LoadXml(xml);
                        if ((DateTime.Now - now).TotalMilliseconds > 50.0)
                        {
                        }
                        goto IL_129;
                    }
                }
                fileStream.Close();
                binaryReader.Close();
                result = null;
                return result;
            }
            xmlDocument.Load(absoluteFilePath);
        IL_129:
            fileStream.Close();
            binaryReader.Close();
            result = xmlDocument;
        }
        return result;
    }
}
