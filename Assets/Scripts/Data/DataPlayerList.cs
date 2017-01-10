using UnityEngine;
using System.Collections;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataPlayerList 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.9.19
// 模块描述：角色配置信息表
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 角色配置信息表
/// </summary>
public class DataPlayerList : GameData<DataPlayerList>
{
    public static readonly string fileName = "dataPlayerList";
    public int ID 
    {
        get;
        set;
    }
    public string Name
    {
        get;
        set;
    }
    public string IconFile 
    {
        get;
        set;
    }
}
