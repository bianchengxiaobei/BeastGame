using UnityEngine;
using System.Collections;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataBeastBook 
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.28
// 模块描述：神兽图鉴数据
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 神兽图鉴数据
/// </summary>
public class DataBeastBook : GameData<DataBeastBook>
{
    public static readonly string fileName = "dataBeastBook";
    public int ID
    {
        get;
        set;
    }
    /// <summary>
    /// 当前页码
    /// </summary>
    public int Page
    {
        get;
        set;
    }
    /// <summary>
    /// 神兽id
    /// </summary>
    public int BeastId
    {
        get;
        set;
    }
    /// <summary>
    /// 神兽故事描述id
    /// </summary>
    public int StoryId
    {
        get;
        set;
    }
}
