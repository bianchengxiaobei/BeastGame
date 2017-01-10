using UnityEngine;
using System.Collections;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataItemTamplates 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.11.16
// 模块描述：装备数据类
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 装备数据类
/// </summary>
public class DataItemTamplates : GameData<DataItemTamplates>
{
    public const string fileName = "DataItemTemplates";
    public int ID
    {
        get;set;
    }
    public string FileName
    {
        get;set;
    }
    public string FilePart
    {
        get;set;
    }
    public string AnchorNodeName
    {
        get;set;
    }
}
