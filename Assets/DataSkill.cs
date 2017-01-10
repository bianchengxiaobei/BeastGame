using UnityEngine;
using System.Collections;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataSkill
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.10
// 模块描述：技能数据
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 技能数据
/// </summary>
public class DataSkill : GameData<DataSkill>
{
    public static string fileName = "dataSkill";
    public int ID
    {
        get;
        private set;
    }
    public int SkillType
    {
        get;
        private set;
    }
    public string Name
    {
        get;
        private set;
    }
    public bool IsActive
    {
        get;
        private set;
    }
    public int UseDisMin
    {
        get;
        private set;
    }
    public int UseDis
    {
        get;
        private set;
    }
    public int TargetType
    {
        get;
        private set;
    }
    public int Cooldown
    {
        get;
        private set;
    }
    public string SmallIcon
    {
        get;
        private set;
    }
    public string Icon
    {
        get;
        private set;
    }
    public string Desc
    {
        get;
        private set;
    }
    public string DescBook
    {
        get;
        private set;
    }
    public string Probability
    {
        get;
        private set;
    }
    public int Price
    {
        get;
        private set;
    }
    public int PriceDM
    {
        get;
        private set;
    }
}
