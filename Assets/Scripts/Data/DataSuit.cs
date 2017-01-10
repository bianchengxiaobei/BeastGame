using UnityEngine;
using System.Collections;
using Utility.Export;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataSuit
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：皮肤数据
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 皮肤数据
/// </summary>
public class DataSuit : GameData<DataSuit>
{
    public static readonly string fileName = "dataSuit";
    public int ID
    {
        get;
        private set;
    }
    public string Name
    {
        get;
        private set;
    }
    public string HeadName
    {
        get;
        private set;
    }
    /// <summary>
    /// 皮肤原画名字Texture/Beast/...
    /// </summary>
    public string PicName
    {
        get;
        private set;
    }
    /// <summary>
    /// 神兽基础模型路径Data/Model/BeastModel
    /// </summary>
    public string BasePath
    {
        get;
        private set;
    }
    /// <summary>
    /// 皮肤模型路径Data/Model/BeastModel
    /// </summary>
    public string SuitPath
    {
        get;
        private set;
    }
    public int BeastId
    {
        get;
        private set;
    }
    public int Merge
    {
        get;
        private set;
    }
    /// <summary>
    /// 贴图名称，多张用","分割Texture/SuitTexture/...
    /// </summary>
    public string Texture
    {
        get;
        private set;
    }
    public string ShowAction
    {
        get;
        private set;
    }
    public int ShowId
    {
        get;
        private set;
    }
    public bool IsLook
    {
        get;
        private set;
    }
}
