using UnityEngine;
using System.Collections;
using Utility.Export;
using Client.Data;
#region 模块信息
/*----------------------------------------------------------------
// 模块名：DataBeastList
// 创建者：chen
// 修改者列表：
// 创建日期：2016.3.6
// 模块描述：神兽数据列表
//----------------------------------------------------------------*/
#endregion
/// <summary>
/// 神兽数据列表
/// </summary>
public class DataBeastlist : GameData<DataBeastlist>
{
    static public readonly string fileName = "beastList";
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
    public string NickName
    {
        get;
        private set;
    }
    public string ModelFile
    {
        get;
        private set;
    }
    public string IconFile
    {
        get;
        private set;
    }
    public int PassiveSkill
    {
        get;
        set;
    }
    public int Skill_1
    {
        get;
        private set;
    }
    public int Skill_2
    {
        get;
        private set;
    }
    public int Skill_3
    {
        get;
        private set;
    }
    public int Skill_4
    {
        get;
        private set;
    }
    public string ExtraSkill
    {
        get;
        private set;
    }
    public int Hp
    {
        get;
        private set;
    }
    /// <summary>
    /// 最大移动距离
    /// </summary>
    public int Move
    {
        get;
        private set;
    }
    public int Type
    {
        get;
        private set;
    }
    public string TypeMsg
    {
        get;
        private set;
    }
    public int MoneyRequired
    {
        get;
        private set;
    }
    public int TicketRequired
    {
        get;
        private set;
    }
    public string DescStory
    {
        get;
        private set;
    }
    public float Movementspeed
    {
        get;
        private set;
    }
    public int Operation
    {
        get;
        private set;
    }
    public int Control
    {
        get;
        private set;
    }
    public int Existence
    {
        get;
        private set;
    }
    public int Auxiliary
    {
        get;
        private set;
    }
    public int Output
    {
        get;
        private set;
    }
    public int Endurance
    {
        get;
        private set;
    }
    public int DeadEffect
    {
        get;
        private set;
    }
    public float DeadDelay
    {
        get;
        private set;
    }
    public float DeadFadeout
    {
        get;
        private set;
    }
    public float DeadFadeoutDepth
    {
        get;
        private set;
    }
    public string RecommendEquip
    {
        get;
        private set;
    }
    public string RecommendPartner
    {
        get;
        private set;
    }
    public string StrategyDesc
    {
        get;
        private set;
    }
    public string Heroinfo
    {
        get;
        private set;
    }
    public int AttackEffectId
    {
        get;
        private set;
    }
    public int BeAttackEffectId
    {
        get;
        private set;
    }
    public string ConvertItem
    {
        get;
        private set;
    }
}