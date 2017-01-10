using UnityEngine;
using System.Collections;
using Utility.Export;
using GameData;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：TaskMain
// 创建者：chen
// 修改者列表：
// 创建日期：2016.2.19
// 模块描述：主线任务
//----------------------------------------------------------------*/
#endregion
public class TaskMain : IDynamicData
{
    /// <summary>
    /// 主线任务id
    /// </summary>
    public int ID
    {
        get;
        private set;
    }
    /// <summary>
    /// 主线任务名称
    /// </summary>
    public string Name
    {
        get;
        private set;
    }
    /// <summary>
    /// 主线任务描述
    /// </summary>
    public string Desc
    {
        get;
        private set;
    }
    /// <summary>
    /// 通关经验奖励
    /// </summary>
    public int PlayerExpReward
    {
        get;
        private set;
    }
    /// <summary>
    /// 通过金钱奖励
    /// </summary>
    public int MoneyReward
    {
        get;
        private set;
    }
    public string GoTo
    {
        get;
        private set;
    }

    public void Serialize(IDynamicPacket packet)
    {
        packet.Write(this.ID);
    }
    public void Deserialize(IDynamicPacket packet)
    {
 
    }
    public void CorrectString(StringFileReader reader)
    {
 
    }
}
