using UnityEngine;
using System.Collections;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：GenericXmlSerializer
// 创建者：chen
// 修改者列表：
// 创建日期：#CREATIONDATE#
// 模块描述：
//----------------------------------------------------------------*/
#endregion
namespace Utility.Export
{
    public class GenericXmlSerializer
    {
        private static XPathNavigator GetXPathNavigator(object obj)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            XPathDocument xPathDocument = null;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (TextWriter textWriter = new StreamWriter(memoryStream))
                {
                    xmlSerializer.Serialize(textWriter, obj);
                    memoryStream.Seek(0L, SeekOrigin.Begin);
                    xPathDocument = new XPathDocument(memoryStream);
                }
            }
            return xPathDocument.CreateNavigator();
        }
        public static T LoadFromXmlFile<T>(string fileName) where T : class
        {
            StreamReader streamReader = new StreamReader(fileName);
            return GenericXmlSerializer.ReadFromXmlString<T>(streamReader.ReadToEnd());
        }
        public static T ReadFromXmlString<T>(string xmlString) where T : class
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString));
            return xmlSerializer.Deserialize(stream) as T;
        }
        public static void SaveToXmlFile(object obj, string fileName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
            using (XmlWriter xmlWriter = XmlWriter.Create(fileName, new XmlWriterSettings
            {
                Indent = true
            }))
            {
                xmlSerializer.Serialize(xmlWriter, obj);
                xmlWriter.Close();
            }
        }
        public static string WriteToXmlString(object obj)
        {
            XPathNavigator xPathNavigator = GenericXmlSerializer.GetXPathNavigator(obj);
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            MemoryStream memoryStream = new MemoryStream();
            using (XmlWriter xmlWriter = XmlWriter.Create(memoryStream, xmlWriterSettings))
            {
                xPathNavigator.WriteSubtree(xmlWriter);
                xmlWriter.Close();
            }
            StreamReader streamReader = new StreamReader(memoryStream);
            return streamReader.ReadToEnd();
        }
    }
}