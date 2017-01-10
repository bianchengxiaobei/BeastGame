using UnityEngine;
using System.Collections;
using System;
using System.IO;
using Utility;
using Utility.Export;
using UnityAssetEx.Export;
using GameData;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataManager
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.7
// 模块描述：数据管理器
//----------------------------------------------------------------*/
#endregion
namespace Client.Logic
{
    /// <summary>
    /// 数据管理器
    /// </summary>
    public class DataManager : Singleton<DataManager>
    {
        #region 字段
        private const string m_DataFile = "Config/csv.byte";
        private const string m_StringFile = "Config/csv.string";
        private const uint version = 1u;
        private IXLog m_log = XLog.GetLog<DataManager>();
        #endregion
        #region 属性
        #endregion
        #region 构造方法
        #endregion
        #region 公有方法
        public void Init()
        {
            try 
            {
                this.LoadCSV();
            }
            catch(Exception e)
            {
                this.m_log.Fatal(e.ToString());
            }
        }
        #endregion
        #region 私有方法
        private bool LoadCSV()
        {
            string fullPath = ResourceManager.GetFullPath(m_DataFile.ToLower(), false);
            bool result;
            if (!File.Exists(fullPath))
            {
                result = false;
            }
            else 
            {
                FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                BinaryReader binaryReader = new BinaryReader(fileStream);
                //读取是不是csv的文件，如果是的话，就加载。防止被别人修改数据
                if (binaryReader.ReadString() != "chenfuling")
                {
                    binaryReader.Close();
                    fileStream.Close();
                    result = false;
                }
                else 
                {
                    //先读取所有数据的总大小，然后读取byte[]数据
                    IDynamicPacket dynamicPacket = DynamicPacket.Create(binaryReader.ReadBytes(binaryReader.ReadInt32()));
                    //在byte[]数据里面，先读取多少份不同类型的数据
                    int num = dynamicPacket.ReadInt32();
                    int i = 0;
                    while (i < num)
                    {
                        //读取这份类型数据的总大小
                        int size = dynamicPacket.ReadInt32();
                        //根据大小读取这份数据byte[]
                        IDynamicPacket subPacket = DynamicPacket.Create(dynamicPacket.ReadBytes(size));
                        //读取出这份类型的数据类型字符串
                        string type = subPacket.ReadString();
                        //根据类型，解析出数据
                        switch (type)
                        {
                            case "table\\map\\maplist.csv":
                                //根据ushort的数量大小，再解析出List<T>数据
                                //DataMaplistManager.Instance.Deserialize(subPacket);
                                break;
                        }
                        i++;
                    }
                    binaryReader.Close();
                    fileStream.Close();
                    result = true;
                }
            }
            return result;
        }
        #endregion
    }
}